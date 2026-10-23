using API.DAPPER;
using API.ENTIDADES;

namespace API.LOGICA.NEGOCIO
{
    public class InicioSesionBLL
    {

        //Instancia

        private readonly InicioSesionDAL _InicioSesionDAL;


        //Constructor

        public InicioSesionBLL(InicioSesionDAL inicioSesionDAL)
        {

            _InicioSesionDAL = inicioSesionDAL;

        }


        public Respuesta<InicioSesionDto> IniciarSesion(InicioSesionDto ObjUsuario)
        {

            Respuesta<InicioSesionDto> respuesta = new Respuesta<InicioSesionDto>();

            try
            {

                if (!string.IsNullOrWhiteSpace(ObjUsuario.Correo) && !string.IsNullOrWhiteSpace(ObjUsuario.Contrasena))
                {

                    ObjUsuario.Correo = ObjUsuario.Correo.Trim();

                    var reply = _InicioSesionDAL.IniciarSesion(ObjUsuario);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }

                }
                else
                {

                    respuesta.Ok = false;
                    respuesta.Mensaje = "Debe ingresar el correo y la contraseña";
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {

                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método IniciarSesion {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
