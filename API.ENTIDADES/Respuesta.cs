namespace API.ENTIDADES
{
    public class Respuesta<T>
    {

        public bool Ok { get; set; }
        public string? Mensaje { get; set; }
        public T? ValorRetorno { get; set; }

    }
}
