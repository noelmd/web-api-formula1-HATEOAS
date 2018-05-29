using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationFormula1.Models;
using WebApplicationFormula1.Controllers.Helpers;

namespace WebApplicationFormula1.Controllers
{
    [Route("api/formula1/pilotos")]
    public class PilotosController : Controller
    {
        private readonly Formula1Context _context;

        public PilotosController(Formula1Context context)
        {
            _context = context;

            if (_context.Pilotos.Count() == 0)
            {
                List<Piloto> pilotos = this.CrearPilotos();

                foreach (Piloto p in pilotos)
                    _context.Pilotos.Add(p);
                
                _context.SaveChanges();
            }
        }

        private LinkHelper<Piloto> AddHATEOAS (Piloto item, string operacion)
        {
            var link = new LinkHelper<Piloto>(item);

            switch (operacion)
            {
                case "GetPiloto":

                    link.Links.Add(new Link
                    {
                        Href = Url.Link("GetPiloto", new { item.Dorsal }),
                        Rel = "self",
                        method = "GET"
                    });

                    link.Links.Add(new Link
                    {
                        Href = Url.Link("UpdatePiloto", new { item.Dorsal }),
                        Rel = "UpdatePiloto",
                        method = "PUT"
                    });


                    link.Links.Add(new Link
                    {
                        Href = Url.Link("DeletePiloto", new { item.Dorsal }),
                        Rel = "UpdatePiloto",
                        method = "GET"
                    });

                    break;

                case "UpdatePiloto":

                    link.Links.Add(new Link
                    {
                        Href = Url.Link("GetPiloto", new { item.Dorsal }),
                        Rel = "GetPiloto",
                        method = "GET"
                    });

                    link.Links.Add(new Link
                    {
                        Href = Url.Link("UpdatePiloto", new { item.Dorsal }),
                        Rel = "self",
                        method = "PUT"
                    });


                    link.Links.Add(new Link
                    {
                        Href = Url.Link("DeletePiloto", new { item.Dorsal }),
                        Rel = "UpdatePiloto",
                        method = "GET"
                    });

                    break;

                case "DeletePiloto":

                    link.Links.Add(new Link
                    {
                        Href = Url.Link("GetPiloto", new { item.Dorsal }),
                        Rel = "GetPiloto",
                        method = "GET"
                    });

                    link.Links.Add(new Link
                    {
                        Href = Url.Link("UpdatePiloto", new { item.Dorsal }),
                        Rel = "UpdatePiloto",
                        method = "PUT"
                    });


                    link.Links.Add(new Link
                    {
                        Href = Url.Link("DeletePiloto", new { item.Dorsal }),
                        Rel = "self",
                        method = "GET"
                    });

                    break;
            }

            link.Links.Add(new Link
            {
                Href = Url.Link("GetMonoplaza", new { item.Dorsal }), Rel = "GetMonoplaza", method = "GET"
            });

            return link;
        }

        // GET api/formula1/pilotos
        [HttpGet]
        public IEnumerable <LinkHelper<Piloto>> GetAll()
        {
            //return _context.Pilotos.ToList<Piloto>();

            IEnumerable <Piloto> pilotos = _context.Pilotos.ToList<Piloto>();

            List <LinkHelper<Piloto>> items = new List<LinkHelper<Piloto>>(); 

            foreach (Piloto p in pilotos)
            {
                items.Add(AddHATEOAS(p, "GetPiloto"));
            }

            return items.ToList<LinkHelper<Piloto>>();
        }

        // GET api/formula1/piloto/5
        [HttpGet("{dorsal}",Name ="GetPiloto")]
        public IActionResult GetByDorsal(int dorsal)
        {
            var item = _context.Pilotos.FirstOrDefault(t => t.Dorsal == dorsal);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(this.AddHATEOAS(item, "GetPiloto"));
            }
        }

        // POST api/formula1/piloto
        [HttpPost(Name = "CreatePiloto")]
        public IActionResult Post([FromBody] Piloto value)
        {
            if(value == null)
            {
                return BadRequest();
            }

            _context.Pilotos.Add(value);
            _context.SaveChanges();

            return CreatedAtRoute("GetPiloto", new { dorsal = value.Dorsal }, value);
        }

        // PUT api/formula1/77
        [HttpPut("{dorsal}",Name = "UpdatePiloto")]
        public IActionResult Put(int dorsal, [FromBody] Piloto value)
        {
            //if (value == null || value.Dorsal != dorsal)
            if (value == null)
            {
                return BadRequest();
            }

            var piloto = _context.Pilotos.FirstOrDefault(t => t.Dorsal == dorsal);

            if (piloto == null)
            {
                return NotFound();
            }

            if (value.Dorsal != dorsal)
            {

                _context.Pilotos.Remove(piloto);
                _context.Pilotos.Add(value);
            }
            else
            {
                piloto.Nombre = value.Nombre;
                piloto.Apellido = value.Apellido;
                piloto.Pais = value.Pais;

                _context.Pilotos.Update(piloto);
            }

            _context.SaveChanges();

            return new NoContentResult();
        }

        // PATCH api/formula1/14/Negro
        [HttpPatch("{dorsal}", Name = "PatchPiloto")]
        public IActionResult Patch(int dorsal, [FromBody] PatchValue value)
        {
            var piloto = _context.Pilotos.FirstOrDefault(t => t.Dorsal == dorsal);

            if (piloto == null)
            {
                return NotFound();
            }

            switch (value.Name)
            {
                case "Nombre":
                    piloto.Nombre = value.Value;
                break;

                case "Apellido":
                    piloto.Apellido = value.Value;
                break;

                case "Pais":
                    piloto.Pais = value.Value;
                break;

                default:
                    return new NoContentResult();
            }
               
   
            _context.Pilotos.Update(piloto);
            _context.SaveChanges();

            return new NoContentResult();
        }

        // DELETE api/formula1/77
        [HttpDelete("{dorsal}", Name = "DeletePiloto")]
        public IActionResult Delete(int dorsal)
        {
            var piloto = _context.Pilotos.FirstOrDefault(t => t.Dorsal == dorsal);

            if (piloto == null)
            {
                return NotFound();
            }

            _context.Pilotos.Remove(piloto);
            _context.SaveChanges();

            return new NoContentResult();
        }

        private List<Piloto> CrearPilotos()
        {
            List<Piloto> pilotos = new List<Piloto>();

            pilotos.Add(new Piloto(5, "Seb", "Vettel", "Alemania"));
            pilotos.Add(new Piloto(7, "Kimi", "Raikkonen", "Finlandia"));
            pilotos.Add(new Piloto(1, "Lewis","Hamilton", "Gran Bretaña"));
            pilotos.Add(new Piloto(14, "Fernando", "Alonoso", "España"));

            return pilotos;
        }
    }
}
