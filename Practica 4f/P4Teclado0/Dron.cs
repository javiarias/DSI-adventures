using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;


namespace P4Teclado
{

    public class Dron
    {
        public enum estados { Aterrizado, Autonomo, Manual };

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public Image Img;
        public string Explicacion { get; set; }
        public estados Estado { get; set; }
        public int X { get; set; }
        public int Y { get; set; }



        public Dron() { }


        public Dron(int Id_, string Nombre_, string Imagen_, int X_, int Y_)
        {

            Id = Id_;
            Nombre = Nombre_;
            Imagen = Imagen_;
            Img = null;
            Explicacion = "Bla bla ............................................\n Bla......................................................Bla.............Bla Bla......................................Bla, Bla ........................................Bla ...........................Bla";
            Estado = estados.Aterrizado;
            X = X_;
            Y = Y_;
        }



    }
 }