using Bitacora.Models;
using Bitacora.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Bitacora
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ZonasApi api;

        public MainPage()
        {
            this.InitializeComponent();
            api = new ZonasApi();
            loadZonas();
        }

        public async void loadZonas() {
            List<Zona> newData = await api.getZonas();

            foreach (Zona z in newData)
            {
                data.Add(z);
            }

            
        }


        private ObservableCollection<Zona> data;

        public ObservableCollection<Zona> Data
        {
            get {
                if (data == null)
                    data = new ObservableCollection<Zona>();

                return data;
            }
            set { data = value; }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddZonaPage));
        }
    }
}
