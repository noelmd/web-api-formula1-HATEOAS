using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationFormula1.Models
{
    
    public class Piloto
    {
        private int dorsal;
        private string nombre;
        private string apellido;
        private string pais;

        public Piloto()
        {

        }

        public Piloto(int dorsal, string nombre, string apellido, string pais)
        {
            this.dorsal = dorsal;
            this.nombre = nombre;
            this.apellido = apellido;
            this.pais = pais;
        }

        [Key]
        public int Dorsal { get => dorsal; set => dorsal = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Pais { get => pais; set => pais = value; }
    }
}
