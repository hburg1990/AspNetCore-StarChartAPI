using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id, celestialObject });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var storedCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (storedCelestialObject == null)
            {
                return NotFound();
            }

            storedCelestialObject.Name = celestialObject.Name;
            storedCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            storedCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = name;
            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id);

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}