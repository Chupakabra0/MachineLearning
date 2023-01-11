using Craiyon.Net;
using MachineLearningLab3.Basic;
using System;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;

namespace MachineLearningLab3.ViewModels
{
    internal class WindowViewModel : BaseViewModel
    {
        private const int MaxImages          = 9;
        private const double Scale           = 0.125;
        private const string ImageFolderName = "images";
        private const string GenerateText    = "Generate";
        private const string ProcessingText  = "Generating...";

        public WindowViewModel()
        {
            Accent.ApplySystemAccent();
        }

        public ICommand ClickCommand => new RelayCommand(_ => this.GenerateImage(), _ => this.InputText != string.Empty && this.IsWorking == false);
        public ICommand NextImageCommand => new RelayCommand(_ => this.NextImage(), _ => this.ImageSource != null);
        public ICommand PrevImageCommand => new RelayCommand(_ => this.PrevImage(), _ => this.ImageSource != null);

        void NextImage()
        {

            this.Index = this.Index < WindowViewModel.MaxImages - 1 ? this.Index + 1 : 0;
        }

        void PrevImage()
        {
            this.Index = this.Index > 0 ? this.Index - 1 : WindowViewModel.MaxImages - 1;
        }

        async void GenerateImage()
        {
            var craiyonService = new CraiyonService(); // Gallery index isn't needed if you are downloading the entire gallery.

            try
            {
                this.IsWorking  = true;
                this.ButtonText = WindowViewModel.ProcessingText;

                await craiyonService.DownloadGalleryAsync(this.InputText, $"{WindowViewModel.ImageFolderName}");

                this.IsImageExists         = false;
                this.ImageSourceCollection = new();

                for (var i = 0; i < WindowViewModel.MaxImages; ++i)
                {
                    var newName = $"{Guid.NewGuid()}.jpg";
                    FileSystem.RenameFile($"{WindowViewModel.ImageFolderName}/{i}.jpg", newName);

                    this.ImageSourceCollection.Add(new TransformedBitmap(new BitmapImage(new Uri($"{WindowViewModel.ImageFolderName}/{newName}", UriKind.Relative)),new ScaleTransform(WindowViewModel.Scale, WindowViewModel.Scale)));
                }

                this.Index         = 0;
                this.IsImageExists = true;

                this.OnPropertyChanged(nameof(this.ImageSource));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                this.IsWorking  = false;
                this.ButtonText = WindowViewModel.GenerateText;
            }
        }

        public string InputText { get; set; } = string.Empty;
        public string ButtonText { get; set; } = WindowViewModel.GenerateText;

        public ObservableCollection<ImageSource> ImageSourceCollection { get; set; } = new();
        private int Index { get; set; } = 0;
        public ImageSource? ImageSource => this.ImageSourceCollection.Count != 0 ?
            this.ImageSourceCollection?[this.Index] ?? null : null;

        public bool IsImageExists { get; set; } = false;
        public bool IsWorking     { get; set; } = false;
    }
}
