using Craiyon.Net;
using MachineLearningLab3.Basic;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MachineLearningLab3.ViewModels
{
    internal class WindowViewModel : BaseViewModel
    {
        public WindowViewModel()
        {

        }

        public ICommand ClickCommand => new RelayCommand(_ => this.GenerateImage(), _ => this.InputText != string.Empty);

        async void GenerateImage()
        {
            var craiyonService = new CraiyonService(); // Gallery index isn't needed if you are downloading the entire gallery.

            try
            {
                await craiyonService.DownloadGalleryAsync(this.InputText, "testFolder");

                this.IsImageExists = true;
                this.ImageSource   = new TransformedBitmap(
                    new BitmapImage(
                        new Uri("testFolder/0.jpg", UriKind.Relative)
                    ), new ScaleTransform(0.125, 0.125));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public string InputText { get; set; } = string.Empty;
        public ImageSource? ImageSource { get; set; } = null;

        public bool IsImageExists { get; set; } = false;
    }
}
