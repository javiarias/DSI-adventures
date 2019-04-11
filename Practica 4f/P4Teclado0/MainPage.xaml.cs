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
using Windows.Gaming.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.System;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace P4Teclado
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //Lista de Drones del UI      
        public ObservableCollection<Dron> ListaDrones { get; } = new ObservableCollection<Dron>(); private readonly object myLock = new object();
        private List<Gamepad> myGamepads = new List<Gamepad>();
        private Gamepad mainGamepad;

        private void GetGamepads()
        {
            lock (myLock)
            {
                foreach (var gamepad in Gamepad.Gamepads)
                {
                    // Check if the gamepad is already in myGamepads; if it isn't, add it.
                    bool gamepadInList = myGamepads.Contains(gamepad);

                    if (!gamepadInList)
                    {
                        // This code assumes that you're interested in all gamepads.
                        myGamepads.Add(gamepad);
                    }
                }
            }
        }

        DispatcherTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += TimerTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            dispatcherTimer.Start();
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {
            GetGamepads();
            Gamepad.GamepadAdded += (object sender, Gamepad e) =>
            {
                // Check if the just-added gamepad is already in myGamepads; if it isn't, add
                // it.
                lock (myLock)
                {
                    bool gamepadInList = myGamepads.Contains(e);

                    if (!gamepadInList)
                    {
                        myGamepads.Add(e);
                    }
                }
            };
            Gamepad.GamepadRemoved += (object sender, Gamepad e) =>
            {
                lock (myLock)
                {
                    int indexRemoved = myGamepads.IndexOf(e);

                    if (indexRemoved > -1)
                    {
                        if (mainGamepad == myGamepads[indexRemoved])
                        {
                            mainGamepad = null;
                        }

                        myGamepads.RemoveAt(indexRemoved);
                    }
                }
            };

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

            DispatcherTimerSetup();

            base.OnNavigatedTo(eventArgs);
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

        private void DroneClickPress(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
            {
                ContentControl c = sender as ContentControl;
                c.Focus(FocusState.Pointer);

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

        private void DroneClickRelease(object sender, PointerRoutedEventArgs e)
        {
            //in case we need it later, empty for the moment
        }

        private void DroneKeyEvents(object sender, KeyRoutedEventArgs e)
        {
            ContentControl c = sender as ContentControl;
            int senderIndex = Map.Children.IndexOf(c);
            Image i = c.Content as Image;

        //  KEYBOARD CONTROLS

            if (e.Key == VirtualKey.W)
            {
                if ((double)c.GetValue(Canvas.TopProperty) > 5)
                    c.SetValue(Canvas.TopProperty, (double)c.GetValue(Canvas.TopProperty) - 5);
                else
                    c.SetValue(Canvas.TopProperty, 0);
            }
            else if (e.Key == VirtualKey.D)
            {
                if ((double)c.GetValue(Canvas.LeftProperty) < Map.Width - i.Width - 5)
                    c.SetValue(Canvas.LeftProperty, (double)c.GetValue(Canvas.LeftProperty) + 5);
                else
                    c.SetValue(Canvas.LeftProperty, Map.Width - i.Width);
            }
            if (e.Key == VirtualKey.A)
            {
                if ((double)c.GetValue(Canvas.LeftProperty) > 5)
                    c.SetValue(Canvas.LeftProperty, (double)c.GetValue(Canvas.LeftProperty) - 5);
                else
                    c.SetValue(Canvas.LeftProperty, 0);
            }
            else if (e.Key == VirtualKey.S)
            {
                if ((double)c.GetValue(Canvas.TopProperty) < Map.Height - i.Height - 5)
                    c.SetValue(Canvas.TopProperty, (double)c.GetValue(Canvas.TopProperty) + 5);
                else
                    c.SetValue(Canvas.TopProperty, Map.Height - i.Height);
            }
            if (e.Key == VirtualKey.Left)
            {
                if (senderIndex > 0)
                {
                    c = Map.Children[senderIndex - 1] as ContentControl;
                    c.Focus(FocusState.Keyboard);
                }
            }
            if (e.Key == VirtualKey.Right)
            {
                if (senderIndex < Map.Children.Count() - 1)
                {
                    c = Map.Children[senderIndex + 1] as ContentControl;
                    c.Focus(FocusState.Keyboard);
                }
            }

            DroneFocusUpdate(sender, null);
        }

        void TimerTick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            lastTime = time;
            //Time since last tick should be very very close to Interval


            if (myGamepads.Count > 0)
            {
                Gamepad gamepad = myGamepads[0];
                GamepadReading reading = gamepad.GetCurrentReading();

                double rightStickX = reading.RightThumbstickX; // returns a value between -1.0 and +1.0
                double rightStickY = reading.RightThumbstickY; // returns a value between -1.0 and +1.0

                ContentControl c = null;
                foreach (ContentControl cont in Map.Children)
                {
                    if (cont.FocusState == FocusState.Keyboard)
                    {
                        c = cont;
                        break;
                    }
                }

                if (c != null)
                {
                    Image i = c.Content as Image;
                    double droneX = (double)c.GetValue(Canvas.LeftProperty);
                    double droneY = (double)c.GetValue(Canvas.TopProperty);

                    if (rightStickX > 0.3 || rightStickX < -0.3)
                        droneX += rightStickX * 4;
                    if (rightStickY > 0.3 || rightStickY < -0.3)
                        droneY -= rightStickY * 4;  //y is inverted


                    if (droneY < 0)
                        c.SetValue(Canvas.TopProperty, 0);
                    else if (droneY > Map.Height - i.Height)
                        c.SetValue(Canvas.TopProperty, Map.Height - i.Height);
                    else
                        c.SetValue(Canvas.TopProperty, droneY);

                    if (droneX < 0)
                        c.SetValue(Canvas.LeftProperty, 0);
                    else if (droneX > Map.Width - i.Width)
                        c.SetValue(Canvas.LeftProperty, Map.Width - i.Width);
                    else
                        c.SetValue(Canvas.LeftProperty, droneX);

                    DroneFocusUpdate(c, null);
                }
            }
        }

        private void DroneClickMove(object sender, PointerRoutedEventArgs e)
        {
            ContentControl c = sender as ContentControl;

            if (c.FocusState == FocusState.Pointer)
            {
                Windows.UI.Input.PointerPoint ptr = e.GetCurrentPoint(Map);
                Image i = c.Content as Image;
                if ((ptr.Position.X > i.Width / 2 && ptr.Position.X < Map.Width - i.Width / 2) && (ptr.Position.Y > i.Height / 2 && ptr.Position.Y < Map.Height - i.Height / 2))
                {
                    c.SetValue(Canvas.LeftProperty, ptr.Position.X - i.Width / 2);
                    c.SetValue(Canvas.TopProperty, ptr.Position.Y - i.Height / 2);

                    DroneFocusUpdate(sender, null);
                }
            }
        }
        private void DroneFocusUpdate(object sender, RoutedEventArgs e)
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

                    if(c.FocusState != FocusState.Unfocused)
                        c.SetValue(Canvas.ZIndexProperty, 1);
                    else
                        c.SetValue(Canvas.ZIndexProperty, 0);

                    break;
                }
            }
        }
    }
}
