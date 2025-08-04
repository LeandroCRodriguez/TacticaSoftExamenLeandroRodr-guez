using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TacticaSoftLeandroRodriguez.Entities;
using static TacticaSoftLeandroRodriguez.Repositories.Database;




namespace TacticaSoftLeandroRodriguez.Services
{
    public class VentaService
    {
        public static void AgregarVenta()
        {
            Venta venta = new Venta();
            venta.Fecha = DateTime.Now;         

            Console.WriteLine("Ingresá el ID del producto:");
            if (!int.TryParse(Console.ReadLine(), out int idProducto))
            {
                Console.WriteLine("No es un número válido.");
                return;
            }

            Console.WriteLine("Ingresá el ID del cliente:");
            if (!int.TryParse(Console.ReadLine(), out int idCliente))
            {
                Console.WriteLine("No es un número válido.");
                return;
            }

            venta.IDCliente = idCliente;


            Console.WriteLine("Ingresá la cantidad:");
            if (!float.TryParse(Console.ReadLine(), out float cantidad) || cantidad <= 0)
            {
                Console.WriteLine("La cantidad debe ser mayor a 0.");
                return;
            }

            Console.WriteLine("Ingresá el precio unitario del producto:");
            if (!float.TryParse(Console.ReadLine(), out float precioUnitario) || precioUnitario <= 0)
            {
                Console.WriteLine("El precio debe ser mayor a 0.");
                return;
            }

            VentaItem item = new VentaItem
            {
                IDVenta = venta.ID,
                IDProducto = idProducto,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario
                // PrecioTotal se calcula automáticamente desde la clase VentaItem.cs
            };

            venta.Items.Add(item);
            venta.Total = item.PrecioTotal;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertVenta = "INSERT INTO ventas (IDCliente, Fecha, Total) " +
                                 "OUTPUT INSERTED.ID " +
                                 "VALUES (@IDCliente, @Fecha, @Total)";
                    using (SqlCommand cmdVenta = new SqlCommand(insertVenta, connection))
                    {
                        cmdVenta.Parameters.AddWithValue("@IDCliente", venta.IDCliente);
                        cmdVenta.Parameters.AddWithValue("@Fecha", venta.Fecha);
                        cmdVenta.Parameters.AddWithValue("@Total", venta.Total);
                        venta.ID = Convert.ToInt32(cmdVenta.ExecuteScalar());
                    }


                    foreach (var ventaItem in venta.Items)
                    {
                        ventaItem.IDVenta = venta.ID;
                        string queryItem = "INSERT INTO ventasitems " +
                                           "(IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal) " +
                                           "VALUES (@IDVenta, @IDProducto, @PrecioUnitario, @Cantidad, @PrecioTotal)";
                        using (SqlCommand cmdItem = new SqlCommand(queryItem, connection))
                        {
                            cmdItem.Parameters.AddWithValue("@IDVenta", ventaItem.IDVenta);
                            cmdItem.Parameters.AddWithValue("@IDProducto", ventaItem.IDProducto);
                            cmdItem.Parameters.AddWithValue("@PrecioUnitario", ventaItem.PrecioUnitario);
                            cmdItem.Parameters.AddWithValue("@Cantidad", ventaItem.Cantidad);
                            cmdItem.Parameters.AddWithValue("@PrecioTotal", ventaItem.PrecioTotal);
                            cmdItem.ExecuteNonQuery();
                        }
                    }
                }

                Console.WriteLine("Venta agregada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar la venta: " + ex.Message);
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

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string queryItems = "DELETE FROM ventasitems WHERE IDVenta = @IDVenta";
                    using (SqlCommand cmdItems = new SqlCommand(queryItems, connection))
                    {
                        cmdItems.Parameters.AddWithValue("@IDVenta", idVenta);
                        cmdItems.ExecuteNonQuery();
                    }
                    string queryVenta = "DELETE FROM ventas WHERE ID = @IDVenta";

                    using (SqlCommand cmdVenta = new SqlCommand(queryVenta, connection))
                    {
                        cmdVenta.Parameters.AddWithValue("@IDVenta", idVenta);
                        int filas = cmdVenta.ExecuteNonQuery();
                        if (filas > 0)
                        {
                            Console.WriteLine($"Venta ID {idVenta} eliminada correctamente.");
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
                using (SqlConnection connection = new SqlConnection(connectionString))
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
                        updateCmd.ExecuteNonQuery();
                    }
                    string calcularTotal = "SELECT SUM(PrecioTotal) FROM ventasitems WHERE IDVenta = @IDVenta";
                    float nuevoTotal = 0;
                    using (SqlCommand cmdSuma = new SqlCommand(calcularTotal, connection))
                    {
                        cmdSuma.Parameters.AddWithValue("@IDVenta", idVenta);
                        object resultado = cmdSuma.ExecuteScalar();
                        if (resultado != DBNull.Value)
                        {
                            nuevoTotal = Convert.ToSingle(resultado);
                        }
                    }

                    string updateVenta = "UPDATE ventas SET Total = @Total WHERE ID = @IDVenta";
                    using (SqlCommand cmdUpdateVenta = new SqlCommand(updateVenta, connection))
                    {
                        cmdUpdateVenta.Parameters.AddWithValue("@Total", nuevoTotal);
                        cmdUpdateVenta.Parameters.AddWithValue("@IDVenta", idVenta);
                        int filas = cmdUpdateVenta.ExecuteNonQuery();

                        if (filas > 0)
                            Console.WriteLine("Venta modificada exitosamente.");
                        else
                            Console.WriteLine("No se pudo actualizar el total de la venta.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar la venta: " + ex.Message);
                return;
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }


        public static void MostrarTotalVentas()
        {
            Console.WriteLine("Total de ventas realizadas: ");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal FROM ventasitems";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
