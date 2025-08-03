using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TacticaSoftLeandroRodriguez.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TacticaSoftLeandroRodriguez.Repositories.Database;


namespace TacticaSoftLeandroRodriguez.Services
{
    public class ClienteService
    {

        public static void ListarClientes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Cliente, Telefono, Correo FROM Clientes";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["ID"]} | Nombre: {reader["Cliente"]} | Tel: {reader["Telefono"]} | Correo: {reader["Correo"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar clientes: " + ex.Message);
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        public static void EliminarCliente()
        {
            Cliente cliente = new Cliente();

            Console.WriteLine("ingrese ID del cliente a eliminar");
            if (int.TryParse(Console.ReadLine(), out int clienteId))
            {
                cliente.ID = clienteId;
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Clientes WHERE Id = @Id";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", cliente.ID);
                            int filas = command.ExecuteNonQuery();

                            if (filas > 0)
                            {
                                Console.WriteLine($"{filas} cliente eliminado correctamente.");
                            }
                            else
                            {
                                Console.WriteLine("No se encontró un cliente con ese ID.");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar el cliente: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("ID Inválido");
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }


        public static void ModificarCliente()
        {
            Cliente cliente = new Cliente();

            Console.WriteLine("Ingresá el Id del cliente que querés modificar");
            if (!int.TryParse(Console.ReadLine(), out int idCliente))
            {
                Console.WriteLine("ID inválido.");
                return;
            }
            cliente.ID = idCliente;

            Console.WriteLine("Ingresá el nuevo nombre o presioná enter para dejarlo igual");
            string nuevoNombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nuevoNombre) && (nuevoNombre.Length < 3 || nuevoNombre.Any(char.IsDigit)))
            {
                Console.WriteLine("El nombre debe tener al menos 3 letras y no contener números.");
                return;
            }
            cliente.Nombre = nuevoNombre;


            Console.WriteLine("Ingresá el nuevo teléfono o presioná enter para dejarlo igual");
            string nuevoTelefono = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nuevoTelefono) && (!nuevoTelefono.All(char.IsDigit) || nuevoTelefono.Length < 8))
            {
                Console.WriteLine("El teléfono debe contener solo números y tener al menos 8 dígitos.");
                return;
            }
            cliente.Telefono = nuevoTelefono;


            Console.WriteLine("Ingresá el nuevo correo o presioná enter para dejarlo igual");
            string nuevoCorreo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nuevoCorreo) && !nuevoCorreo.Contains('@'))
            {
                Console.WriteLine("El correo debe contener un '@'.");
                return;
            }
            cliente.Correo = nuevoCorreo;

            string campos = "";
            if (cliente.Nombre != null)
                campos += "Cliente = @Cliente, ";
            if (cliente.Telefono != null)
                campos += "Telefono = @Telefono, ";
            if (cliente.Correo != null)
                campos += "Correo = @Correo, ";
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
                    string query = $"UPDATE Clientes SET {campos} WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (cliente.Nombre != null)
                            command.Parameters.AddWithValue("@Cliente", cliente.Nombre);
                        if (cliente.Telefono != null)
                            command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        if (cliente.Correo != null)
                            command.Parameters.AddWithValue("@Correo", cliente.Correo);

                        command.Parameters.AddWithValue("@ID", cliente.ID);

                        int filas = command.ExecuteNonQuery();
                        Console.WriteLine($"{filas} cliente modifcado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al modificar el cliente: " + ex.Message);

            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }


        public static void AgregarCliente()
        {
            Cliente cliente = new Cliente();

            Console.WriteLine("Nombre del cliente: ");
            cliente.Nombre = Console.ReadLine();
            if (cliente.Nombre.Length < 3 || cliente.Nombre.Any(char.IsDigit))
            {
                Console.WriteLine("El nombre debe tener al menos 3 letras y no contener números.");
                return;
            }

            Console.WriteLine("Teléfono: ");
            cliente.Telefono = Console.ReadLine();
            if (!cliente.Telefono.All(char.IsDigit) || cliente.Telefono.Length < 8)
            {
                Console.WriteLine("El teléfono debe contener solo números y tener al menos 8 dígitos.");
                return;
            }

            Console.WriteLine("Correo: ");
            cliente.Correo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cliente.Correo) || !cliente.Correo.Contains('@'))
            {
                Console.WriteLine("El correo debe contener un '@'.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Clientes (Cliente, Telefono, Correo) VALUES (@Cliente, @Telefono, @Correo)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Cliente", cliente.Nombre);
                        command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        command.Parameters.AddWithValue("@Correo", cliente.Correo);
                        int filas = command.ExecuteNonQuery();
                        Console.WriteLine($"{filas} cliente agregado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar al cliente: " + ex.Message);

            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        public static void BuscarClientePorNombre()
        {
            Console.WriteLine("Ingrese el nombre del cliente a buscar: ");
            string nombreCliente = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nombreCliente) || nombreCliente.Length < 2)
            {
                Console.WriteLine("Tenés que ingresar al menos 2 caracteres.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Cliente, Telefono, Correo FROM Clientes WHERE Cliente LIKE @Cliente";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Cliente", "%" + nombreCliente + "%");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID: {reader["ID"]} | Nombre: {reader["Cliente"]} | Tel: {reader["Telefono"]} | Correo: {reader["Correo"]}");
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("No se encontraron clientes con ese nombre.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar el cliente: " + ex.Message);
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        public static void BuscarClientePorId()
        {
            Console.WriteLine("Ingrese el ID del cliente a buscar: ");
            string idCliente = Console.ReadLine();

            if (!int.TryParse(idCliente, out int idParseado))
            {
                Console.WriteLine("ID inválido. Debe ser un número entero.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Cliente, Telefono, Correo FROM Clientes WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", idParseado);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID: {reader["ID"]} | Nombre: {reader["Cliente"]} | Tel: {reader["Telefono"]} | Correo: {reader["Correo"]}");
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("No se encontraron clientes con ese ID");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar el cliente" + ex.Message);
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
    }
}
