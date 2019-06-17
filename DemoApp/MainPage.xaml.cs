using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DemoApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MainView.Content = new Views.RandomImgsView();
        }

        private void RndImgsClicked(object sender, EventArgs e)
        {
            MainView.Content = new Views.RandomImgsView();
        }

        private void LoadFavImgs(object sender, EventArgs e)
        {
            MainView.Content = new Views.FavImgsView();
        }

        private void AppDetails(object sender, EventArgs e)
        {
            MainView.Content = new Views.AppDetails();
        }
    }
}
