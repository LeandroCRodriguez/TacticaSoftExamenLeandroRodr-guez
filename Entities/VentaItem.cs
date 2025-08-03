using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticaSoftLeandroRodriguez.Entities
{
    public class VentaItem
    {
        public int ID { get; set; }
        public int IDVenta { get; set; }
        public int IDProducto { get; set; }
        public float PrecioUnitario { get; set; }
        public float Cantidad { get; set; }

        public float PrecioTotal
        {
            get { return PrecioUnitario * Cantidad; }
            set { /* necesario para leer desde la base */ }
        }
    }

}
