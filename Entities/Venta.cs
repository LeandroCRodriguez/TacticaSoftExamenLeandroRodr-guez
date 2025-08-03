using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticaSoftLeandroRodriguez.Entities
{
    public class Venta
    {
        public int ID { get; set; }
        public int IDCliente { get; set; }
        public DateTime Fecha { get; set; }
        public float Total { get; set; }

        public List<VentaItem> Items { get; set; } = new List<VentaItem>();

    }
}
