using System;
using Microsoft.Data.SqlClient;
using TacticaSoftLeandroRodriguez.Services;

namespace TacticaSoftLeandroRodriguez
{
    class Program
    {
        static void Main(string[] args)
        {
            int opcion = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("1. Gestión de Clientes");
                Console.WriteLine("2. Gestión de Productos");
                Console.WriteLine("0. Salir");
                Console.Write("Elegí una opción: ");


                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 0:
                            Console.WriteLine("Saliendo...");
                            break;
                        case 1:
                            SubmenuClientes();
                            break;
                        case 2:
                            SubmenuProductos();
                            break;
                        default:
                            Console.WriteLine("Opción inválida.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no válida. Por favor, ingresá un número.");
                }

                System.Threading.Thread.Sleep(1000);

            } while (opcion != 0);
        }

        static void SubmenuClientes()
        {
            int opcionCliente = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Agregar cliente");
                Console.WriteLine("2. Modificar cliente");
                Console.WriteLine("3. Eliminar cliente");
                Console.WriteLine("4. Listar clientes");
                Console.WriteLine("5. Buscar cliente por nombre");
                Console.WriteLine("6. Buscar cliente por ID");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("Elegí una opción: ");

                if (int.TryParse(Console.ReadLine(), out opcionCliente))
                {
                    switch (opcionCliente)
                    {
                        case 0: break;
                        case 1: ClienteService.AgregarCliente(); break;
                        case 2: ClienteService.ModificarCliente(); break;
                        case 3: ClienteService.EliminarCliente(); break;
                        case 4: ClienteService.ListarClientes(); break;
                        case 5: ClienteService.BuscarClientePorNombre(); break;
                        case 6: ClienteService.BuscarClientePorId(); break;
                        default: Console.WriteLine("Opción inválida."); break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no válida.");
                }

                System.Threading.Thread.Sleep(1000);

            } while (opcionCliente != 0);
        }

        static void SubmenuProductos()
        {
            int opcionProducto = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Agregar producto");
                Console.WriteLine("2. Modificar producto");
                Console.WriteLine("3. Eliminar producto");
                Console.WriteLine("4. Listar productos");
                Console.WriteLine("5. Buscar producto por nombre");
                Console.WriteLine("6. Buscar producto por ID");
                Console.WriteLine("7. Agregar una venta");
                Console.WriteLine("8. Modificar ventas");
                Console.WriteLine("9. Eliminar ventas");
                Console.WriteLine("10. Mostrar el total de las ventas");
                Console.WriteLine("11. Buscar ventas por ID");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("Elegí una opción: ");

                if (int.TryParse(Console.ReadLine(), out opcionProducto))
                {
                    switch (opcionProducto)
                    {
                        case 0: break;
                        case 1: ProductoService.AgregarProducto(); break;
                        case 2: ProductoService.ModificarProducto(); break;
                        case 3: ProductoService.EliminarProducto(); break;
                        case 4: ProductoService.ListarProductos(); break;
                        case 5: ProductoService.BuscarProductoPorNombre(); break;
                        case 6: ProductoService.BuscarProductoPorId(); break;
                        case 7: VentaService.AgregarVenta(); break;
                        case 8: VentaService.ModificarVenta(); break;
                        case 9: VentaService.EliminarVenta(); break;
                        case 10: VentaService.MostrarTotalVentas(); break;
                        case 11: VentaService.BuscarVentaPorId(); break;
                        default: Console.WriteLine("Opción inválida."); break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no válida.");
                }

                System.Threading.Thread.Sleep(1000);

            } while (opcionProducto != 0);
        }
    }
}
