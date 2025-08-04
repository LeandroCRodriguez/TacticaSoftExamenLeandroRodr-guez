using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticaSoftLeandroRodriguez.Entities
{
    public class Producto
    {
        public int ID { get; set; }

        public string Nombre { get; set; }

        public float Precio { get; set; }

        public string Categoria { get; set; }

        public Producto() { }

        public Producto(int id, string nombre, float precio, string categoria)
        {
            ID = id;
            Nombre = nombre;
            Precio = precio;
            Categoria = categoria;
        }
    }
}

