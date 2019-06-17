using System;
using Xamarin.Forms;

namespace DemoApp.Classes
{
    class AlertClass : ContentPage
    {
        public void AlertStart()
        {
            InternetAlert();
        }
        public async void InternetAlert()
        {
            await DisplayAlert("Alert", "Отсутсвует соединение с интернетом", "ОК");
        }
    }
}
