using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScanGallery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageDetail : ContentPage
    {
        public ImageDetail()
        {
            InitializeComponent();
            var timeStamp = Application.Current.Properties["ImageTime"].ToString();

            var formattedDate = formatDate(timeStamp);

            SelectedImageTime.Text = formattedDate;
            SelectedImage.Source = ImageSource.FromUri(new System.Uri(Application.Current.Properties["ImageUrl"].ToString()));
        }

        private string formatDate(string unixTimestamp)
        {
            long result = long.Parse(unixTimestamp);
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeMilliseconds(result);
            string returnResult = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            return returnResult;
        }
    }
}