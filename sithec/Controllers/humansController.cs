using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sithec.Models;

namespace sithec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class humansController : ControllerBase
    {
        private readonly HumanContext _context;        

        public humansController(HumanContext context)
        {
            _context = context;
        }

        [HttpGet("mock")]
        public IActionResult GetMockHumanos()
        {
            // Aquí puedes generar datos de ejemplo
            var testInfo = new List<humano>
        {
            new humano { Id = 1, Nombre = "Persona 1", Sexo = "M", Edad = 30, Altura = 175, Peso = 70 },
            new humano { Id = 2, Nombre = "Persona 2", Sexo = "F", Edad = 25, Altura = 160, Peso = 55 }
        };

            return Ok(testInfo);
        }

        [HttpPost("operacion")]
        public IActionResult RealizarOperacion(string Operador, long Numero1, long Numero2)
        {
            double resultado = 0;

            switch (Operador)
            {
                case "+":
                    resultado = Numero1 + Numero2;
                    break;
                case "-":
                    resultado = Numero1 - Numero2;
                    break;
                case "*":
                    resultado = Numero1 * Numero2;
                    break;
                case "/":
                    if (Numero2 == 0) return BadRequest("Error No se puede dividir entre 0");
                    resultado = (double)Numero1 / Numero2;
                    break;
                default:
                    return BadRequest("Operador no válido");
            }

            return Ok(resultado);
        }

        // GET: api/humans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<humano>>> Gethumanos()
        {
          if (_context.humanos == null)
          {
              return NotFound();
          }
            return await _context.humanos.ToListAsync();
        }

        // GET: api/humans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<humano>> Gethumano(int id)
        {
          if (_context.humanos == null)
          {
              return NotFound();
          }
            var humano = await _context.humanos.FindAsync(id);

            if (humano == null)
            {
                return NotFound();
            }

            return humano;
        }

        // PUT: api/humans/5
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/humans
        ///     {
        ///        "id": 1,
        ///        "name": "Nombre"
        ///     }
        ///
        /// </remarks>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> Puthumano(humano Humano)
        {
            var oldHuman = _context.humanos.FirstOrDefault(x => x.Id == Humano.Id);
            if (oldHuman == null)
            {
                return NotFound();
            }         
            
            //Simple validation
            oldHuman.Nombre = string.IsNullOrEmpty(Humano.Nombre) ? oldHuman.Nombre : Humano.Nombre;
            oldHuman.Sexo = string.IsNullOrEmpty(Humano.Sexo) ? oldHuman.Sexo : Humano.Sexo;
            oldHuman.Edad = (Humano.Edad == 0) ? oldHuman.Edad : Humano.Edad;
            oldHuman.Altura = (Humano.Altura == 0) ? oldHuman.Altura : Humano.Altura;
            oldHuman.Peso = (Humano.Peso == 0) ? oldHuman.Peso : Humano.Peso;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!humanoExists(Humano.Id))
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

        // POST: api/humans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<humano>> Posthumano(humano humano)
        {
          if (_context.humanos == null)
          {
              return Problem("Entity set 'HumanContext.humanos'  is null.");
          }
            _context.humanos.Add(humano);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gethumano", new { id = humano.Id }, humano);
        }       

        private bool humanoExists(int id)
        {
            return (_context.humanos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
    } 

}
