using ScanGallery.Models;
using ScanGallery.Services;
using ScanGallery.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScanGallery
{
    public partial class MainPage : ContentPage
    {

        private ImagesViewModel _viewModel;

        private ImageService _imageService;

        public MainPage()
        {
            InitializeComponent();
            _imageService = new ImageService();
            BindingContext = _viewModel = new ImagesViewModel();
            _viewModel.PropertyChanged += ImageModel_PropertyChanged;

        }


        protected override void OnAppearing()
        {

            base.OnAppearing();
           _viewModel.OnAppearing();
        }

       

        public async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Are you sure!", "You want to delete all images?", "Delete", "Cancel");
            if (result)
            {
                // user accepted
                _viewModel.DeleteAllImages();
            }
            else
            {
                // user canceled
            }
        }

        private void ImageModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            // Check if the ImagePath property changed
            if (e.PropertyName == "ImageModel")
            {
                // Perform actions based on the change in the ImagePath property

                var images = _viewModel.ImageModel;

                // Sorting the collection chronologically
                images = new ObservableCollection<ImageModel>(images.OrderByDescending(x => x.Timestamp));

                if (images != null)
                {
                    Console.WriteLine("Number of images: " + images.Count);
                    ImageGrid.Children.Clear();
                    int row = 0;
                    int column = 0;
                    foreach (Models.ImageModel image in images)
                    {
                        var imageView = new Image { Source = ImageSource.FromUri(new System.Uri(image.Url)) };
                        var frame = new Frame
                        {
                            BorderColor = Color.Black,
                            CornerRadius = 5,
                            Content = imageView,
                            WidthRequest = 100,
                            HeightRequest = 100
                        };

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (senderGesture, eventGesture) =>
                        {

                            // Navigate to the detail page and pass the image data
                            Application.Current.Properties["ImageUrl"] = image.Url;
                            Application.Current.Properties["ImageTime"] = image.Timestamp;
                            Navigation.PushAsync(new ImageDetail());
                        };
                            

                        frame.GestureRecognizers.Add(tapGestureRecognizer);

                        ImageGrid.Children.Add(frame, column, row);
                        column++;
                        if (column == 3)
                        {
                            column = 0;
                            row++;
                        }
                    }
                }
            }
        }

    }
}
