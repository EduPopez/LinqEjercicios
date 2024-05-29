using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Extensiones
{
    public static class Extension
    {
        public static List<T> Filtrar<T>(this List<T> registros, Func<T, bool> filter)
        {
            List<T> listaFiltrada = new List<T>();

            foreach (T registro in registros)
            {
                if (filter(registro))
                {
                    listaFiltrada.Add(registro);
                }
            }

            return listaFiltrada;
        }
    }
}
