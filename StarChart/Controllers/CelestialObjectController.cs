using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = 
                _context.CelestialObjects
                    .Where(c => c.OrbitedObjectId == celestialObject.Id)
                    .ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Name == name);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = 
                _context.CelestialObjects
                    .Where(c => c.OrbitedObjectId == celestialObject.Id)
                    .ToList();

            return Ok(celestialObject);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            foreach (var celestialObject in _context.CelestialObjects)
            {
                celestialObject.Satellites = 
                _context.CelestialObjects
                    .Where(c => c.OrbitedObjectId == celestialObject.Id)
                    .ToList();
            }

            return Ok(_context.CelestialObjects);
        }
    }
}