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
                c.PointerPressed += DroneClickPress;    //since the focusing/defocusing is handled elsewhere, PointerPressed only makes sure the object is positioned correctly when clicked
                c.PointerReleased += DroneClickRelease;
                c.PointerMoved += DroneClickMove;
                c.KeyDown += DroneKeyEvents;

                //both the text and image at the bottom, as well as the associated item in listaDrones, are updated whenever a drone loses or gains focus
                //this is to avoid duplicating code and updating the text every single time the drone is moved, purely a design choice
                //since GotFocus will always be raised after LostFocus (https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.uielement.gotfocus), we can be sure the right text appears
                c.LostFocus += DroneFocusUpdate;
                c.GotFocus += DroneFocusUpdate;

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
            }
        }

        private void DroneClickRelease(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //in case we need it later, empty for the moment
        }

        private void DroneKeyEvents(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            ContentControl c = sender as ContentControl;
            int senderIndex = Map.Children.IndexOf(c); Image i = c.Content as Image;

            if (e.Key == Windows.System.VirtualKey.W)
            {
                if ((double)c.GetValue(Canvas.TopProperty) > 5)
                    c.SetValue(Canvas.TopProperty, (double)c.GetValue(Canvas.TopProperty) - 5);
                else
                    c.SetValue(Canvas.TopProperty, 0);
            }
            else if (e.Key == Windows.System.VirtualKey.A)
            {
                if ((double)c.GetValue(Canvas.LeftProperty) > 5)
                    c.SetValue(Canvas.LeftProperty, (double)c.GetValue(Canvas.LeftProperty) - 5);
                else
                    c.SetValue(Canvas.LeftProperty, 0);
            }
            else if (e.Key == Windows.System.VirtualKey.S)
            {
                if ((double)c.GetValue(Canvas.TopProperty) < Map.Height - i.Height - 5)
                    c.SetValue(Canvas.TopProperty, (double)c.GetValue(Canvas.TopProperty) + 5);
                else
                    c.SetValue(Canvas.TopProperty, Map.Height - i.Height);
            }
            else if (e.Key == Windows.System.VirtualKey.D)
            {
                if ((double)c.GetValue(Canvas.LeftProperty) < Map.Width - i.Width - 5)
                    c.SetValue(Canvas.LeftProperty, (double)c.GetValue(Canvas.LeftProperty) + 5);
                else
                    c.SetValue(Canvas.LeftProperty, Map.Width - i.Width);
            }
            else if (e.Key == Windows.System.VirtualKey.Left)
            {
                if (senderIndex > 0)
                {
                    c = Map.Children[senderIndex - 1] as ContentControl;
                    c.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Right)
            {
                if (senderIndex < Map.Children.Count() - 1)
                {
                    c = Map.Children[senderIndex + 1] as ContentControl;
                    c.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                }
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
        private void DroneFocusUpdate(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentControl c = sender as ContentControl;

            foreach (Dron d in ListaDrones)
            {
                if (d.Img == c.Content)
                {
                    //for some reason, you can't turn getvalue directly into an int, you first have to cast it as a double
                    d.X = (int)(double)c.GetValue(Canvas.LeftProperty);
                    d.Y = (int)(double)c.GetValue(Canvas.TopProperty);
                    DroneInfo.Text = "Id: " + d.Id + ", Nombre: " + d.Nombre + ", Estado: " + d.Estado + ", Coordenadas: (" + d.X + ", " + d.Y + ")" + "\n" + "Explicación: " + d.Explicacion;
                    DroneImg.Source = d.Img.Source;

                    if(c.FocusState != Windows.UI.Xaml.FocusState.Unfocused)
                        c.SetValue(Canvas.ZIndexProperty, 1);
                    else
                        c.SetValue(Canvas.ZIndexProperty, 0);

                    break;
                }
            }
        }
    }
}
