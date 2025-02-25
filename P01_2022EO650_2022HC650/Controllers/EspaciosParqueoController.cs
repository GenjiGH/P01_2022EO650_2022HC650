using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022EO650_2022HC650.Models;

namespace P01_2022EO650_2022HC650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspaciosParqueoController : ControllerBase
    {
        private readonly parqueoContext _parqueoContext;
        public EspaciosParqueoController(parqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }

        [HttpGet]
        [Route("GetAllSucursales")]
        public IActionResult GetSucursales()
        {
            var sucursales = _parqueoContext.Sucursales
                .Include(s => s.Administrador)
                .Select(s => new
                {
                    s.IdSucursal,
                    s.Nombre,
                    s.Direccion,
                    s.Telefono,
                    Administrador = s.Administrador.Nombre,
                    s.NumEspaciosDisponibles
                }).ToList();

            if (!sucursales.Any())
            {
                return NotFound("No hay sucursales registradas.");
            }
            return Ok(sucursales);
        }

        [HttpGet]
        [Route("GetSucursalById/{id}")]
        public IActionResult GetSucursalById(int id)
        {
            var sucursal = _parqueoContext.Sucursales
                .Include(s => s.Administrador)
                .Where(s => s.IdSucursal == id)
                .Select(s => new
                {
                    s.IdSucursal,
                    s.Nombre,
                    s.Direccion,
                    s.Telefono,
                    Administrador = s.Administrador.Nombre,
                    s.NumEspaciosDisponibles
                }).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound($"No se encontró una Sucursales con ID {id}.");
            }
            return Ok(sucursal);
        }

        [HttpPost]
        [Route("AddSucursal")]
        public IActionResult AddSucursal([FromBody] Sucursales sucursal)
        {
            try
            {
                _parqueoContext.Sucursales.Add(sucursal);
                _parqueoContext.SaveChanges();
                return Ok(sucursal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSucursal/{id}")]
        public IActionResult UpdateSucursal(int id, [FromBody] Sucursales sucursalModificar)
        {
            var sucursalActual = _parqueoContext.Sucursales.Find(id);
            if (sucursalActual == null)
            {
                return NotFound($"No se encontró una Sucursales con ID {id}.");
            }

            sucursalActual.Nombre = sucursalModificar.Nombre;
            sucursalActual.Direccion = sucursalModificar.Direccion;
            sucursalActual.Telefono = sucursalModificar.Telefono;
            sucursalActual.IdAdministrador = sucursalModificar.IdAdministrador;
            sucursalActual.NumEspaciosDisponibles = sucursalModificar.NumEspaciosDisponibles;

            _parqueoContext.Entry(sucursalActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();

            return Ok(sucursalModificar);
        }

        [HttpDelete]
        [Route("DeleteSucursal/{id}")]
        public IActionResult DeleteSucursal(int id)
        {
            var sucursal = _parqueoContext.Sucursales.Find(id);
            if (sucursal == null)
            {
                return NotFound($"No se encontró una Sucursales con ID {id}.");
            }

            _parqueoContext.Sucursales.Remove(sucursal);
            _parqueoContext.SaveChanges();

            return Ok($"Sucursal con ID {id} eliminada exitosamente.");
        }

        [HttpGet]
        [Route("GetAvailableSpaces")]
        public IActionResult GetAvailableSpaces()
        {
            var espacios = _parqueoContext.EspaciosParqueo
                .Include(e => e.Sucursal)
                .Where(e => e.Estado == "Disponible")
                .Select(e => new
                {
                    e.IdEspacio,
                    e.NumeroEspacio,
                    e.Ubicacion,
                    e.CostoPorHora,
                    e.Estado,
                    Sucursal = e.Sucursal.Nombre
                }).ToList();

            if (!espacios.Any())
            {
                return NotFound("No hay espacios de parqueo disponibles.");
            }
            return Ok(espacios);
        }

        [HttpPost]
        [Route("AddEspacio")]
        public IActionResult AddEspacio([FromBody] EspaciosParqueo espacio)
        {
            try
            {
                _parqueoContext.EspaciosParqueo.Add(espacio);
                _parqueoContext.SaveChanges();
                return Ok(espacio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEspacioById/{id}")]
        public IActionResult GetEspacioById(int id)
        {
            var espacio = _parqueoContext.EspaciosParqueo
                .Include(e => e.Sucursal)
                .Where(e => e.IdEspacio == id)
                .Select(e => new
                {
                    e.IdEspacio,
                    e.NumeroEspacio,
                    e.Ubicacion,
                    e.CostoPorHora,
                    e.Estado,
                    Sucursal = e.Sucursal.Nombre
                }).FirstOrDefault();

            if (espacio == null)
            {
                return NotFound($"No se encontró un espacio con ID {id}.");
            }
            return Ok(espacio);
        }

        [HttpPut]
        [Route("UpdateEspacio/{id}")]
        public IActionResult UpdateEspacio(int id, [FromBody] EspaciosParqueo espacioModificar)
        {
            var espacioActual = _parqueoContext.EspaciosParqueo.Find(id);
            if (espacioActual == null)
            {
                return NotFound($"No se encontró un espacio de parqueo con ID {id}.");
            }

            espacioActual.NumeroEspacio = espacioModificar.NumeroEspacio;
            espacioActual.Ubicacion = espacioModificar.Ubicacion;
            espacioActual.CostoPorHora = espacioModificar.CostoPorHora;
            espacioActual.Estado = espacioModificar.Estado;

            _parqueoContext.Entry(espacioActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();

            return Ok(espacioModificar);
        }

        [HttpDelete]
        [Route("DeleteEspacio/{id}")]
        public IActionResult DeleteEspacio(int id)
        {
            var espacio = _parqueoContext.EspaciosParqueo.Find(id);
            if (espacio == null)
            {
                return NotFound($"No se encontró un espacio de parqueo con ID {id}.");
            }

            _parqueoContext.EspaciosParqueo.Remove(espacio);
            _parqueoContext.SaveChanges();

            return Ok($"Espacio de parqueo con ID {id} eliminado exitosamente.");
        }
    }
}
