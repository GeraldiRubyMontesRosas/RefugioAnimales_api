using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refugio.DTOs;
using Refugio.Entities;

namespace Refugio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TamañosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TamañosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<List<TamañoDTO>>> GetAll()
        {
            var medidas = await context.Tamaños.ToListAsync();
            if (!medidas.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<TamañoDTO>>(medidas));
        }
        [HttpPost("crear")]
        public async Task<ActionResult> Post([FromBody] TamañoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var medidas = mapper.Map<Tamaño>(dto);
            context.Add(medidas);
            try
            {

                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor al guardar el tamaño.", details = ex.Message });
            }
        }
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var medidas = await context.Tamaños.FindAsync(id);
            if (medidas == null)
            {
                return NotFound();
            }
            context.Tamaños.Remove(medidas);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] TamañoDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("El ID de la ruta y el Id del objeto no coinciden");
            }
            var medidas = await context.Tamaños.FindAsync(id);
            if (medidas == null)
            {
                return NotFound();
            }
            mapper.Map(dto, medidas);
            context.Update(medidas);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedidasExists(id))
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
        private bool MedidasExists(int id)
        {
            return context.Tamaños.Any(e => e.Id == id);
        }
    }
}
