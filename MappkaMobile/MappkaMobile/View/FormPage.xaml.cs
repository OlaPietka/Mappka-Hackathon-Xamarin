using MappkaMobile.Azure;
using MappkaMobile.Interfaces;
using Newtonsoft.Json;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MappkaMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FormPage : ContentPage
	{
        private string uploadedFilename = string.Empty;
        private byte[] byteData;

        private Stream stream;

        private readonly InputConvert input = new InputConvert();

        public FormPage ()
		{
			InitializeComponent ();

            var list = input.GetServiceList(Db.DatabaseStrings.DbName);

            var i = 2;
            foreach (var view in GridLayout.Children)
            {
                if (view is CheckBox chk)
                {
                    chk.Text = list[i++];
                    NameEntry.Title = list[0];
                    this.Type.Text = list[1];
                }
            }
        }


        #region Buttons Events
        private async void Register_Clicked(object sender, EventArgs e)
        {
            foreach (var view in Stack.Children)
                if (view is AdvancedEntry en && en.Text == null)
                {
                    if(!(en == BuildingNumberEntry))
                    {
                        en.ValidationMessage = "Pole jest wymagane";
                        return;
                    }
                }

            var address = $"{CityEntry.Text} {StreetEntry.Text} {BuildingNumberEntry.Text}";
            var locations = await Geocoding.GetLocationsAsync(address).ConfigureAwait(false);

            var location = locations?.FirstOrDefault();

            if (location == null) return;

            var checkBoxList = new List<string>();
            foreach (var view in GridLayout.Children)
                if (view is CheckBox chk && chk.IsChecked)
                    checkBoxList.Add(chk.Text);

            using (SqlConnection connection = new SqlConnection(Db.DatabaseStrings.ConnetionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT Name FROM dbo.{Db.DatabaseStrings.DbName} WHERE Id=\'{VerificationEntry.Text}\'", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            connection.Close();
                            return;
                        }
                    }
                }
            }

            Preferences.Set("id", VerificationEntry.Text);

            if (stream != null)
            {
                var imageToUpload = new Image();
                var activityIndicator = new ActivityIndicator();

                byteData = Azure.Convert.ConvertToByteArray(stream);
                imageToUpload.Source = ImageSource.FromStream(() => new MemoryStream(byteData));

                activityIndicator.IsRunning = true;
                uploadedFilename = await AzureStorage.UploadFileAsync(ContainerType.Image, new MemoryStream(byteData)).ConfigureAwait(false);
                activityIndicator.IsRunning = false;
            }

            using (SqlConnection connection = new SqlConnection(Db.DatabaseStrings.ConnetionString))
            {
                 var insertQuery = $"UPDATE dbo.{Db.DatabaseStrings.DbName} SET Name = @name, City = @city, Street = @street, Building = @building, Flat = @flat, Latitude = @lat, Longitude = @lon, ImageName = @image, Type = @types WHERE Id = @id; ";

                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@name", NameEntry.Text);
                command.Parameters.AddWithValue("@city", CityEntry.Text);
                command.Parameters.AddWithValue("@street", StreetEntry.Text);
                command.Parameters.AddWithValue("@building", BuildingNumberEntry.Text);
                command.Parameters.AddWithValue("@flat", FlatNumberEntry.Text);
                command.Parameters.AddWithValue("@lat", location.Latitude);
                command.Parameters.AddWithValue("@lon", location.Longitude);
                command.Parameters.AddWithValue("@image", uploadedFilename);
                command.Parameters.AddWithValue("@types", JsonConvert.SerializeObject(checkBoxList));
                command.Parameters.AddWithValue("@id", Preferences.Get("id", ""));
                
                connection.Open();

                if(command.ExecuteNonQuery() < 0)
                    Console.WriteLine("Error inserting data into Database!");

                await Navigation.PushModalAsync(new ServicePage());
            }
        }

        private async void Gallery_Clicked(object sender, EventArgs e)
        {
            stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
        }
        #endregion

        private void VerificationEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (AdvancedEntry)sender;

            if (entry.Text != null)
                entry.ValidationMessage = string.Empty;
        }
    }
}