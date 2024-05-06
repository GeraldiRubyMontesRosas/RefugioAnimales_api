using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refugio.DTOs;
using Refugio.Entities;
using System.Text.RegularExpressions;

namespace Refugio.Controllers
{
    [Route("api/especies")]
    [ApiController]
    public class EspeciesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EspeciesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<List<EspecieDTO>>> GetAll()
        {
            var especies = await context.Especies.ToListAsync();
            if (!especies.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<EspecieDTO>>(especies));
        }
        [HttpPost("crear")]
        public async Task<ActionResult> Post([FromBody] EspecieDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var especies = mapper.Map<Especie>(dto);
            context.Add(especies);
            try
            {

                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor al guardar al especies.", details = ex.Message });
            }
        }
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var especie = await context.Especies.FindAsync(id);
            if (especie == null)
            {
                return NotFound();
            }
            context.Especies.Remove(especie);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] EspecieDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("El ID de la ruta y el Id del objeto no coinciden");
            }
            var especies = await context.Especies.FindAsync(id);
            if (especies == null)
            {
                return NotFound();
            }
            mapper.Map(dto, especies);
            context.Update(especies);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecieExists(id))
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
        private bool EspecieExists(int id)
        {
            return context.Especies.Any(e => e.Id == id);
        }
    }
}
