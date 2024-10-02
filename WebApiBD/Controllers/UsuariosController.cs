using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBD.Models;

namespace WebApiBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioContext _context;
        public UsuariosController(UsuarioContext context)
        {
            _context = context;
        }

        //GET :api/Usuario/listar
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<Usuario>>> Listar()
        {
            return await _context.Usuarios.ToListAsync();
        }

        //GET: api/Usuario/buscarporid/5
        [HttpGet("buscarporid")]
        public async Task<ActionResult<Usuario>> BuscarPorID(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        //POST: api/Usuario/Registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<Usuario>> Registrar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorID), new { id = usuario.Id }, usuario);
        }

        //PUT: api/Usuario/actualizar/5
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Actualizar(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest(new { message = "El ID del cliente no coincide con el de la URL." });
            }
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound(new { message = "Cliente no encontrado" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        //DELETE: api/Usuario/eliminar/5
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool UsuarioExists(int id)
        { return _context.Usuarios.Any(equals => equals.Id == id); }
    }
}