using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFormula1.Models
{
    public class PatchValue 
    {
        private string value;
        private string name;

        public string Value { get => value; set => this.value = value; }
        public string Name { get => name; set => name = value; }
    }
}
