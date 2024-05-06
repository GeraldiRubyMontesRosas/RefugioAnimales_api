using Refugio.DTOs;

namespace Refugio.Entities
{
    public class Mascota
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public Especie Especie { get; set; }
        public Genero Genero { get; set; }
        public string Edad { get; set; }
        public bool Esterilizado { get; set; }
        public Tamaño Tamaño { get; set; }
        public string Color { get; set; }
        public string Foto { get; set; }
        public string Descripcion { get; set; }
        public bool Vacunas { get; set; }
    }
}
