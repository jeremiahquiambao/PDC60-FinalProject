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
    public partial class LoginPage : ContentPage
    {
        public const string admin_login = "http://192.168.56.1/StudentRecordApp/admin-login.php";
        public LoginPage()
        {
            InitializeComponent();
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Create a dictionary to hold the login data
            var loginData = new
            {
                username,
                password
            };

            // Convert the dictionary to JSON
            var jsonData = JsonConvert.SerializeObject(loginData);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a POST request to the login API
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(admin_login, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

                        if (result.status)
                        {
                            // Login successful, navigate to MainPage
                            await Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            // Display error message for unsuccessful login
                            await DisplayAlert("Error", result.message, "OK");
                        }
                    }
                    else
                    {
                        // Display error message for unsuccessful API request
                        await DisplayAlert("Error", "Failed to connect to the server", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        }
    }
    public class LoginResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}