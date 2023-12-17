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
    public partial class EditHistory : ContentPage
    {
        private string historyId;
        public const string history_searchID = "http://192.168.56.1/StudentRecordApp/history-searchID.php";
        public const string history_update = "http://192.168.56.1/StudentRecordApp/history-update.php";
        public EditHistory(string historyID)
        {
            InitializeComponent();
            this.historyId = historyID;
            if (int.TryParse(historyId, out int historyIdInt))
            {
                // Fetch data from the API and initialize entry values
                FetchHistoryDetails(historyIdInt);
            }
            else
            {
                // Handle the case where historyId is not a valid integer
                // You might want to display an error message or handle it based on your requirements
            }
        }
        private async void FetchHistoryDetails(int historyId)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"{history_searchID}?id={historyId}");

                // Deserialize the response and process the academic history data
                var result = JsonConvert.DeserializeObject<AcademicHistoryResponses>(response);

                if (result.status)
                {
                    // Set the values of entry fields from the fetched data
                    SchoolEntry.Text = result.academic_history["school"];
                    AcademicYearEntry.Text = result.academic_history["academic_year"];
                    YearLevelEntry.Text = result.academic_history["year_level"];
                }
                else
                {
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

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Prepare the updated data
                var updatedData = new Dictionary<string, string>
                {
                    { "id", historyId },
                    { "school", SchoolEntry.Text },
                    { "academic_year", AcademicYearEntry.Text },
                    { "year_level", YearLevelEntry.Text }
                };

                // Create and send a POST request to update the academic history record
                HttpClient client = new HttpClient();
                var content = new FormUrlEncodedContent(updatedData);
                var response = await client.PostAsync(history_update, content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle successful update
                    // Display a success message
                    await DisplayAlert("Success", "Academic history updated successfully", "OK");

                    // Navigate back to the previous page
                    await Navigation.PopAsync();
                }
                else
                {
                    // Handle update failure
                    // Display an error message
                    await DisplayAlert("Error", "Error updating academic history", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }
    }
    public class AcademicHistoryResponses
    {
        public bool status { get; set; }
        public Dictionary<string, string> academic_history { get; set; }
    }
}