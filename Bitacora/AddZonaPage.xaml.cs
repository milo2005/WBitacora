using Bitacora.Models;
using Bitacora.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Bitacora
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class AddZonaPage : Page
    {
        StorageFile file;
        ZonasApi api;

        public AddZonaPage()
        {
            this.InitializeComponent();
            api = new ZonasApi();
        }

        private async void Button_Image(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            file = await openPicker.PickSingleFileAsync();
           
            
            BitmapImage bitmapImage = new BitmapImage();
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
            await bitmapImage.SetSourceAsync(stream);
            
            imagen.Source= bitmapImage;
              
        }

        private async void Button_Save(object sender, RoutedEventArgs e)
        {
            Zona z = new Zona();
            z.Nombre = nombre.Text;
            z.Direccion = direccion.Text;
            z.Descripcion = descripcion.Text;

            bool success = await api.insertZona(z, file);
            if (success)
            {
                Frame root = Window.Current.Content as Frame;
                root.GoBack();
            }
            else {
                MessageDialog dialog = new MessageDialog("could not open the folder?", "Information");
                await dialog.ShowAsync();
            }

        }
    }
}
