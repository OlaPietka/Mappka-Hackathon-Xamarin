using MappkaMobile.View;
using System;
using Xamarin.Forms;

namespace MappkaMobile
{
    public partial class MainPage : ContentPage
    {
        private readonly InputConvert input = new InputConvert();

        public MainPage()
        {
            InitializeComponent();
            selectionView.Color = Color.LightBlue;
            selectionView.ItemsSource = new[]
            {
                "SOR", "Przychodnia", "Restauracja", "Klub", "Urząd"
            };
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (selectionView.SelectedItem != null)
            {
                input.SetServiceType(selectionView.SelectedItem.ToString());

                await Navigation.PushModalAsync(new FormPage()).ConfigureAwait(false);
            }

        }
    }
}
