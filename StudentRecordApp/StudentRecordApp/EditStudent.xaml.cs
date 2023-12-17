using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using System.Net.Http;

namespace StudentRecordApp
{
    public class StudentItems
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string date_of_birth { get; set; } // Add this line
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string grade_level { get; set; }
        // Add other properties based on your student details
    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditStudent : ContentPage
    {
        private string studentId;
        public const string student_searchid = "http://192.168.56.1/StudentRecordApp/student-searchID.php";
        public const string student_update = "http://192.168.56.1/StudentRecordApp/student-update.php";
        public EditStudent(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            FetchStudentData();
        }
        private async void OnYearLevelSelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle the selected index changed event here
            var picker = (Picker)sender;
            string selectedYearLevel = picker.SelectedItem?.ToString();
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
                    // Set the initial values of the entries based on the fetched data
                    int yearLevel;
                    if (int.TryParse(result.student.grade_level, out yearLevel))
                    {
                        // Ensure the yearLevel is within the valid range (1-5)
                        if (yearLevel >= 1 && yearLevel <= 5)
                        {
                            YearLevelPicker.SelectedIndex = yearLevel - 1;
                        }
                    }
                    FirstNameEntry.Text = result.student.first_name;
                    LastNameEntry.Text = result.student.last_name;
                    DateOfBirthPicker.Date = DateTime.Parse(result.student.date_of_birth); // Assuming date_of_birth is a string
                    EmailEntry.Text = result.student.email;
                    PhoneEntry.Text = result.student.phone;
                    AddressEntry.Text = result.student.address;
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

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            try
            {
                // Create a dictionary to hold the updated data
                var data = new
                {
                    id = studentId,
                    grade_level = YearLevelPicker.SelectedIndex + 1,
                    first_name = FirstNameEntry.Text,
                    last_name = LastNameEntry.Text,
                    date_of_birth = DateOfBirthPicker.Date.ToString("yyyy-MM-dd"),
                    email = EmailEntry.Text,
                    phone = PhoneEntry.Text,
                    address = AddressEntry.Text
                };

                // Convert the dictionary to JSON
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the update request to the server with the updated data in the request body
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(student_update, content);

                if (response.IsSuccessStatusCode)
                {
                    // Student updated successfully
                    await DisplayAlert("Success", "Student updated successfully", "OK");
                    await Navigation.PopAsync();
                    // You may navigate back to the previous page or perform other actions
                }
                else
                {
                    // Handle the case where the server request was not successful
                    await DisplayAlert("Error", "Failed to update student", "OK");
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
    public class StudentResponse
    {
        public bool status { get; set; }
        public StudentItems student { get; set; }
    }

}