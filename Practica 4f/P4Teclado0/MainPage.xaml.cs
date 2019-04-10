using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
            DroneList.SelectAll();
        }

        private void deselectAll()
        {
            DroneList.SelectedItems.Clear();
        }

        private void DroneList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(Dron d in e.RemovedItems)
            {
                foreach(ContentControl c in Map.Children)
                {
                    if (d.Img == c.Content)
                    {
                        Map.Children.Remove(c);
                        c.Content = null;
                        break;
                    }
                }
            }

            foreach(Dron d in e.AddedItems)
            {
                ContentControl c = new ContentControl();
                c.Content = d.Img;
                c.UseSystemFocusVisuals = true;
                c.PointerPressed += DroneClickPress;
                c.PointerReleased += DroneClickRelease;
                c.PointerMoved += DroneClickMove;
                c.KeyDown += DroneKeyMove;

                //just in case the pointer/mouse is dragged outside the canvas and then released, we can update the text
                c.LostFocus += DroneLostFocus;

                //c.KeyDown = ;
                Map.Children.Add(c);
                Map.Children.Last().SetValue(Canvas.LeftProperty, d.X);
                Map.Children.Last().SetValue(Canvas.TopProperty, d.Y);
            }


            if (e.AddedItems.Count == 0 && DroneList.SelectedItems.Count == 0)
            {
                DroneInfo.Text = "";
                DroneImg.Source = null;
            }
            else
            {
                Dron d = DroneList.SelectedItems.Last() as Dron;

                DroneInfo.Text = "Id: " + d.Id + ", Nombre: " + d.Nombre + ", Estado: " + d.Estado + ", Coordenadas: (" + d.X + ", " + d.Y + ")" + "\n" + "Explicación: " + d.Explicacion;
                DroneImg.Source = d.Img.Source;
            }
        }

        private void DroneClickPress(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
            {
                ContentControl c = sender as ContentControl;
                c.Focus(Windows.UI.Xaml.FocusState.Pointer);

                Image i = c.Content as Image;
                Windows.UI.Input.PointerPoint ptr = e.GetCurrentPoint(Map);

                c.SetValue(Canvas.LeftProperty, ptr.Position.X - i.Width / 2);
                c.SetValue(Canvas.TopProperty, ptr.Position.Y - i.Height / 2);

                if (ptr.Position.X < i.Width / 2)
                    c.SetValue(Canvas.LeftProperty, 0);
                else if (ptr.Position.X > Map.Width - i.Width / 2)
                    c.SetValue(Canvas.LeftProperty, Map.Width - i.Width);

                if (ptr.Position.Y < i.Height / 2)
                    c.SetValue(Canvas.TopProperty, 0);
                else if(ptr.Position.Y > Map.Height - i.Height / 2)
                    c.SetValue(Canvas.TopProperty, Map.Height - i.Height);

                foreach (Dron d in ListaDrones)
                {
                    if (d.Img == c.Content)
                    {
                        DroneInfo.Text = "Id: " + d.Id + ", Nombre: " + d.Nombre + ", Estado: " + d.Estado + ", Coordenadas: (" + d.X + ", " + d.Y + ")" + "\n" + "Explicación: " + d.Explicacion;
                        DroneImg.Source = d.Img.Source;
                        Map.Children.Move((uint)Map.Children.IndexOf(c), (uint)Map.Children.IndexOf(Map.Children.Last()));
                        break;
                    }
                }
            }
        }

        private void DroneClickRelease(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ContentControl c = sender as ContentControl;
            foreach (Dron d in ListaDrones)
            {
                if (d.Img == c.Content)
                {
                    d.X = (int)Convert.ToDouble(c.GetValue(Canvas.LeftProperty).ToString());
                    d.Y = (int)Convert.ToDouble(c.GetValue(Canvas.TopProperty).ToString());
                    DroneInfo.Text = "Id: " + d.Id + ", Nombre: " + d.Nombre + ", Estado: " + d.Estado + ", Coordenadas: (" + d.X + ", " + d.Y + ")" + "\n" + "Explicación: " + d.Explicacion;
                    DroneImg.Source = d.Img.Source;
                    break;
                }
            }
        }

        private void DroneKeyMove(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            ContentControl c = sender as ContentControl;

            if (e.Key == Windows.System.VirtualKey.W)
            {
                c.SetValue(Canvas.TopProperty, (double)c.GetValue(Canvas.TopProperty) - 5);
            }
            else if (e.Key == Windows.System.VirtualKey.A)
            {
                c.SetValue(Canvas.LeftProperty, (double)c.GetValue(Canvas.LeftProperty) - 5);
            }
            else if (e.Key == Windows.System.VirtualKey.S)
            {
                c.SetValue(Canvas.TopProperty, (double)c.GetValue(Canvas.TopProperty) + 5);
            }
            else if (e.Key == Windows.System.VirtualKey.D)
            {
                c.SetValue(Canvas.LeftProperty, (double)c.GetValue(Canvas.LeftProperty) + 5);
            }
            else if (e.Key == Windows.System.VirtualKey.Left)
            {

            }
            else if (e.Key == Windows.System.VirtualKey.Right)
            {

            }
        }

        private void DroneClickMove(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ContentControl c = sender as ContentControl;

            if (c.FocusState == Windows.UI.Xaml.FocusState.Pointer)
            {
                Windows.UI.Input.PointerPoint ptr = e.GetCurrentPoint(Map);
                Image i = c.Content as Image;
                if ((ptr.Position.X > i.Width / 2 && ptr.Position.X < Map.Width - i.Width / 2) && (ptr.Position.Y > i.Height / 2 && ptr.Position.Y < Map.Height - i.Height / 2))
                {
                    c.SetValue(Canvas.LeftProperty, ptr.Position.X - i.Width / 2);
                    c.SetValue(Canvas.TopProperty, ptr.Position.Y - i.Height / 2);
                }
            }
        }

        private void DroneLostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DroneClickRelease(sender, null);
        }
    }
}
