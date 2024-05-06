using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refugio.DTOs;

namespace Refugio.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GeneroController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<List<GeneroDTO>>> GetAll()
        {
            var genero = await context.Generos.ToListAsync();
            if (!genero.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<GeneroDTO>>(genero));
        }
    }
}
