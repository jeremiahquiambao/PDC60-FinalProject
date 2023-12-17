using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace StudentRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewStudent : ContentPage
    {
        private string studentId;
        public const string student_searchid = "http://192.168.56.1/StudentRecordApp/student-searchID.php";
        public const string history_search = "http://192.168.56.1/StudentRecordApp/history-search.php";
        public const string history_delete = "http://192.168.56.1/StudentRecordApp/history-delete.php";
        public const string attendance_read = "http://192.168.56.1/StudentRecordApp/attendance-read.php";
        public const string attendance_delete = "http://192.168.56.1/StudentRecordAppl/attendance-delete.php";

        public ViewStudent(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            FetchStudentData();
            FetchAcademicHistory();
            FetchAttendanceData();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchStudentData();
            FetchAcademicHistory();
            FetchAttendanceData();
        }
        private async void FetchStudentData()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"{student_searchid}?id={studentId}");

                // Deserialize the response and process the student data
                var result = JsonConvert.DeserializeObject<StudentResponse>(response);

                if (result.status)
                {
                    IdLabel.Text = $"Student ID: {result.student.id}";
                    GradeLevelLabel.Text = $"Year Level: {result.student.grade_level}";
                    FirstNameLabel.Text = $"First Name: {result.student.first_name}";
                    LastNameLabel.Text = $"Last Name: {result.student.last_name}";
                    DateOfBirthLabel.Text = $"Date of Birth: {result.student.date_of_birth}";
                    EmailLabel.Text = $"Email: {result.student.email}";
                    PhoneLabel.Text = $"Phone: {result.student.phone}";
                    AddressLabel.Text = $"Address: {result.student.address}";
                }
                else
                {
                    // Handle case where the student data is not found
                    // You might want to display an error message or handle it based on your requirements
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }
        private async void FetchAcademicHistory()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"{history_search}?student_id={studentId}");

                // Deserialize the response and process the academic history data
                var result = JsonConvert.DeserializeObject<AcademicHistoryResponse>(response);

                if (result.status)
                {
                    // Clear existing items in the ListView
                    AcademicHistoryListView.ItemsSource = null;

                    // Set the ItemsSource of the ListView to the list of academic history
                    AcademicHistoryListView.ItemsSource = result.academic_history;
                }
                else
                {
                    // Clear the ListView if no academic history is found
                    AcademicHistoryListView.ItemsSource = null;

                    // Handle case where no academic history is found
                    // You might want to display an error message or handle it based on your requirements
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }

        private async void OnAddAcademicHistoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddHistory(studentId));
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var selectedAcademicHistory = (AcademicHistoryItem)menuItem.CommandParameter;

            if (selectedAcademicHistory != null)
            {
                // Navigate to the EditHistory page and pass the history_id as a parameter
                await Navigation.PushAsync(new EditHistory(selectedAcademicHistory.id));
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var selectedAcademicHistory = (AcademicHistoryItem)menuItem.CommandParameter;

            if (selectedAcademicHistory != null)
            {
                // Display a confirmation dialog
                bool answer = await DisplayAlert("Confirmation", "Do you want to delete this record?", "Yes", "No");

                // Check the user's response
                if (answer)
                {
                    // Call the DeleteAcademicHistory method passing the academic history id
                    await DeleteAcademicHistory(selectedAcademicHistory.id);
                }
                // If the user clicks "No", you can handle it accordingly
            }
        }

        // Assuming you have a method to delete academic history
        private async Task DeleteAcademicHistory(string academicHistoryId)
        {
            try
            {
                // Create a dictionary to hold the data
                var data = new Dictionary<string, string>
        {
            { "id", academicHistoryId }
        };

                // Convert the dictionary to JSON
                var jsonData = JsonConvert.SerializeObject(data);

                // Create the StringContent with JSON data
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the delete request to the server with the data in the request body
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(history_delete, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Academic history deleted successfully
                        await DisplayAlert("Success", "Academic history deleted successfully", "OK");
                        FetchAcademicHistory();
                    }
                    else
                    {
                        // Handle the case where the server request was not successful
                        await DisplayAlert("Error", "Failed to delete academic history", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
                await DisplayAlert("Error", "An error occurred", "OK");
            }
        }
        private async void OnEditStudentDetailsClicked(object sender, EventArgs e)
        {

            // Navigate to the EditStudent page and pass only the student ID as a parameter
            await Navigation.PushAsync(new EditStudent(studentId));
        }
        private async void FetchAttendanceData()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"{attendance_read}?student_id={studentId}");

                // Deserialize the response and process the attendance data
                var result = JsonConvert.DeserializeObject<AttendanceResponse>(response);

                // Create a new ObservableCollection to hold attendance data
                var attendanceData = new ObservableCollection<AttendanceItem>();

                if (result.status)
                {
                    // Iterate through the result data and add items to the ObservableCollection
                    foreach (var attendanceItem in result.data)
                    {
                        attendanceData.Add(attendanceItem);
                    }
                }

                // Set the ObservableCollection as the ItemsSource for the ListView
                AttendanceListView.ItemsSource = attendanceData;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }

        private async void OnEditAttendanceClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var selectedAttendance = menuItem?.CommandParameter as AttendanceItem;

            if (selectedAttendance != null)
            {
                // Navigate to the EditAttendance page and pass the attendance_id as a parameter
                await Navigation.PushAsync(new EditAttendance(selectedAttendance.id));
            }
        }

        private async void OnDeleteAttendanceClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var selectedAttendance = menuItem?.CommandParameter as AttendanceItem;

            if (selectedAttendance != null)
            {
                // Display a confirmation dialog
                bool answer = await DisplayAlert("Confirmation", "Do you want to delete this record?", "Yes", "No");

                // Check the user's response
                if (answer)
                {
                    // Call the DeleteAttendance method passing the attendance date
                    await DeleteAttendance(selectedAttendance.id);
                }
                // If the user clicks "No", you can handle it accordingly
            }
        }

        // Implement the logic to delete attendance
        private async Task DeleteAttendance(string id)
        {
            try
            {
                // Create a dictionary to hold the data
                var data = new Dictionary<string, string>
        {
            { "id", id }
        };

                // Convert the dictionary to JSON
                var jsonData = JsonConvert.SerializeObject(data);

                // Create the StringContent with JSON data
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the delete request to the server with the data in the request body
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(attendance_delete, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Attendance record deleted successfully
                        await DisplayAlert("Success", "Attendance record deleted successfully", "OK");

                        // Refresh the attendance data after deletion
                        FetchAttendanceData();
                    }
                    else
                    {
                        // Handle the case where the server request was not successful
                        await DisplayAlert("Error", "Failed to delete attendance record", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
                await DisplayAlert("Error", "An error occurred", "OK");
            }
        }

    }
    public class AcademicHistoryResponse
    {
        public bool status { get; set; }
        public List<AcademicHistoryItem> academic_history { get; set; }
    }
    public class AcademicHistoryItem
    {
        public string id { get; set; }
        public string student_id { get; set; }
        public string school { get; set; }
        public string academic_year { get; set; }
        public string year_level { get; set; }
        public string history => $"{year_level} | {academic_year}";
    }

    // Update your AttendanceResponse class
    public class AttendanceResponse
    {
        public bool status { get; set; }
        public List<AttendanceItem> data { get; set; }
    }

    public class AttendanceItem
    {
        public string id { get; set; }
        public string attendance_date { get; set; }
        public string status { get; set; }
    }

}