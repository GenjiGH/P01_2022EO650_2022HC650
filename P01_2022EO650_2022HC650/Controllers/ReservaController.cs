using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022EO650_2022HC650.Models;

namespace P01_2022EO650_2022HC650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly parqueoContext _parqueoContext;
        public ReservaController(parqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }

        [HttpPost("ReservarEspacio")]
        public IActionResult ReservarEspacio([FromBody] Reservas nuevaReserva)
        {
            var espacio = _parqueoContext.EspaciosParqueo
                .FirstOrDefault(e => e.IdEspacio == nuevaReserva.IdEspacio && e.Estado == "Disponible");

            if (espacio == null)
            {
                return BadRequest("El espacio de parqueo no está disponible.");
            }

            nuevaReserva.Estado = "Activa";
            _parqueoContext.Reservas.Add(nuevaReserva);
            espacio.Estado = "Ocupado"; 
            _parqueoContext.SaveChanges();

            return Ok(nuevaReserva);
        }

        [HttpGet("GetReservasActivasPorUsuario/{idUsuario}")]
        public IActionResult GetReservasActivasPorUsuario(int idUsuario)
        {
            var reservas = _parqueoContext.Reservas
                .Where(r => r.IdUsuario == idUsuario && r.Estado == "Activa")
                .Join(_parqueoContext.EspaciosParqueo, r => r.IdEspacio, e => e.IdEspacio, (r, e) => new
                {
                    r.IdReserva,
                    r.FechaReserva,
                    r.HoraReserva,
                    r.CantidadHoras,
                    Espacio = new
                    {
                        e.NumeroEspacio,
                        e.Ubicacion,
                        e.CostoPorHora,
                        Sucursal = e.Sucursal.Nombre
                    }
                })
                .ToList();

            if (reservas.Count == 0)
            {
                return NotFound("No hay reservas activas para este Usuarios.");
            }

            return Ok(reservas);
        }

        [HttpGet("GetReservasPorSucursalYFecha/{idSucursal}/{fecha}")]
        public IActionResult GetReservasPorSucursalYFecha(int idSucursal, DateTime fecha)
        {
            var reservas = _parqueoContext.Reservas
                .Where(r => r.FechaReserva == fecha)
                .Join(_parqueoContext.EspaciosParqueo, r => r.IdEspacio, e => e.IdEspacio, (r, e) => new
                {
                    r.IdReserva,
                    r.FechaReserva,
                    r.HoraReserva,
                    r.CantidadHoras,
                    Usuario = new
                    {
                        r.Usuario.Nombre,
                        r.Usuario.Correo,
                        r.Usuario.Telefono
                    },
                    Espacio = new
                    {
                        e.NumeroEspacio,
                        e.Ubicacion,
                        e.CostoPorHora,
                        e.IdSucursal
                    }
                })
                .Where(r => r.Espacio.IdSucursal == idSucursal)
                .ToList();

            if (reservas.Count == 0)
            {
                return NotFound("No hay reservas para esta Sucursales en la fecha especificada.");
            }

            return Ok(reservas);
        }

        [HttpGet("GetReservasPorRangoFechas/{idSucursal}/{fechaInicio}/{fechaFin}")]
        public IActionResult GetReservasPorRangoFechas(int idSucursal, DateTime fechaInicio, DateTime fechaFin)
        {
            var reservas = _parqueoContext.Reservas
                .Where(r => r.FechaReserva >= fechaInicio && r.FechaReserva <= fechaFin)
                .Join(_parqueoContext.EspaciosParqueo, r => r.IdEspacio, e => e.IdEspacio, (r, e) => new
                {
                    r.IdReserva,
                    r.FechaReserva,
                    r.HoraReserva,
                    r.CantidadHoras,
                    Usuario = new
                    {
                        r.Usuario.Nombre,
                        r.Usuario.Correo,
                        r.Usuario.Telefono
                    },
                    Espacio = new
                    {
                        e.NumeroEspacio,
                        e.Ubicacion,
                        e.CostoPorHora,
                        e.IdSucursal
                    }
                })
                .Where(r => r.Espacio.IdSucursal == idSucursal)
                .ToList();

            if (reservas.Count == 0)
            {
                return NotFound("No hay reservas en el rango de fechas especificado.");
            }

            return Ok(reservas);
        }

        [HttpDelete("CancelarReserva/{id}")]
        public IActionResult CancelarReserva(int id)
        {
            var reserva = _parqueoContext.Reservas.Find(id);
            if (reserva == null)
            {
                return NotFound("No se encontró la Reservas.");
            }

            reserva.Estado = "Cancelada";
            var espacio = _parqueoContext.EspaciosParqueo.Find(reserva.IdEspacio);
            if (espacio != null)
            {
                espacio.Estado = "Disponible";
            }

            _parqueoContext.SaveChanges();
            return Ok("Reserva cancelada exitosamente.");
        }

    }
}
