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
    public partial class AddClass : ContentPage
    {
        public const string class_add = "http://192.168.56.1/StudentRecordApp/class-add.php";

        public AddClass()
        {
            InitializeComponent();
        }
        private async void OnAddClassClicked(object sender, EventArgs e)
        {
            try
            {
                // Create a dictionary to hold the class details
                var classData = new Dictionary<string, string>
                {
                    { "class_name", ClassNameEntry.Text }
                };

                // Create a HttpClient
                using (var client = new HttpClient())
                {
                    // Convert the dictionary to JSON
                    var jsonData = JsonConvert.SerializeObject(classData);

                    // Post the JSON data to the server
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(class_add, content);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle success (optional)
                        DisplayAlert("Success", "Class added successfully", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        // Handle unsuccessful response
                        DisplayAlert("Error", "Failed to add class", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}