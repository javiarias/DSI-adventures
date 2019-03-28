using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace P4Teclado
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //Lista de Drones del UI      
        public ObservableCollection<Dron> ListaDrones { get; } = new ObservableCollection<Dron>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Contruye las listas y objetos que se necesiten 
            if (ListaDrones != null)
                foreach (Dron item in DataDrones.GetAllDrones())
                {   //Mi lista de Drones para el LisView
                    ListaDrones.Add(item);
                    Image img = new Image();
                    string s = Directory.GetCurrentDirectory() + "\\" + item.Imagen;
                    img.Source = new BitmapImage(new Uri(s));
                    img.Width = 50; img.Height = 50;
                    item.Img = img;
                }

            base.OnNavigatedTo(e);
        }
            

        private void selectAll()
        {

        }

        private void deselectAll()
        {

        }

    }
}
