using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace StudentRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAttendance : ContentPage
    {
        private List<AttendanceRecord> attendanceRecords = new List<AttendanceRecord>();
        public const string attendance_add = "http://192.168.56.1/StudentRecordApp/attendance-add.php";
        public const string class_update = "http://192.168.56.1/StudentRecordApp/class-students.php";
        private string classId;
        public AddAttendance(string classId)
        {
            InitializeComponent();
            this.classId = classId;
            FetchStudents(classId);
        }

        private async void FetchStudents(string classId)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"{class_update}?class_id={classId}");

                // Deserialize the response and process the data as needed
                var result = JsonConvert.DeserializeObject<StudentsResponse>(response);

                if (result.status)
                {
                    // Set the ItemsSource of the ListView to the list of students
                    StudentsListView.ItemsSource = result.students;
                }
                else
                {
                    // Handle case where no students are found
                    // You might want to display an error message or handle it based on your requirements
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }
        private async void OnSaveAllClicked(object sender, EventArgs e)
        {
            foreach (var item in StudentsListView.ItemsSource)
            {
                var student = (StudentItem)item;

                // Find the Picker for the current student
                var cell = StudentsListView.TemplatedItems.First(c => c.BindingContext == item);
                var picker = cell.FindByName<Picker>("StatusPicker");

                if (picker != null && picker.SelectedItem != null)
                {
                    var status = picker.SelectedItem.ToString();

                    var attendanceRecord = new AttendanceRecord
                    {
                        StudentId = student.id,
                        ClassId = classId,
                        AttendanceDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        Status = status
                    };

                    attendanceRecords.Add(attendanceRecord);
                }
            }

            // Save all attendance records
            await SaveAllAttendance();
        }

        private async Task SaveAllAttendance()
        {
            foreach (var record in attendanceRecords)
            {
                await RecordAttendance(record);
            }

            // Clear the list of attendance records after saving
            attendanceRecords.Clear();

            // Display a success message
            await DisplayAlert("Success", "Attendance recorded successfully for all students", "OK");

            // Navigate back
            await Navigation.PopAsync();
        }

        private async Task RecordAttendance(AttendanceRecord record)
        {
            try
            {
                // Create a dictionary to hold the data with correct parameter names
                var data = new Dictionary<string, string>
        {
            { "student_id", record.StudentId },
            { "class_id", record.ClassId },
            { "attendance_date", record.AttendanceDate },
            { "status", record.Status }
        };

                // Convert the dictionary to JSON
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the attendance record to the server
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(attendance_add, content);

                if (response.IsSuccessStatusCode)
                {
                    // Attendance recorded successfully
                    System.Diagnostics.Debug.WriteLine($"Attendance recorded successfully for student {record.StudentId}");
                }
                else
                {
                    // Handle the case where the server request was not successful
                    System.Diagnostics.Debug.WriteLine($"Failed to record attendance for student {record.StudentId}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }

    }
    public class AttendanceRecord
    {
        public string StudentId { get; set; }
        public string ClassId { get; set; }
        public string AttendanceDate { get; set; }
        public string Status { get; set; }
    }
}