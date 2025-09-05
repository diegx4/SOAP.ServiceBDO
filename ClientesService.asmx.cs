using Npgsql;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace SOAP.ServiceBD
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ClientesService : WebService
    {
        private readonly string connString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=Unach;";

        [WebMethod]
        public List<Cliente> ObtenerClientes()
        {
            var clientes = new List<Cliente>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT id, nombre, correo FROM public.clientes";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new Cliente
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Correo = reader.GetString(2)
                        });
                    }
                }
            }

            return clientes;
        }

        [WebMethod]
        public string InsertarCliente(string nombre, string correo)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO public.clientes (nombre, correo) VALUES (@nombre, @correo)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    cmd.Parameters.AddWithValue("correo", correo);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? "Cliente insertado correctamente" : "No se pudo insertar el cliente";
                }
            }
        }

        [WebMethod]
        public string ActualizarCliente(int id, string nombre, string correo)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE public.clientes SET nombre = @nombre, correo = @correo WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    cmd.Parameters.AddWithValue("correo", correo);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? "Cliente actualizado correctamente" : "No se encontró el cliente";
                }
            }
        }

        [WebMethod]
        public string EliminarCliente(int id)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM public.clientes WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? "Cliente eliminado correctamente" : "No se encontró el cliente";
                }
            }
        }
    }
}
