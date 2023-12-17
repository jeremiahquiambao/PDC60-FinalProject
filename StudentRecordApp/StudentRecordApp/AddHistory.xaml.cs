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
    public class ApiResponses
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddHistory : ContentPage
    {
        private string studentId;
        public const string history_add = "http://192.168.56.1/StudentRecordApp/history-add.php";
        public AddHistory(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
        }
        private async void OnAddAcademicHistoryClicked(object sender, EventArgs e)
        {
            string school = SchoolEntry.Text;
            string academicYear = AcademicYearEntry.Text;
            string yearLevel = YearLevelEntry.Text;

            // Check if any of the fields are empty
            if (string.IsNullOrEmpty(school) || string.IsNullOrEmpty(academicYear) || string.IsNullOrEmpty(yearLevel))
            {
                await DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            // Create the parameters for the POST request
            var parameters = new Dictionary<string, string>
            {
                { "student_id", studentId },
                { "school", school },
                { "academic_year", academicYear },
                { "year_level", yearLevel }
            };

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(history_add, new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<ApiResponses>(result);

                if (resultObject.status)
                {
                    await DisplayAlert("Success", "Academic history added successfully", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add academic history", "OK");
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
}