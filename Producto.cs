using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticaSoftLeandroRodriguez
{
    public class Producto
    {
        public int ID { get; set; }

        public string Nombre { get; set; }

        public float Precio { get; set; }

        public string Categoria { get; set; }

        // Constructor vacío
        public Producto() { }

        // Constructor con parámetros (opcional)
        public Producto(int id, string nombre, float precio, string categoria)
        {
            ID = id;
            Nombre = nombre;
            Precio = precio;
            Categoria = categoria;
        }
    }
}

