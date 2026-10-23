using API.ENTIDADES;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace API.DAPPER
{
    public class ClientesDAL
    {

        // Procedimientos almacenados

        private const string sp_InsertarClientes = "InsertarClientes";
        private const string sp_ObtenerClientes = "ObtenerClientes";
        private const string sp_ObtenerClientePorId = "ObtenerClientePorId";
        private const string sp_ActualizarClientePorId = "ActualizarClientePorId";
        private const string sp_EliminarClientePorId = "EliminarClientePorId";

        // Número de error que usa SQL Server para los mensajes de RAISERROR de los procedimientos
        private const int ErrorDeValidacionSQL = 50000;


        //Instancias

        private readonly IConfiguration _Config;


        public ClientesDAL(IConfiguration configuration)
        {

            _Config = configuration;

        }


        public Respuesta<ClientesDto> InsertarClientes(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_InsertarClientes, connection))
                    {

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Los campos opcionales se envían como DBNull.Value cuando vienen null
                        command.Parameters.Add(new SqlParameter("@pCedula", System.Data.SqlDbType.NVarChar, 20) { Value = ObjCliente.Cedula });
                        command.Parameters.Add(new SqlParameter("@pNombre", System.Data.SqlDbType.NVarChar, 50) { Value = ObjCliente.Nombre });
                        command.Parameters.Add(new SqlParameter("@pApellido1", System.Data.SqlDbType.NVarChar, 50) { Value = ObjCliente.Apellido1 });
                        command.Parameters.Add(new SqlParameter("@pApellido2", System.Data.SqlDbType.NVarChar, 50) { Value = (object?)ObjCliente.Apellido2 ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@pTelefono", System.Data.SqlDbType.NVarChar, 20) { Value = ObjCliente.Telefono });
                        command.Parameters.Add(new SqlParameter("@pCorreo", System.Data.SqlDbType.NVarChar, 100) { Value = ObjCliente.Correo });
                        command.Parameters.Add(new SqlParameter("@pDireccion", System.Data.SqlDbType.NVarChar, 500) { Value = (object?)ObjCliente.Direccion ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@pIdProvincia", System.Data.SqlDbType.Int) { Value = ObjCliente.IdProvincia });

                        // El procedimiento devuelve el IdCliente generado
                        object? IdGenerado = command.ExecuteScalar();

                        if (IdGenerado != null && IdGenerado != DBNull.Value)
                        {

                            ObjCliente.IdCliente = Convert.ToInt32(IdGenerado);

                            respuesta.Ok = true;
                            respuesta.Mensaje = $"El cliente ha sido agregado de manera exitosa con el Id {ObjCliente.IdCliente}";
                            respuesta.ValorRetorno = ObjCliente;

                        }
                        else
                        {

                            respuesta.Ok = false;
                            respuesta.Mensaje = "Ha ocurrido un error al insertar el cliente en la base de datos";
                            respuesta.ValorRetorno = null;
                        }

                    }

                }

            }
            catch (SqlException ex) when (ex.Number == ErrorDeValidacionSQL)
            {

                // Mensaje de validación que viene del procedimiento (cédula repetida, provincia inexistente)
                respuesta.Ok = false;
                respuesta.Mensaje = ex.Message;
                respuesta.ValorRetorno = null;
            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método InsertarClientes en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<List<ClientesDto>> ObtenerClientes()
        {

            Respuesta<List<ClientesDto>> respuesta = new Respuesta<List<ClientesDto>>();
            List<ClientesDto> ListaClientes = new List<ClientesDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_ObtenerClientes, connection))
                    {

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                ListaClientes.Add(LeerCliente(reader));

                            }

                            respuesta.Ok = true;
                            respuesta.Mensaje = $"Se ha obtenido la información de los clientes de manera exitosa ({ListaClientes.Count} registros)";
                            respuesta.ValorRetorno = ListaClientes;

                        }

                    }

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método ObtenerClientes en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<ClientesDto> ObtenerClientePorId(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_ObtenerClientePorId, connection))
                    {

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pIdCliente", System.Data.SqlDbType.Int) { Value = ObjCliente.IdCliente });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                respuesta.Ok = true;
                                respuesta.Mensaje = "Se ha obtenido la información del cliente de manera exitosa";
                                respuesta.ValorRetorno = LeerCliente(reader);

                            }
                            else
                            {

                                respuesta.Ok = false;
                                respuesta.Mensaje = $"El cliente con el Id {ObjCliente.IdCliente} no ha sido encontrado";
                                respuesta.ValorRetorno = null;

                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método ObtenerClientePorId en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<ClientesDto> ActualizarClientePorId(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_ActualizarClientePorId, connection))
                    {

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pIdCliente", System.Data.SqlDbType.Int) { Value = ObjCliente.IdCliente });
                        command.Parameters.Add(new SqlParameter("@pCedula", System.Data.SqlDbType.NVarChar, 20) { Value = ObjCliente.Cedula });
                        command.Parameters.Add(new SqlParameter("@pNombre", System.Data.SqlDbType.NVarChar, 50) { Value = ObjCliente.Nombre });
                        command.Parameters.Add(new SqlParameter("@pApellido1", System.Data.SqlDbType.NVarChar, 50) { Value = ObjCliente.Apellido1 });
                        command.Parameters.Add(new SqlParameter("@pApellido2", System.Data.SqlDbType.NVarChar, 50) { Value = (object?)ObjCliente.Apellido2 ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@pTelefono", System.Data.SqlDbType.NVarChar, 20) { Value = ObjCliente.Telefono });
                        command.Parameters.Add(new SqlParameter("@pCorreo", System.Data.SqlDbType.NVarChar, 100) { Value = ObjCliente.Correo });
                        command.Parameters.Add(new SqlParameter("@pDireccion", System.Data.SqlDbType.NVarChar, 500) { Value = (object?)ObjCliente.Direccion ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@pIdProvincia", System.Data.SqlDbType.Int) { Value = ObjCliente.IdProvincia });

                        int FilasAfectadas = command.ExecuteNonQuery();

                        if (FilasAfectadas > 0)
                        {

                            respuesta.Ok = true;
                            respuesta.Mensaje = "El cliente ha sido actualizado de manera exitosa";
                            respuesta.ValorRetorno = ObjCliente;

                        }
                        else
                        {

                            respuesta.Ok = false;
                            respuesta.Mensaje = $"No se encontró un cliente con el Id {ObjCliente.IdCliente} para actualizar";
                            respuesta.ValorRetorno = null;
                        }

                    }

                }

            }
            catch (SqlException ex) when (ex.Number == ErrorDeValidacionSQL)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = ex.Message;
                respuesta.ValorRetorno = null;
            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método ActualizarClientePorId en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<ClientesDto> EliminarClientePorId(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_EliminarClientePorId, connection))
                    {

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pIdCliente", System.Data.SqlDbType.Int) { Value = ObjCliente.IdCliente });

                        int FilasAfectadas = command.ExecuteNonQuery();

                        if (FilasAfectadas > 0)
                        {

                            respuesta.Ok = true;
                            respuesta.Mensaje = $"El cliente con el Id {ObjCliente.IdCliente} ha sido eliminado de manera exitosa";
                            respuesta.ValorRetorno = null;

                        }
                        else
                        {

                            respuesta.Ok = false;
                            respuesta.Mensaje = $"No se encontró un cliente con el Id {ObjCliente.IdCliente} para eliminar";
                            respuesta.ValorRetorno = null;
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método EliminarClientePorId en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        // Convierte la fila actual del reader en un ClientesDto.
        // Se usa "as" en las columnas que permiten NULL en la base de datos.
        private static ClientesDto LeerCliente(SqlDataReader reader)
        {

            return new ClientesDto
            {
                IdCliente = (int)reader["IdCliente"],
                Cedula = (string)reader["Cedula"],
                Nombre = (string)reader["Nombre"],
                Apellido1 = (string)reader["Apellido1"],
                Apellido2 = reader["Apellido2"] as string,
                Telefono = (string)reader["Telefono"],
                Correo = (string)reader["Correo"],
                Direccion = reader["Direccion"] as string,
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                IdProvincia = (int)reader["IdProvincia"],
                NombreProvincia = (string)reader["NombreProvincia"]
            };

        }

    }
}
