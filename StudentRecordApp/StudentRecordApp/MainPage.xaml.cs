using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Net.Http;
using Newtonsoft.Json;


namespace StudentRecordApp
{
    public partial class MainPage : ContentPage
    {
        public const string class_retrieve = "http://192.168.56.1/StudentRecordApp/class-readall.php";
        public const string class_delete = "http://192.168.56.1/StudentRecordApp/class-delete.php";
        private Label errorLabel;

        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchClasses();
        }
        private async void FetchClasses()
        {
            try
            {
                ClassesStackLayout.Children.Clear();

                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(class_retrieve);
                System.Diagnostics.Debug.WriteLine(response);

                // Deserialize the response
                var classesResponse = JsonConvert.DeserializeObject<ClassesResponse>(response);

                // Check if the response is successful
                if (classesResponse.status)
                {
                    // Loop through the classes and create cards
                    foreach (var classItem in classesResponse.classes)
                    {
                        ClassesStackLayout.Children.Add(CreateClassCard(classItem));
                    }
                }
                else
                {
                    // Handle unsuccessful response
                    //DisplayError(classesResponse.message);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                //DisplayError($"Exception: {ex}");
            }
        }
        private Frame CreateClassCard(ClassItem classItem)
        {
            // Create a Frame for each class card
            var frame = new Frame
            {
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10),
                CornerRadius = 8,
                BackgroundColor = Color.FromHex("#003566"),
                BindingContext = classItem // Set the BindingContext to hold the ClassItem
            };

            // Create a Label to display the class name
            var label = new Label
            {
                Text = classItem.class_name,
                FontSize = 18,
                TextColor = Color.White,
                FontFamily = Device.RuntimePlatform == Device.iOS ? "SFUIDisplay-Bold" : "SFUIDisplay-Bold.ttf#SFUIDisplay-Bold"
            };

            // Add the Label to the Frame
            frame.Content = label;

            // Add gesture recognizer for touch events
            var tapGestureRecognizer = new TapGestureRecognizer();
            var longPressGestureRecognizer = new PanGestureRecognizer();

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                // Handle tap event
                if (frame.BindingContext is ClassItem selectedClassItem)
                {
                    // Redirect to ClassStudents and pass classId as a parameter
                    await Navigation.PushAsync(new ClassStudents(selectedClassItem.id, selectedClassItem.class_name));
                }
            };


            longPressGestureRecognizer.PanUpdated += async (s, e) =>
            {
                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        // Handle long-press started event
                        break;
                    case GestureStatus.Running:
                        // Handle long-press running event
                        break;
                    case GestureStatus.Completed:
                        // Handle long-press completed event
                        var classCard = (Frame)s;

                        // Extract class ID from classCard.BindingContext
                        if (classCard.BindingContext is ClassItem selectedClassItem)
                        {
                            var action = await DisplayActionSheet("Options", "Cancel", null, "Edit", "Delete");

                            switch (action)
                            {
                                case "Edit":
                                    // Implement the edit functionality (navigate to the edit page with classId parameter)
                                    await Navigation.PushAsync(new EditClass(selectedClassItem.id));
                                    break;
                                case "Delete":
                                    // Prompt user for confirmation before deletion
                                    bool answer = await DisplayAlert("Confirmation", $"Delete {selectedClassItem.class_name}?", "Yes", "No");

                                    if (answer)
                                    {
                                        // Send a request to delete the class on the server
                                        var deleteResponse = await DeleteClass(selectedClassItem.id);

                                        if (deleteResponse.status)
                                        {
                                            // Remove the card from the UI
                                            ClassesStackLayout.Children.Remove(classCard);
                                        }
                                        else
                                        {
                                            DisplayAlert("Error", "Failed to delete the class.", "OK");
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case GestureStatus.Canceled:
                        // Handle long-press canceled event
                        break;
                }
            };

            // Attach gesture recognizers
            frame.GestureRecognizers.Add(tapGestureRecognizer);
            frame.GestureRecognizers.Add(longPressGestureRecognizer);

            return frame;
        }
        private async Task<ClassesResponse> DeleteClass(string classId)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Create the data to send in the request body
                var data = new Dictionary<string, string>
        {
            { "class_id", classId }
        };

                // Serialize the data to JSON
                var jsonData = JsonConvert.SerializeObject(data);

                // Create a HttpRequestMessage with the DELETE method and content
                var request = new HttpRequestMessage(HttpMethod.Delete, $"{class_delete}")
                {
                    Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
                };

                // Send the request and get the response
                var deleteResponse = await client.SendAsync(request);

                var responseContent = await deleteResponse.Content.ReadAsStringAsync();

                // Deserialize the response
                var classesResponse = JsonConvert.DeserializeObject<ClassesResponse>(responseContent);

                return classesResponse;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
                return new ClassesResponse { message = "Exception occurred", status = false, classes = null };
            }
        }
        private async void OnAddClassClicked(object sender, EventArgs e)
        {
            // Navigate to the AddClass page
            await Navigation.PushAsync(new AddClass());
        }

    }
    public class ClassesResponse
    {
        public string message { get; set; }
        public bool status { get; set; }
        public List<ClassItem> classes { get; set; }
    }

    public class ClassItem
    {
        public string id { get; set; }
        public string class_name { get; set; }
    }
}
