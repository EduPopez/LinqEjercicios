using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Employee
    {
        public int Id { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public decimal SalarioAnual { get; set; }

        public bool EsAdministrador { get; set; }

        public int DepartamentoId { get; set; }

        public DateTime FechaNacimiento { get; set;}

        public override string ToString()
        {
            return $"{Nombres} {Apellidos}";
        }

        public override bool Equals(object obj)
        {
            return obj != null &&
                   obj.GetType() == typeof(Employee) &&
                   validacionPropiedades((Employee)obj);
        }

        private bool validacionPropiedades(Employee obj)
            => obj.Id == this.Id &&
            obj.Nombres == this.Nombres &&
            obj.Apellidos == this.Apellidos &&
            obj.DepartamentoId == this.DepartamentoId;

        public override int GetHashCode()
        => HashCode.Combine(Id, Nombres, Apellidos, DepartamentoId);
    }
}
