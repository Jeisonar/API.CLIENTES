namespace API.ENTIDADES
{
    public class ClientesDto
    {

        public int IdCliente { get; set; }
        public string? Cedula { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? IdProvincia { get; set; }
        public string? NombreProvincia { get; set; }

    }
}
