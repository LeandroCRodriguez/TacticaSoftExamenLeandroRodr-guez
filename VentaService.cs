using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static TacticaSoftLeandroRodriguez.Database;




namespace TacticaSoftLeandroRodriguez
{
    public class VentaService
    {
        public static void AgregarVenta()
        {
            Console.WriteLine("Ingresá el ID de la venta");
            if(!int.TryParse(Console.ReadLine(), out int idVenta))
            {
                Console.WriteLine("No es un número válido");
                return;
            }
            Console.WriteLine("Ingresá el ID del producto");
            if(!int.TryParse(Console.ReadLine(), out int idProducto))
            {
                Console.WriteLine("No es un número válido");
                return;
            }

            Console.WriteLine("Ingresá la cantidad");
            string cantidad = Console.ReadLine();
            
            if (!float.TryParse(cantidad, out float cantidadParseada) || cantidadParseada <= 0)
            {
                Console.WriteLine("La cantidad debe ser mayor a 0");
                return;
            }
            Console.WriteLine("Ingresá el precio unitario del producto");
            if (!float.TryParse(Console.ReadLine(), out float precioUnitario) || precioUnitario <= 0)
            {
                Console.WriteLine("No es un número válido y debe ser mayor a 0");
                return;
            }

            VentaItem item = new VentaItem
            {
                IDVenta = idVenta,
                IDProducto = idProducto,
                Cantidad = cantidadParseada,
                PrecioUnitario = precioUnitario
                // PrecioTotal se calcula automáticamente
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(Database.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO ventasitems " +
                        "(IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal) " +
                        "VALUES (@IDVenta, @IDProducto, @PrecioUnitario, @Cantidad, @PrecioTotal)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IDVenta", item.IDVenta);
                        cmd.Parameters.AddWithValue("@IDProducto", item.IDProducto);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                        cmd.Parameters.AddWithValue("@PrecioTotal", item.PrecioTotal);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Venta agregada exitosamente");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error al agregar la venta " +ex.Message);
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
        public static void EliminarVenta()
        {
            Console.WriteLine("Ingresá el ID de la venta que querés eliminar");
            if (!int.TryParse(Console.ReadLine(), out int idVenta))
            {
                Console.WriteLine("No es un número válido");
                return;
            }

            Venta venta = new Venta
            {
                ID = idVenta
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(Database.connectionString))
                {
                    connection.Open();

                    // Primero eliminamos los ítems asociados a la venta
                    string queryItems = "DELETE FROM ventasitems WHERE IDVenta = @IDVenta";
                    using (SqlCommand cmdItems = new SqlCommand(queryItems, connection))
                    {
                        cmdItems.Parameters.AddWithValue("@IDVenta", venta.ID);
                        cmdItems.ExecuteNonQuery();
                    }

                    // Luego eliminamos la venta
                    string queryVenta = "DELETE FROM ventas WHERE ID = @ID";
                    using (SqlCommand cmdVenta = new SqlCommand(queryVenta, connection))
                    {
                        cmdVenta.Parameters.AddWithValue("@ID", venta.ID);
                        int filasAfectadas = cmdVenta.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine("Venta eliminada exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró una venta con ese ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar la venta: " + ex.Message);
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }


        public static void ModificarVenta()
        {
            Console.WriteLine("Ingresá el ID de la venta");
            if (!int.TryParse(Console.ReadLine(), out int idVenta))
            {
                Console.WriteLine("No es un número válido");
                return;
            }
            Console.WriteLine("Ingresá el ID del producto");
            if (!int.TryParse(Console.ReadLine(), out int idProducto))
            {
                Console.WriteLine("No es un número válido");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Database.connectionString))
                {
                    connection.Open();
                    string query = "SELECT 1 FROM ventasitems WHERE IDVenta = @IDVenta AND IDProducto = @IDProducto";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IDVenta", idVenta);
                        cmd.Parameters.AddWithValue("@IDProducto", idProducto);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No existe una venta con ese ID de venta y ese ID de producto.");
                                return;
                            }
                        }
                    }

                    VentaItem item = new VentaItem
                    {
                        IDVenta = idVenta,
                        IDProducto = idProducto
                    };

                    Console.WriteLine("Ingresá la nueva cantidad");
                    if (!float.TryParse(Console.ReadLine(), out float nuevaCantidad) || nuevaCantidad <= 0)
                    {
                        Console.WriteLine("La cantidad debe ser mayor a 0");
                        return;
                    }

                    Console.WriteLine("Ingresá el precio");
                    if (!float.TryParse(Console.ReadLine(), out float nuevoPrecio) || nuevoPrecio <= 0)
                    {
                        Console.WriteLine("El precio debe ser mayor a 0");
                        return;
                    }

                    item.Cantidad = nuevaCantidad;
                    item.PrecioUnitario = nuevoPrecio;

                    string updateQuery =
                        "UPDATE ventasitems SET PrecioUnitario = @PrecioUnitario, " +
                        "Cantidad = @Cantidad, PrecioTotal = @PrecioTotal " +
                        "WHERE IDVenta = @IDVenta AND IDProducto = @IDProducto";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario);
                        updateCmd.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                        updateCmd.Parameters.AddWithValue("@PrecioTotal", item.PrecioTotal);
                        updateCmd.Parameters.AddWithValue("@IDVenta", item.IDVenta);
                        updateCmd.Parameters.AddWithValue("@IDProducto", item.IDProducto);
                        int filasAfectadas = updateCmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine("Venta modificada exitosamente");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró una venta con ese ID de venta y ese ID de producto.");
                        }
                    }
                }
                }catch (Exception ex)
                {
                    Console.WriteLine("Error al buscar la venta: " + ex.Message);
                    return;
                }
            }
                

        public static void MostrarTotalVentas()
        {
            Console.WriteLine("Total de ventas realizadas: ");

            using (SqlConnection connection = new SqlConnection(Database.connectionString))
            {
                connection.Open();
                string query = "SELECT IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal FROM ventasitems";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader  reader = cmd.ExecuteReader())
                    {
                        float totalGeneral = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["IDVenta"]} " +
                                $"| Producto: {reader["IDProducto"]} " +
                                $"| Precio unitario: {reader["PrecioUnitario"]} " +
                                $"| Cantidad: {reader["Cantidad"]} " +
                                $"| Precio total: {reader["PrecioTotal"]}");
                            Console.WriteLine();
                            totalGeneral += Convert.ToSingle(reader["PrecioTotal"]);
                        }
                        Console.WriteLine($"TOTAL GENERAL: {totalGeneral}");
                    }
                }
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        public static void BuscarVentaPorId()
        {
            Console.WriteLine("Ingrese el ID de la venta a buscar: ");
            string idVenta = Console.ReadLine();

            if (!int.TryParse(idVenta, out int idParseado))
            {
                Console.WriteLine("ID inválido. Debe ser un número entero.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal FROM ventasitems WHERE IDVenta = @IDVenta";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDVenta", idParseado);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                float totalGeneral = 0;

                                while (reader.Read())
                                {
                                    Console.WriteLine($"Venta Nº: {reader["IDVenta"]} | Producto: {reader["IDProducto"]} | Precio: {reader["PrecioUnitario"]} | Cantidad: {reader["Cantidad"]} | Precio total: {reader["PrecioTotal"]}");
                                    Console.WriteLine();
                                    totalGeneral += Convert.ToSingle(reader["PrecioTotal"]);
                                }
                                Console.WriteLine($"TOTAL GENERAL: {totalGeneral}");
                            }
                            else
                            {
                                Console.WriteLine("No se encontraron ventas con ese ID");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar la venta " + ex.Message);
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
    }
}
