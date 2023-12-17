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
    public partial class EditAttendance : ContentPage
    {
        private string attendanceId;
        public const string attendance_update = "http://192.168.56.1/StudentRecordApp/attendance-update.php";
        public const string attendance_searchId = "http://192.168.56.1/StudentRecordApp/attendance-searchID.php";
        public EditAttendance(string attendanceId)
        {
            InitializeComponent();
            this.attendanceId = attendanceId;
            FetchAttendanceData();
        }
        private async void FetchAttendanceData()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"{attendance_searchId}?id={attendanceId}");

                var result = JsonConvert.DeserializeObject<AttendanceResponses>(response);

                if (result.status)
                {
                    // Populate the DatePicker control with existing data
                    DateTime attendanceDate;
                    if (DateTime.TryParse(result.data.attendance_date, out attendanceDate))
                    {
                        AttendanceDatePicker.Date = attendanceDate;
                    }

                    // Set the selected status in the Picker
                    StatusPicker.SelectedItem = result.data.status;
                }
                else
                {
                    // Handle case where the attendance data is not found
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }

        private async void OnUpdateAttendanceClicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Prepare the data for updating attendance
                var data = new Dictionary<string, string>
        {
            { "id", attendanceId },
            { "attendance_date", AttendanceDatePicker.Date.ToString("yyyy-MM-dd") },
            { "status", StatusPicker.SelectedItem.ToString() }
        };

                // Convert the dictionary to JSON
                var jsonData = JsonConvert.SerializeObject(data);

                // Create the StringContent with JSON data
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the update request to the server with the data in the request body
                var response = await client.PostAsync(attendance_update, content);

                if (response.IsSuccessStatusCode)
                {
                    // Attendance updated successfully
                    await DisplayAlert("Success", "Attendance updated successfully", "OK");

                    // Navigate back to the previous page (ViewStudent) after updating
                    await Navigation.PopAsync();
                }
                else
                {
                    // Handle the case where the server request was not successful
                    await DisplayAlert("Error", "Failed to update attendance", "OK");
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
    public class AttendanceResponses
    {
        public bool status { get; set; }
        public AttendanceItem data { get; set; }
    }
}