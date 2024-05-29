using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Departamento
    {
        public int Id { get; set; }

        public string NombreLargo { get; set; }

        public string NombreCorto { get; set;}

        public override string ToString()
        {
            return $"{this.Id} - {NombreCorto}";
        }
    }
}
