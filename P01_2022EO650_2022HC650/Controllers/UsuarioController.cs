using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022EO650_2022HC650.Models;

namespace P01_2022EO650_2022HC650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly parqueoContext _parqueoContext;
        public UsuarioController(parqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }

        [HttpPost]
        [Route("RegisterUsuario")]
        public IActionResult Register([FromBody] Usuarios usuario)
        {
            try
            {
                _parqueoContext.Usuarios.Add(usuario);
                _parqueoContext.SaveChanges();
                return Ok(new { message = "Usuario registrado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _parqueoContext.Usuarios
                .FirstOrDefault(u => u.Correo == request.Correo && u.Contrasena == request.Contrasena);

            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            return Ok(new { message = "Inicio de sesión exitoso." });
        }

        [HttpGet]
        [Route("GetAllUsuarios")]
        public IActionResult GetUsuarios()
        {
            var usuarios = _parqueoContext.Usuarios
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.Correo,
                    u.Telefono,
                    u.Rol
                }).ToList();

            if (!usuarios.Any())
            {
                return NotFound("No hay usuarios registrados.");
            }
            return Ok(usuarios);
        }

        [HttpGet]
        [Route("GetUsuarioById/{id}")]
        public IActionResult GetUsuarioById(int id)
        {
            var usuario = _parqueoContext.Usuarios
                .Where(u => u.IdUsuario == id)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.Correo,
                    u.Telefono,
                    u.Rol
                }).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"No se encontró un Usuarios con ID {id}.");
            }
            return Ok(usuario);
        }

        [HttpPut]
        [Route("UpdateUsuario/{id}")]
        public IActionResult UpdateUsuario(int id, [FromBody] Usuarios usuarioModificar)
        {
            var usuarioActual = _parqueoContext.Usuarios.Find(id);
            if (usuarioActual == null)
            {
                return NotFound($"No se encontró un Usuarios con ID {id}.");
            }

            usuarioActual.Nombre = usuarioModificar.Nombre;
            usuarioActual.Correo = usuarioModificar.Correo;
            usuarioActual.Telefono = usuarioModificar.Telefono;
            usuarioActual.Rol = usuarioModificar.Rol;

            if (!string.IsNullOrEmpty(usuarioModificar.Contrasena))
            {
                usuarioActual.Contrasena = usuarioModificar.Contrasena;
            }

            _parqueoContext.Entry(usuarioActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();

            return Ok(new { message = "Usuario actualizado correctamente." });
        }

        [HttpDelete]
        [Route("DeleteUsuario/{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuario = _parqueoContext.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound($"No se encontró un Usuarios con ID {id}.");
            }

            _parqueoContext.Usuarios.Remove(usuario);
            _parqueoContext.SaveChanges();

            return Ok($"Usuario con ID {id} eliminado exitosamente.");
        }
    }
    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}
