using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RandomImgsView : ContentView
    {
        Classes.ImgURLsOnLoad urlsOnLoadClass;
        private List<string> RndThumbsUrls = new List<string>();
        private List<string> RndFullUrls = new List<string>();


        private ObservableCollection<Classes.DynamicCollections.RandomImgsDC> rndImgsCollection = new ObservableCollection<Classes.DynamicCollections.RandomImgsDC>();
        CancellationToken cancellationToken;

        public RandomImgsView()
        {
            InitializeComponent();
            // Инициализируем списки и формируем ListView
            ImageSource imageToLoad = new Image().Source;
            urlsOnLoadClass = new Classes.ImgURLsOnLoad();
            urlsOnLoadClass.GetImgsURLs();
            RndThumbsUrls = urlsOnLoadClass.RndThumbsUrls;
            RndFullUrls = urlsOnLoadClass.RndFullUrls;

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
            RandomImgsListView.ItemTemplate = template;
            RandomImgsListView.ItemsSource = rndImgsCollection;

            // Асинхронно загружаем изображения в список
            Task.Run(async () =>
            {
                try
                {
                    foreach(string ad in RndThumbsUrls)
                    {
                        string index = "Index " + RndThumbsUrls.IndexOf(ad).ToString();
                        imageToLoad = await AsynkImageLoad(ad, cancellationToken);
                        rndImgsCollection.Add(new Classes.DynamicCollections.RandomImgsDC { ImageContainer = imageToLoad, ImageIndexContainer = index});
                    }
                }
                catch(System.OperationCanceledException ex)
                {
                    Console.WriteLine("Task was canceled " + ex);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error " + ex);
                }
            }, cancellationToken);
        }
        private async Task<ImageSource> AsynkImageLoad(string imgAdress, CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();
            ImageSource img = new Image().Source;
            img = ImageSource.FromUri(new Uri(imgAdress));
            return img;
        }

        // SelectedImg
        async void RandomImgsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int SelectedItemIndex = (RandomImgsListView.ItemsSource as ObservableCollection<Classes.DynamicCollections.RandomImgsDC>).IndexOf(e.SelectedItem as Classes.DynamicCollections.RandomImgsDC);

            Page fullImg = new Pages.FullImgPage(SelectedItemIndex, RndFullUrls, RndThumbsUrls);

            await Navigation.PushAsync(fullImg);
        }
    }
}