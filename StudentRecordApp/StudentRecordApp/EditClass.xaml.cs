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
    public class ApiResponse
    {
        public bool status { get; set; }
        public ClassDetails @class { get; set; }
        public string message { get; set; }
    }

    public class ClassDetails
    {
        public string class_name { get; set; }
        // Add other properties based on your class details
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditClass : ContentPage
    {
        public const string class_searchid = "http://192.168.56.1/StudentRecordApp/class-searchID.php";
        public const string class_update = "http://192.168.56.1/StudentRecordApp/class-update.php";

        private string classId;

        public EditClass(string classId)
        {
            InitializeComponent();
            this.classId = classId;
            FetchClassDetails();
        }
        private async void FetchClassDetails()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"{class_searchid}?id={classId}");
                var result = JsonConvert.DeserializeObject<ApiResponse>(response);

                if (result.status)
                {
                    var classDetails = result.@class;
                    ClassNameEntry.Text = classDetails.class_name;
                }
                else
                {
                    // Handle case where class details are not found
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }

        private async void OnUpdateClassClicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Prepare the data to be sent in the request body
                var data = new
                {
                    id = classId,
                    new_class_name = ClassNameEntry.Text
                    // Add other properties based on your input fields
                };

                // Serialize the data to JSON
                var jsonData = JsonConvert.SerializeObject(data);

                // Create a StringContent with the JSON data
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the HTTP POST request to update the class
                var response = await client.PostAsync(class_update, content);

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response
                var updateResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

                if (updateResponse.status)
                {
                    // Handle successful update
                    DisplayAlert("Success", "Class updated successfully", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    // Handle update failure
                    DisplayAlert("Error", "Failed to update class", "OK");

                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            }
        }
    }
}