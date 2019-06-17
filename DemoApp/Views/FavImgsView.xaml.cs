using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Threading;

namespace DemoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavImgsView : ContentView
    {
        private List<string> favThumbs = new List<string>();
        private List<string> favFull = new List<string>();
        // Paths
        private string FullImgsPath = string.Empty;
        private string ThumbsImgsPath = string.Empty;

        // ListView Custom Collection
        private ObservableCollection<Classes.DynamicCollections.FavoritesContainerCollection> favImgsCollection = new ObservableCollection<Classes.DynamicCollections.FavoritesContainerCollection>();
        CancellationToken cancellationToken;

        public FavImgsView()
        {
            InitializeComponent();
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FavThumbsList.txt")))
            {
                FullImgsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FavFullsList.txt");
                ThumbsImgsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FavThumbsList.txt");
                // Read Lists of Fav Imgs
                favThumbs = File.ReadAllLines(ThumbsImgsPath).ToList();
                favFull = File.ReadAllLines(FullImgsPath).ToList();

                // Image Source
                ImageSource imageSource = new Image().Source;

                // Creating DataView Template for our ListView
                DataTemplate template = new DataTemplate(() =>
                {
                    Grid grid = new Grid();

                    Image imgg = new Image();
                    Label imageIndex = new Label();
                    imgg.SetBinding(Image.SourceProperty, "ImageContainer");
                    imageIndex.SetBinding(Label.TextProperty, "ImageIndexContainer");

                    grid.Children.Add(imgg);
                    grid.Children.Add(imageIndex);

                    return new ViewCell { View = grid };
                });
                FavoriteImgsListView.ItemTemplate = template;
                FavoriteImgsListView.ItemsSource = favImgsCollection;

                // Loadyng Imgs Async
                Task.Run(async () =>
                {
                    try
                    {
                        foreach (string i in favThumbs)
                        {
                            string index = "Index " + favThumbs.IndexOf(i).ToString();
                            imageSource = await AsyncImageLoad(i, cancellationToken);
                            favImgsCollection.Add(new Classes.DynamicCollections.FavoritesContainerCollection { ImageContainer = imageSource, ImageIndexContainer = index });
                        }
                    }
                    catch (System.OperationCanceledException ex)
                    {
                        Console.WriteLine("Task was canceled " + ex);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error " + ex);
                    }
                }, cancellationToken);
            }
            else
            {
                Label lb = new Label()
                {
                    Text = "Favorite Images is empty add new Imgs",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                FavImgsContent.Content = lb;
            }
        }
        // Loading Imgs Async
        private async Task<ImageSource> AsyncImageLoad(string imgAdress, CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();
            ImageSource img = new Image().Source;
            img = ImageSource.FromUri(new Uri(imgAdress));
            return img;
        }

        // Selecting Img
        async void FavoriteImgsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int SelectedItemIndex = (FavoriteImgsListView.ItemsSource as ObservableCollection<Classes.DynamicCollections.FavoritesContainerCollection>).IndexOf(e.SelectedItem as Classes.DynamicCollections.FavoritesContainerCollection);

            Page fullImg = new Pages.FullImgPage(SelectedItemIndex, favFull, favThumbs);

            await Navigation.PushAsync(fullImg);
        }
    }
}