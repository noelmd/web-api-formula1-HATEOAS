using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationFormula1.Models;

namespace WebApplicationFormula1.Controllers
{
    [Route("api/formula1/monoplazas")]
    public class Formula1Controller : Controller
    {
        private readonly Formula1Context _context;

        public Formula1Controller(Formula1Context context)
        {
            _context = context;

            if (_context.Monoplazas.Count() == 0)
            {
                List<Monoplaza> monolazas = this.CrearMonoplazas();

                foreach (Monoplaza m in monolazas)
                    _context.Monoplazas.Add(m);
                
                _context.SaveChanges();
            }
        }

        // GET api/formula1
        [HttpGet]
        public IEnumerable<Monoplaza> GetAll()
        {
            return _context.Monoplazas.ToList<Monoplaza>();
        }

        // GET api/formula1/5
        [HttpGet("{dorsal}",Name ="GetMonoplaza")]
        public IActionResult GetByDorsal(int dorsal)
        {
            var item = _context.Monoplazas.FirstOrDefault(t => t.Dorsal == dorsal);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                var link = new LinkHelper<Monoplaza>(item);

                link.Links.Add(new Link
                {
                    Href = Url.Link("GetMonoplaza", new { item.Dorsal}),
                    Rel = "self",
                    method = "GET"
                });

                switch (item.Dorsal)
                {
                    case 5:

                        link.Links.Add(new Link
                        {
                            Href = Url.Link("GetPiloto", "Vettel"),
                            Rel = "http://api/formula1/piloto/vettel",
                            method = "GET"
                        });

                        return new ObjectResult(link);
                }
            }

            return new ObjectResult(item);
        }

        // POST api/formula1
        [HttpPost(Name = "CreateMonoplaza")]
        public IActionResult Post([FromBody] Monoplaza value)
        {
            if(value == null)
            {
                return BadRequest();
            }

            _context.Monoplazas.Add(value);
            _context.SaveChanges();

            return CreatedAtRoute("GetMonoplaza", new { dorsal = value.Dorsal }, value);
        }

        // PUT api/formula1/77
        [HttpPut("{dorsal}",Name = "UpdateMonoplaza")]
        public IActionResult Put(int dorsal, [FromBody] Monoplaza value)
        {
            //if (value == null || value.Dorsal != dorsal)
            if (value == null)
            {
                return BadRequest();
            }

            var monoplaza = _context.Monoplazas.FirstOrDefault(t => t.Dorsal == dorsal);

            if (monoplaza == null)
            {
                return NotFound();
            }

            if (value.Dorsal != dorsal)
            {

                _context.Monoplazas.Remove(monoplaza);
                _context.Monoplazas.Add(value);
            }
            else
            {
                monoplaza.Nombre = value.Nombre;
                monoplaza.Potencia = value.Potencia;
                monoplaza.Color = value.Color;

                _context.Monoplazas.Update(monoplaza);
            }

            _context.SaveChanges();

            return new NoContentResult();
        }

        // PATCH api/formula1/14/Negro
        [HttpPatch("{dorsal}", Name = "PatchMonoplaza")]
        public IActionResult Patch(int dorsal, [FromBody] PatchValue value)
        {
            var monoplaza = _context.Monoplazas.FirstOrDefault(t => t.Dorsal == dorsal);

            if (monoplaza == null)
            {
                return NotFound();
            }

            switch (value.Name)
            {
                case "Nombre":
                    monoplaza.Nombre = value.Value;
                break;

                case "Color":
                    monoplaza.Color = value.Value;
                break;

                case "Potencia":

                    int newPotencia = 0;
                    bool response = int.TryParse(value.Value, out newPotencia);

                    monoplaza.Potencia = newPotencia;

                break;

                default:
                    return new NoContentResult();
            }
               
   
            _context.Monoplazas.Update(monoplaza);
            _context.SaveChanges();

            return new NoContentResult();
        }

        // DELETE api/formula1/77
        [HttpDelete("{dorsal}", Name = "DeleteMonoplaza")]
        public IActionResult Delete(int dorsal)
        {
            var monoplaza = _context.Monoplazas.FirstOrDefault(t => t.Dorsal == dorsal);

            if (monoplaza == null)
            {
                return NotFound();
            }

            _context.Monoplazas.Remove(monoplaza);
            _context.SaveChanges();

            return new NoContentResult();
        }

        private List<Monoplaza> CrearMonoplazas()
        {
            List<Monoplaza> monoplazas = new List<Monoplaza>();

            monoplazas.Add(new Monoplaza(5, "Ferrari", "Rojo"));
            monoplazas.Add(new Monoplaza(7, "Ferrari", "Rojo"));
            monoplazas.Add(new Monoplaza(1, "Mercedes", "Plata"));
            monoplazas.Add(new Monoplaza(14, "Mclaren", "Naranja"));

            return monoplazas;
        }
    }
}
