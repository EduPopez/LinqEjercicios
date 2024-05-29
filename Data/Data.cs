using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public static class Data
    {
        public static List<Employee> ObtenerEmpleados()
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    Nombres = "Eduardo",
                    Apellidos = "Lopez Zuniga",
                    SalarioAnual = 80000m,
                    EsAdministrador = true,
                    DepartamentoId = 1,
                    FechaNacimiento = new DateTime(1984,6,14),
                },

                new Employee()
                {
                    Id = 2,
                    Nombres = "Griselda Jimena",
                    Apellidos = "Castro Pereira",
                    SalarioAnual = 40000.05m,
                    EsAdministrador = false,
                    DepartamentoId = 2,
                    FechaNacimiento = new DateTime(1980,9,2),
                },

                new Employee()
                {
                    Id = 3,
                    Nombres = "Daniel Arturo",
                    Apellidos = "Llano Flores",
                    SalarioAnual = 100000m,
                    EsAdministrador = true,
                    DepartamentoId = 1,
                    FechaNacimiento= new DateTime(1983, 9, 16),
                },

                new Employee()
                {
                    Id = 4,
                    Nombres = "Jhessitd Adrian",
                    Apellidos = "Lopez Castro",
                    SalarioAnual = 1000m,
                    EsAdministrador = false,
                    DepartamentoId = 2,
                    FechaNacimiento = new DateTime(2005, 12, 21),
                },

                new Employee()
                {
                    Id = 5,
                    Nombres = "Carolina",
                    Apellidos = "Lopez Zuniga",
                    SalarioAnual = 70000m,
                    EsAdministrador = false,
                    DepartamentoId = 3,
                    FechaNacimiento = new DateTime(1988,10,20),
                }
            };

            return employees;
        }

        public static List<Departamento> ObtenerDepartamentos()
        {
            List<Departamento> departamentos = new List<Departamento>()
            {
                new Departamento { Id = 1, NombreCorto = "SIS", NombreLargo = "Sistemas" },
                new Departamento { Id = 2, NombreCorto = "HG", NombreLargo = "Hogar" },
                new Departamento { Id = 3, NombreCorto = "CBN", NombreLargo = "Cervezas" },
            };

            return departamentos;
        }

        public static ArrayList DatosCualquiera()
        {
            ArrayList list = new ArrayList();
            list.Add(new DateTime(1984, 6, 14));
            list.Add(123456);
            list.Add(654321);
            list.Add("Eduardo");
            list.Add("Mandarina");
            list.Add(new Departamento { Id = 1, NombreCorto = "SIS", NombreLargo = "Sistemas" });
            list.Add(new Departamento { Id = 3, NombreCorto = "CBN", NombreLargo = "Cervezas" });
            list.Add(new Employee()
            {
                Id = 2,
                Nombres = "Griselda Jimena",
                Apellidos = "Castro Pereira",
                SalarioAnual = 40000.05m,
                EsAdministrador = false,
                DepartamentoId = 2,
                FechaNacimiento = new DateTime(1980, 9, 2),
            });
            return list;
        }
    }
}
