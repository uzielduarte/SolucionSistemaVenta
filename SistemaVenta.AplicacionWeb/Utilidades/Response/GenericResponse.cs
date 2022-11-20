namespace SistemaVenta.AplicacionWeb.Utilidades.Response
{
    // como respuesta a todas las solicitudes que se hagan a nuestro sitio web
    public class GenericResponse<TObject>
    {
        public bool Estado { get; set; }
        // va a permitir nulos
        public string? Mensaje { get; set; }
        public TObject Objeto { get; set; } 
        public List<TObject>? ListaObjeto { get; set; }
    }
}
