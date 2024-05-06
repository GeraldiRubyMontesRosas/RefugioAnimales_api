using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refugio.DTOs;
using Refugio.Entities;
using Refugio.Services;
using System.ComponentModel;

namespace Refugio.Controllers
{
    [Route("api/mascotas")]
    [ApiController]
    public class MascotasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorImagenes almacenadorImagenes;
        private readonly string directorioMascotas = "mascotas";

        public MascotasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorImagenes almacenadorImagenes)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorImagenes = almacenadorImagenes;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<List<MascotaDTO>>> GetAll()
        {
            var mascotas = await context.Mascotas
                .Include(b => b.Tamaño)
                .Include(b => b.Especie)
                .Include(b => b.Genero)
                .ToListAsync();
            if (!mascotas.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<MascotaDTO>>(mascotas));
        }
        [HttpPost("crear")]
        public async Task<ActionResult> Post(MascotaDTO dto)
        {
            try
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existeMascota = await context.Mascotas.AnyAsync(n => n.Nombre == dto.Nombre);
            if (existeMascota)
            {
                return Conflict();
            }
            if (!string.IsNullOrEmpty(dto.ImagenBase64))
            {
                dto.Foto = await almacenadorImagenes.GuardarImagen(dto.ImagenBase64, directorioMascotas);
            }
            var Mascota = mapper.Map<Mascota>(dto);
                Mascota.Especie = await context.Especies.SingleOrDefaultAsync(s => s.Id == dto.Especie.Id);
                Mascota.Genero = await context.Generos.SingleOrDefaultAsync(s => s.Id == dto.Genero.Id);
                Mascota.Tamaño = await context.Tamaños.SingleOrDefaultAsync(s => s.Id == dto.Tamaño.Id);

                context.Add(Mascota);
            

                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                // Acceder a la excepción interna si está disponible
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "Detalles no disponibles";

                return StatusCode(500, new { error = "Error interno del servidor al guardar mascota.", details = innerExceptionMessage });
            }
        }

        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> Put(int id, MascotaDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("El ID de la ruta y el ID del objeto no coinciden");
            }

            var mascota = await context.Mascotas.FindAsync(id);

            if (mascota == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(dto.ImagenBase64))
            {
                // Guardar la nueva imagen
                mascota.Foto = await almacenadorImagenes.GuardarImagen(dto.ImagenBase64, directorioMascotas);
            }
            else
            {
                dto.Foto = mascota.Foto;
            }

            mascota.Especie = await context.Especies.SingleOrDefaultAsync(s => s.Id == dto.Especie.Id);
            mascota.Genero = await context.Generos.SingleOrDefaultAsync(s => s.Id == dto.Genero.Id);
            mascota.Tamaño = await context.Tamaños.SingleOrDefaultAsync(s => s.Id == dto.Tamaño.Id);
            mascota.Nombre = dto.Nombre;
            mascota.Edad = dto.Edad;
            mascota.Esterilizado = dto.Esterilizado;
            mascota.Color = dto.Color;
            mascota.Vacunas = dto.Vacunas;
            mascota.Descripcion = dto.Descripcion;
            context.Update(mascota);


            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Mascotas.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var mascota = await context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }

            // Obtener la ruta de la imagen asociada a la mascota
            string rutaImagen = mascota.Foto;

            // Verificar si la ruta de la imagen existe y eliminar el archivo de la imagen
            if (!string.IsNullOrEmpty(rutaImagen))
            {
                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }
            }

            // Eliminar la mascota de la base de datos
            context.Mascotas.Remove(mascota);
            await context.SaveChangesAsync();

            return NoContent();
        }






    }
}
