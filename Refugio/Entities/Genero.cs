namespace Refugio.Entities
{
    public class Genero
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public List<Mascota> Mascota { get; set; }
    }
}
