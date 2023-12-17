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
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudent : ContentPage
    {
        public const string student_add = "http://192.168.56.1/StudentRecordApp/student-add.php";
        private string classId;

        public AddStudent(string classId)
        {
            InitializeComponent();
            this.classId = classId;
        }
        private void OnYearLevelSelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle the selected index changed event for the GradeLevelPicker
            var picker = (Picker)sender;
            string selectedGradeLevel = picker.SelectedItem?.ToString();
            // You can use selectedGradeLevel as needed
        }

        private async void OnAddStudentClicked(object sender, EventArgs e)
        {
            // Get user input
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            DateTime dateOfBirth = DateOfBirthPicker.Date;
            string address = AddressEntry.Text;
            string email = EmailEntry.Text;
            string phone = PhoneEntry.Text;

            // Get the selected grade level from the GradeLevelPicker
            string selectedGradeLevel = YearLevelPicker.SelectedItem?.ToString();

            // Ensure Grade Level is provided and parse it to an integer
            if (!string.IsNullOrEmpty(selectedGradeLevel) && int.TryParse(selectedGradeLevel, out int gradeLevel))
            {
                // Create the student object
                var student = new
                {
                    first_name = firstName,
                    last_name = lastName,
                    date_of_birth = dateOfBirth.ToString("yyyy-MM-dd"), // Format date as required
                    address,
                    email,
                    phone,
                    class_id = classId,
                    grade_level = gradeLevel
                };

                try
                {
                    // Send the student data to the server
                    HttpClient client = new HttpClient();
                    var json = JsonConvert.SerializeObject(student);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(student_add, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Student added successfully
                        await DisplayAlert("Success", "Student added successfully", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        // Handle the case where the server request was not successful
                        await DisplayAlert("Error", "Failed to add student", "OK");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
                    await DisplayAlert("Error", "An error occurred", "OK");
                }
            }
            else
            {
                // Display an alert if Grade Level is not a valid integer
                await DisplayAlert("Error", "Please select a valid Grade Level", "OK");
            }
        }
    }
}