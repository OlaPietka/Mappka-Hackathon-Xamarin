using System;
using System.Data.SqlClient;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Reflection;


namespace MappkaMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ServicePage : ContentPage
	{
        private readonly string updateQuery;
        private readonly List<(Color NotPressed, Color Pressed)> colors = new List<(Color, Color)> { (Color.FromRgb(212, 253, 206), Color.FromRgb(170, 251, 157)),
            (Color.FromRgb(230, 254, 205), Color.FromRgb(206, 253, 155)), (Color.FromRgb(254, 254, 205), Color.FromRgb(253, 253, 155)), 
            (Color.FromRgb(254, 230, 205), Color.FromRgb(253, 206, 155)), (Color.FromRgb(254, 205, 205), Color.FromRgb(253, 155, 155)) };

        public ServicePage ()
		{
            InitializeComponent();

            updateQuery = $"UPDATE dbo.{Db.DatabaseStrings.DbName} SET OccupiedPercent = @occupiedPercent " +
                          $"WHERE Id = @id; ";

            InitColors();
        }

        private void Empty_Clicked(object sender, EventArgs e) => Update((Button)sender, 10);

        private void Low_Clicked(object sender, EventArgs e) => Update((Button)sender, 30);

        private void Medium_Clicked(object sender, EventArgs e) => Update((Button)sender, 50);

        private void High_Clicked(object sender, EventArgs e) => Update((Button)sender, 80);

        private void Full_Clicked(object sender, EventArgs e) => Update((Button)sender, 100);


        private void Update(Button button, int percent)
        {
            InitColors();
            button.BackgroundColor = colors[Int32.Parse(button.AutomationId)].Pressed;

            using (SqlConnection connection = new SqlConnection(Db.DatabaseStrings.ConnetionString))
            {
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@occupiedPercent", percent);
                command.Parameters.AddWithValue("@id", Preferences.Get("id", ""));

                connection.Open();

                if (command.ExecuteNonQuery() < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }

        private void InitColors()
        {
            var i = 0;
            foreach (var view in Stack.Children)
            {
                if (view is Button button)
                    button.BackgroundColor = colors[i++].NotPressed;
            }
        }
    }
}