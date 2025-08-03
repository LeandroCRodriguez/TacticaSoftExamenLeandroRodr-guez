using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TacticaSoftLeandroRodriguez.Database;



namespace TacticaSoftLeandroRodriguez
{
    public class ProductoService
    {

        public static void ListarProductos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Nombre, Precio, Categoria FROM Productos";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["ID"]} | Nombre: {reader["Nombre"]} | Precio: {reader["Precio"]} | Categoria: {reader["Categoria"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar productos: " + ex.Message);
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        public static void EliminarProducto()
        {
            Producto producto = new Producto(); 

            Console.WriteLine("ingrese ID del producto a eliminar");
            if (!int.TryParse(Console.ReadLine(), out int productoId))
            {
                Console.WriteLine("ID inválido.");
                return;
            }
            producto.ID = productoId;
            try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Productos WHERE Id = @ID";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", producto.ID);
                            int filas = command.ExecuteNonQuery();

                            if (filas > 0)
                            {
                                Console.WriteLine($"{filas} producto eliminado correctamente.");
                            }
                            else
                            {
                                Console.WriteLine("No se encontró un producto con ese ID.");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar el producto: " + ex.Message);
                }      
            System.Threading.Thread.Sleep(2000);
        }


        public static void ModificarProducto()
        {
            Console.WriteLine("Ingresá el Id del producto que querés modificar");
            int.TryParse(Console.ReadLine(), out int productoId);

            Console.WriteLine("Ingresá el nuevo nombre o presioná enter para dejarlo igual");
            string nuevoNombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nuevoNombre) && (nuevoNombre.Length < 3 || nuevoNombre.Any(char.IsDigit)))
            {
                Console.WriteLine("El nombre debe tener al menos 3 letras y no contener números.");
                return;
            }

            Console.WriteLine("Ingresá el nuevo precio o presioná enter para dejarlo igual");
            string precioTexto = Console.ReadLine();
            // Usamos decimal? para permitir que el precio sea nulo si no se ingresa un valor
            decimal? nuevoPrecio = null;
            if (!string.IsNullOrWhiteSpace(precioTexto))
            {
                if (!decimal.TryParse(precioTexto, out decimal precioConvertido))
                {
                    Console.WriteLine("Precio inválido.");
                    return;
                }
                else if (precioConvertido <= 0)
                {
                    Console.WriteLine("El precio debe ser un número mayor a 0.");
                    return;
                }
                nuevoPrecio = precioConvertido;
            }



            Console.WriteLine("Ingresá la nueva categoría o presioná enter para dejarlo igual");
            string nuevaCategoria = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nuevaCategoria) && nuevaCategoria.Length < 3)
            {
                Console.WriteLine("La categoría debe tener al menos 3 caracteres.");
                return;
            }

            string campos = "";
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
                campos += "Nombre = @Nombre, ";
            if (nuevoPrecio.HasValue)
                campos += "Precio = @Precio, ";
            if (!string.IsNullOrWhiteSpace(nuevaCategoria))
                campos += "Categoria = @Categoria, ";
            campos = campos.TrimEnd(',', ' ');

            if (string.IsNullOrEmpty(campos))
            {
                Console.WriteLine("No se ingresaron datos para modificar.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"UPDATE Productos SET {campos} WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrWhiteSpace(nuevoNombre))
                            command.Parameters.AddWithValue("@Nombre", nuevoNombre);
                        if (nuevoPrecio.HasValue)
                            command.Parameters.AddWithValue("@Precio", nuevoPrecio.Value);
                        if (!string.IsNullOrWhiteSpace(nuevaCategoria))
                            command.Parameters.AddWithValue("@Categoria", nuevaCategoria);

                        command.Parameters.AddWithValue("@ID", productoId);

                        int filas = command.ExecuteNonQuery();
                        Console.WriteLine($"{filas} producto modificado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al modificar el producto: " + ex.Message);

            }
            System.Threading.Thread.Sleep(2000);
        }


        public static void AgregarProducto()
        {
            Producto producto = new Producto();

            Console.WriteLine("Nombre del Producto: ");
            producto.Nombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(producto.Nombre) || producto.Nombre.Length < 3 || producto.Nombre.Any(char.IsDigit))
            {
                Console.WriteLine("El nombre debe tener al menos 3 letras y no contener números.");
                return;
            }

            Console.WriteLine("Precio: ");
            if (!float.TryParse(Console.ReadLine(), out float precio))
            {
                Console.WriteLine("Precio inválido.");
                return;
            }
            else if (precio <= 0)
            {
                Console.WriteLine("El precio debe ser un número mayor a 0.");
                return;
            }
            producto.Precio = precio;


            Console.WriteLine("Categoría: ");
            producto.Categoria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(producto.Categoria) || producto.Categoria.Length < 3)
            {
                Console.WriteLine("La categoría debe tener al menos 3 caracteres.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Productos (Nombre, Precio, Categoria) VALUES (@Nombre, @Precio, @Categoria)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                        command.Parameters.AddWithValue("@Precio", producto.Precio);
                        command.Parameters.AddWithValue("@Categoria", producto.Categoria);
                        int filas = command.ExecuteNonQuery();
                        Console.WriteLine($"{filas} Producto agregado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar al producto: " + ex.Message);

            }
            System.Threading.Thread.Sleep(2000);
        }

        public static void BuscarProductoPorNombre()
        {
            Console.WriteLine("Ingrese el nombre del producto: ");
            string nombreProducto = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nombreProducto) || nombreProducto.Length < 2)
            {
                Console.WriteLine("Tenés que ingresar al menos 3 caracteres.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Nombre, Precio, Categoria FROM Productos WHERE Nombre LIKE @Nombre";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", "%" + nombreProducto + "%");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID: {reader["ID"]} | Nombre: {reader["Nombre"]} | Precio: {reader["Precio"]} | Categoría: {reader["Categoria"]}");
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("No se encontraron productos con ese nombre.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar el producto: " + ex.Message);
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        public static void BuscarProductoPorId()
        {
            Console.WriteLine("Ingrese el ID del producto a buscar: ");
            string idProducto = Console.ReadLine();

            if (!int.TryParse(idProducto, out int idParseado))
            {
                Console.WriteLine("ID inválido. Debe ser un número entero.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Nombre, Precio, Categoria FROM Productos WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", idParseado);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID: {reader["ID"]} | Nombre: {reader["Nombre"]} | Precio: {reader["Precio"]} | Categoría: {reader["Categoria"]}");
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("No se encontraron productos con ese ID");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar el producto " + ex.Message);
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
    }
}
