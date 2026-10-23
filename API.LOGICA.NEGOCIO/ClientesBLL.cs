using API.DAPPER;
using API.ENTIDADES;
using System.Text.RegularExpressions;

namespace API.LOGICA.NEGOCIO
{
    public class ClientesBLL
    {

        //Instancia

        private readonly ClientesDAL _ClientesDAL;


        //Constructor

        public ClientesBLL(ClientesDAL clientesDAL)
        {

            _ClientesDAL = clientesDAL;

        }


        public Respuesta<ClientesDto> InsertarClientes(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                string? MensajeValidacion = ValidarCliente(ObjCliente);

                if (MensajeValidacion == null)
                {

                    var reply = _ClientesDAL.InsertarClientes(ObjCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }

                }
                else
                {

                    respuesta.Ok = false;
                    respuesta.Mensaje = MensajeValidacion;
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método InsertarClientes {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<List<ClientesDto>> ObtenerClientes()
        {

            Respuesta<List<ClientesDto>> respuesta = new Respuesta<List<ClientesDto>>();

            try
            {

                var reply = _ClientesDAL.ObtenerClientes();

                if (reply != null)
                {
                    respuesta = reply;
                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método ObtenerClientes {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<ClientesDto> ObtenerClientePorId(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                if (ObjCliente.IdCliente > 0)
                {

                    var reply = _ClientesDAL.ObtenerClientePorId(ObjCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }

                }
                else
                {

                    respuesta.Ok = false;
                    respuesta.Mensaje = "El IdCliente debe ser mayor a 0";
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método ObtenerClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<ClientesDto> ActualizarClientePorId(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                string? MensajeValidacion = ObjCliente.IdCliente > 0 ? ValidarCliente(ObjCliente) : "El IdCliente debe ser mayor a 0";

                if (MensajeValidacion == null)
                {

                    var reply = _ClientesDAL.ActualizarClientePorId(ObjCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }

                }
                else
                {

                    respuesta.Ok = false;
                    respuesta.Mensaje = MensajeValidacion;
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método ActualizarClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        public Respuesta<ClientesDto> EliminarClientePorId(ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                if (ObjCliente.IdCliente > 0)
                {

                    var reply = _ClientesDAL.EliminarClientePorId(ObjCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }

                }
                else
                {

                    respuesta.Ok = false;
                    respuesta.Mensaje = "El IdCliente debe ser mayor a 0";
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método EliminarClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        // Validaciones de negocio para agregar y actualizar clientes.
        // Quita los espacios de más y devuelve null si todo está bien, o el mensaje del error.
        private string? ValidarCliente(ClientesDto ObjCliente)
        {

            ObjCliente.Cedula = ObjCliente.Cedula?.Trim();
            ObjCliente.Nombre = ObjCliente.Nombre?.Trim();
            ObjCliente.Apellido1 = ObjCliente.Apellido1?.Trim();
            ObjCliente.Apellido2 = string.IsNullOrWhiteSpace(ObjCliente.Apellido2) ? null : ObjCliente.Apellido2.Trim();
            ObjCliente.Telefono = ObjCliente.Telefono?.Trim();
            ObjCliente.Correo = ObjCliente.Correo?.Trim();
            ObjCliente.Direccion = string.IsNullOrWhiteSpace(ObjCliente.Direccion) ? null : ObjCliente.Direccion.Trim();

            if (string.IsNullOrWhiteSpace(ObjCliente.Cedula))
            {
                return "La cédula es requerida, favor revisar!";
            }

            if (ObjCliente.Cedula.Length > 20)
            {
                return "La cédula no puede superar los 20 caracteres";
            }

            if (string.IsNullOrWhiteSpace(ObjCliente.Nombre))
            {
                return "El nombre es requerido, favor revisar!";
            }

            if (ObjCliente.Nombre.Length > 50)
            {
                return "El nombre no puede superar los 50 caracteres";
            }

            if (string.IsNullOrWhiteSpace(ObjCliente.Apellido1))
            {
                return "El primer apellido es requerido, favor revisar!";
            }

            if (ObjCliente.Apellido1.Length > 50 || (ObjCliente.Apellido2 != null && ObjCliente.Apellido2.Length > 50))
            {
                return "Los apellidos no pueden superar los 50 caracteres";
            }

            if (string.IsNullOrWhiteSpace(ObjCliente.Telefono))
            {
                return "El teléfono es requerido, favor revisar!";
            }

            if (ObjCliente.Telefono.Length > 20)
            {
                return "El teléfono no puede superar los 20 caracteres";
            }

            if (string.IsNullOrWhiteSpace(ObjCliente.Correo))
            {
                return "El correo es requerido, favor revisar!";
            }

            if (ObjCliente.Correo.Length > 100 || !Regex.IsMatch(ObjCliente.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return "El correo no tiene un formato válido (ejemplo: nombre@correo.com)";
            }

            if (ObjCliente.Direccion != null && ObjCliente.Direccion.Length > 500)
            {
                return "La dirección no puede superar los 500 caracteres";
            }

            if (ObjCliente.IdProvincia == null || ObjCliente.IdProvincia <= 0)
            {
                return "El IdProvincia es requerido y debe ser mayor a 0 (1 al 7)";
            }

            return null;

        }

    }
}
