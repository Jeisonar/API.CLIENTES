using API.ENTIDADES;
using API.LOGICA.NEGOCIO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.CLIENTES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InicioSesionController : ControllerBase
    {

        //Instancias

        private readonly InicioSesionBLL _InicioSesionBLL;

        public InicioSesionController(InicioSesionBLL inicioSesionBLL)
        {
            _InicioSesionBLL = inicioSesionBLL;

        }


        // POST api/InicioSesion
        [HttpPost]
        public Respuesta<InicioSesionDto> IniciarSesion([FromBody] InicioSesionDto ObjUsuario)
        {

            Respuesta<InicioSesionDto> respuesta = new Respuesta<InicioSesionDto>();

            try
            {

                var reply = _InicioSesionBLL.IniciarSesion(ObjUsuario);

                if (reply != null)
                {

                    respuesta = reply;

                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador InicioSesionController en el método IniciarSesion {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
