using API.ENTIDADES;
using API.LOGICA.NEGOCIO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.CLIENTES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        //Instancias

        private readonly ClientesBLL _ClientesBLL;

        public ClientesController(ClientesBLL clientesBLL)
        {
            _ClientesBLL = clientesBLL;

        }


        // POST api/Clientes
        [HttpPost]
        public Respuesta<ClientesDto> InsertarClientes([FromBody] ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                var reply = _ClientesBLL.InsertarClientes(ObjCliente);

                if (reply != null)
                {

                    respuesta = reply;

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método InsertarClientes {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        // GET api/Clientes
        [HttpGet]
        public Respuesta<List<ClientesDto>> ObtenerClientes()
        {

            Respuesta<List<ClientesDto>> respuesta = new Respuesta<List<ClientesDto>>();

            try
            {

                var reply = _ClientesBLL.ObtenerClientes();

                if (reply != null)
                {

                    respuesta = reply;

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método ObtenerClientes {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        // GET api/Clientes/5
        [HttpGet("{IdCliente:int}")]
        public Respuesta<ClientesDto> ObtenerClientePorId(int IdCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                ClientesDto ObjCliente = new ClientesDto { IdCliente = IdCliente };

                var reply = _ClientesBLL.ObtenerClientePorId(ObjCliente);

                if (reply != null)
                {

                    respuesta = reply;

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método ObtenerClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        // PUT api/Clientes/5
        [HttpPut("{IdCliente:int}")]
        public Respuesta<ClientesDto> ActualizarClientePorId(int IdCliente, [FromBody] ClientesDto ObjCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                // El Id que manda es el de la ruta
                ObjCliente.IdCliente = IdCliente;

                var reply = _ClientesBLL.ActualizarClientePorId(ObjCliente);

                if (reply != null)
                {

                    respuesta = reply;

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método ActualizarClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

        // DELETE api/Clientes/5
        [HttpDelete("{IdCliente:int}")]
        public Respuesta<ClientesDto> EliminarClientePorId(int IdCliente)
        {

            Respuesta<ClientesDto> respuesta = new Respuesta<ClientesDto>();

            try
            {

                ClientesDto ObjCliente = new ClientesDto { IdCliente = IdCliente };

                var reply = _ClientesBLL.EliminarClientePorId(ObjCliente);

                if (reply != null)
                {

                    respuesta = reply;

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método EliminarClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
