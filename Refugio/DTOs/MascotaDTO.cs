namespace Refugio.DTOs
{
    public class MascotaDTO
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public EspecieDTO Especie { get; set; }
        public GeneroDTO Genero { get; set; }
        public string Edad { get; set; }
        public bool Esterilizado { get; set; }
        public TamañoDTO Tamaño { get; set; }
        public string Color { get; set; }
        public string Foto { get; set; }
        public string ImagenBase64 { get; set; }
        public string Descripcion { get; set; }
        public bool Vacunas { get; set; }
    }
}
