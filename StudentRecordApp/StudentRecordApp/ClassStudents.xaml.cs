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
    public partial class ClassStudents : ContentPage
    {
        public const string class_update = "http://192.168.56.1/StudentRecordApp/class-students.php";
        public const string student_delete = "http://192.168.56.1/StudentRecordApp/student-delete.php";
        public const string student_search = "http://192.168.56.1/StudentRecordApp/student-search.php";
        private string classId;
        public ClassStudents(string classId, string className)
        {
            InitializeComponent();
            FetchStudents(classId);
            this.classId = classId;
            ClassNameLabel.Text = className;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh data when the page appears
            RefreshData();
        }

        private async void RefreshData()
        {
            // Refresh data logic
            FetchStudents(classId);
        }

        private async void FetchStudents(string classId, string searchName = null, string gradeLevel = null)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Build the URL with classId, searchName, and gradeLevel parameters
                var apiUrl = $"{student_search}?class_id={classId}";

                if (!string.IsNullOrEmpty(searchName))
                {
                    apiUrl += $"&name={searchName}";
                }

                // Add a default value of 0 for gradeLevel if not provided
                if (string.IsNullOrEmpty(gradeLevel))
                {
                    gradeLevel = "0";
                }

                apiUrl += $"&grade_level={gradeLevel}";

                var response = await client.GetStringAsync(apiUrl);

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


        private async void OnAddStudentClicked(object sender, EventArgs e)
        {
            // Navigate to the AddClass page
            await Navigation.PushAsync(new AddStudent(classId));
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            // Get the selected student item
            var student = (StudentItem)((MenuItem)sender).CommandParameter;

            // Show confirmation prompt
            var result = await DisplayAlert("Confirmation", $"Do you really want to delete {student.first_name} {student.last_name}?", "Yes", "No");

            if (result)
            {
                // User clicked "Yes", proceed with deletion
                await DeleteStudent(student.id);
            }
        }

        private async Task DeleteStudent(string studentId)
        {
            try
            {
                // Create a dictionary to hold the data
                var data = new Dictionary<string, string>
        {
            { "id", studentId }
        };

                // Convert the dictionary to JSON
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the delete request to the server with the data in the request body
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(student_delete, content);

                if (response.IsSuccessStatusCode)
                {
                    // Student deleted successfully
                    await DisplayAlert("Success", "Student deleted successfully", "OK");

                    // Refresh the data after deletion
                    RefreshData();
                }
                else
                {
                    // Handle the case where the server request was not successful
                    await DisplayAlert("Error", "Failed to delete student", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
                await DisplayAlert("Error", "An error occurred", "OK");
            }
        }
        private async void OnEdit(object sender, EventArgs e)
        {
            // Get the selected student item
            var student = (StudentItem)((MenuItem)sender).CommandParameter;

            // Navigate to the EditStudent page and pass only the student ID as a parameter
            await Navigation.PushAsync(new EditStudent(student.id));
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Handle item selection if needed
            if (e.SelectedItem == null)
                return;

            // Get the selected student item
            var selectedStudent = (StudentItem)e.SelectedItem;

            // Navigate to the ViewStudent page and pass the student ID as a parameter
            Navigation.PushAsync(new ViewStudent(selectedStudent.id));

            // Deselect the item to remove the highlight
            StudentsListView.SelectedItem = null;
        }
        private async void OnRecordAttendanceClicked(object sender, EventArgs e)
        {
            // Navigate to the AddAttendance page and pass the classId as a parameter
            await Navigation.PushAsync(new AddAttendance(classId));
        }
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            int selectedIndex = GradeLevelPicker.SelectedIndex;

            // Set gradeLevel based on the selected index
            string gradeLevel = selectedIndex.ToString();
            // Call the API with the updated search text and grade level
            string searchText = SearchEntry.Text;

            // Check if the search text is empty, if yes, set it to null
            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchText = null;
            }

            FetchStudents(classId, searchText, gradeLevel);
        }

        private void OnGradeLevelSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected index
            int selectedIndex = GradeLevelPicker.SelectedIndex;

            // Set gradeLevel based on the selected index
            string gradeLevel = selectedIndex.ToString();

            // Call the API with the updated grade level
            FetchStudents(classId, SearchEntry.Text, gradeLevel);
        }

    }

    public class StudentsResponse
    {
        public bool status { get; set; }
        public List<StudentItem> students { get; set; }
    }

    public class StudentItem
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string grade_level { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string full_name
        {
            get { return $"{first_name} {last_name}"; }
        }
        public string year_level
        {
            get { return $"Year Level {grade_level} | {phone}"; }
        }
        public string full_name2
        {
            get { return $"{first_name} {last_name}"; }
        }
    }
}