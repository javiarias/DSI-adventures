using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Teclado
{
    public class DataDrones
    {
        private static List<Dron> Drones = new List<Dron>()
        {                       
            new Dron()
            {
                Id = 0,
                Nombre = "Dron1",
                Imagen = "Assets\\Samples\\1.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 10,
                Y = 10,
             },
            new Dron()
            {
                Id = 1,
                Nombre = "Dron2",
                Imagen = "Assets\\Samples\\2.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 20,
                Y = 20,
             },
            new Dron()
            {
                Id = 2,
                Nombre = "Dron3",
                Imagen = "Assets\\Samples\\3.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 30,
                Y = 30,
             },
            new Dron()
            {
                Id = 3,
                Nombre = "Dron4",
                Imagen = "Assets\\Samples\\4.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 40,
                Y = 40,
             },
            new Dron()
            {
                Id = 4,
                Nombre = "Dron5",
                Imagen = "Assets\\Samples\\5.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 50,
                Y = 50,
             },
            new Dron()
            {
                Id = 5,
                Nombre = "Dron6",
                Imagen = "Assets\\Samples\\6.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 60,
                Y = 60,
             },
            new Dron()
            {
                Id = 6,
                Nombre = "Dron7",
                Imagen = "Assets\\Samples\\7.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 70,
                Y = 70,
             },
            new Dron()
            {
                Id = 7,
                Nombre = "Dron8",
                Imagen = "Assets\\Samples\\8.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 80,
                Y = 80,
             },
            new Dron()
            {
                Id = 8,
                Nombre = "Dron9",
                Imagen = "Assets\\Samples\\9.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 90,
                Y = 90,
             },
            new Dron()
            {
                Id = 9,
                Nombre = "Dron10",
                Imagen = "Assets\\Samples\\10.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 100,
                Y = 100,
             },
             new Dron()
            {
                Id = 10,
                Nombre = "Dron11",
                Imagen = "Assets\\Samples\\11.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 110,
                Y = 110,
             },
              new Dron()
            {
                Id = 11,
                Nombre = "Dron12",
                Imagen = "Assets\\Samples\\12.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 120,
                Y = 120,
             },
               new Dron()
            {
                Id = 12,
                Nombre = "Dron13",
                Imagen = "Assets\\Samples\\13.jpg",
                Explicacion = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat. Maecenas eu sapien ac urna aliquam dictum. Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                Estado = Dron.estados.Aterrizado,
                X = 230,
                Y = 230,
             }
             
          };

        public static IList<Dron> GetAllDrones()
        {
            return Drones;
        }

        public static Dron GetDronById(int id)
        {
            return Drones[id];
        }
    }
}
