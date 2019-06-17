using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullImgPage : ContentPage
    {
        // Paths to files
        private string fullPath = string.Empty;
        private string thumbPath = string.Empty;
        // Favorite Imgs
        private List<string> FavThumbs = new List<string>();
        private List<string> FavFull = new List<string>();
        //Random Imgs
        private List<string> RndThumbs = new List<string>();
        private List<string> RndFull = new List<string>();

        private bool checker = false;
        private string URL = string.Empty;

        public FullImgPage(int imgIndex, List<string> FUrls, List<string> TUrls)
        {
            InitializeComponent();
            RndThumbs = TUrls;
            RndFull = FUrls;

            // showing the image
            URL = FUrls.ElementAt(imgIndex);
            Image.Source = ImageSource.FromUri(new Uri(URL));

            thumbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FavThumbsList.txt");
            fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FavFullsList.txt");

            // Check if Imgs in Favorites
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FavThumbsList.txt")))
            {
                FavThumbs = new List<string>(File.ReadAllLines(thumbPath));
                FavFull = new List<string>(File.ReadAllLines(fullPath));
                if(FavFull.Contains(URL))
                {
                    AddRemove.Text = "Remove from Favorite";
                    checker = true;
                }
                else
                {
                    AddRemove.Text = "Add to Favorite";
                    checker = false;
                }
            }
            else
            {
                AddRemove.Text = "Add to Favorite";
                checker = false;
            }
        }

        // Adding or removing
        private void AddRemove_Clicked(object sender, EventArgs e)
        {
            // Removing image from Favorite
            if(checker)
            {
                FavThumbs.Remove(FavThumbs.ElementAt(FavFull.IndexOf(URL)));
                FavFull.Remove(URL);
                File.WriteAllLines(fullPath, FavFull);
                File.WriteAllLines(thumbPath, FavThumbs);
                checker = false;
                AddRemove.Text = "Add to Favorite";
            }
            // Adding image to Favorites
            else
            {
                FavFull.Add(URL);
                FavThumbs.Add(RndThumbs.ElementAt(RndFull.IndexOf(URL)));
                File.WriteAllLines(fullPath, FavFull);
                File.WriteAllLines(thumbPath, FavThumbs);
                checker = true;
                AddRemove.Text = "Remove from Favorite";
            }
        }
    }
}