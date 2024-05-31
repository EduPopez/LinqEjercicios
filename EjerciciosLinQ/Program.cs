// See https://aka.ms/new-console-template for more information
using Data;
using Extensiones;
using System.Runtime.Intrinsics.Arm;

// Primera Parte
// Aqui observamos como el metodo filtrar ser acopla a las listas directamente (esto se da por static)

//List<Employee> employeeList = Data.Data.ObtenerEmpleados();

//var filteredEmployees = employeeList.Filtrar(e => e.EsAdministrador == true);

//foreach (var employee in filteredEmployees)
//{
//    Console.WriteLine($"El nombre del empleado es {employee.Nombres} {employee.Apellidos}");
//}

//Console.WriteLine();

//List<Departamento> departamentsList = Data.Data.ObtenerDepartamentos();

//var filteredDepartments = departamentsList.Where(d => d.Id < 3);

//foreach (var department in filteredDepartments)
//{
//    Console.WriteLine($"El nombre del departamento es { department.NombreLargo }, Id { department.Id}.");
//}

// Segunda Parte: Hagamos Joins Linq Syntax
// Ojo con estas diferencias Deferred (takes a Lazy evaluation) Vs Immediate Query Execution (takes stric evaluation) in LINQ

// Aplicamos Linq Query que devuelve un tipo anonimo (Anonymous Type)
List<Employee> employeesList = Data.Data.ObtenerEmpleados();
List<Departamento> departamentsList = Data.Data.ObtenerDepartamentos();

var result = from emp in employeesList
             join dep in departamentsList
             on emp.DepartamentoId equals dep.Id
             where emp.SalarioAnual >= 50000m
             select new
             {
                 EmpId = emp.Id,
                 EmployeeName = emp.ToString(),
                 Salario = emp.SalarioAnual,
                 EsAdministrador = emp.EsAdministrador,
                 DepId = emp.DepartamentoId,
                 DepartamentoNombre = dep.NombreLargo,
             };

if (result.Any())
{
    foreach (var item in result)
    {
        Console.WriteLine($"Empleado: {item.EmpId} - {item.EmployeeName}.");
        Console.WriteLine($"\tDepartamento: {item.DepId} - {item.DepartamentoNombre} -> Sueldo {item.Salario.ToString("C")}");
        Console.WriteLine();
    }

    var promedioSueldo = result.Average(r => r.Salario);
    var sueldoMayor = result.Max(r => r.Salario);
    var sueldoMenor = result.Min(r => r.Salario);

    Console.WriteLine($"Promedio Sueldo: {promedioSueldo.ToString("C")}, sueldo mayor: {sueldoMayor}, sueldo menor: {sueldoMenor}.");
}

Console.WriteLine();

// Queries

// Usando Metodos (Method Syntax)
employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

var resultDto01 = employeesList.Select(emp => new {
    EmpId = emp.Id,
    EmpleadoNombre = emp.Nombres + " " + emp.Apellidos,
    SueldoAnual = emp.SalarioAnual,
}).Where(r => r.SueldoAnual >= 50000);

foreach (var item in resultDto01)
{
    Console.WriteLine($"{item.EmpleadoNombre} {item.SueldoAnual.ToString("C")}");
}

Console.WriteLine();
Console.WriteLine();

// Method Syntax para Unir dos tablas Join
Console.WriteLine("Inner Join");
Console.WriteLine("==========");
employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

departamentsList.Clear();
departamentsList = Data.Data.ObtenerDepartamentos();

var resultDto02 = employeesList.Join(departamentsList,
    emp => emp.DepartamentoId,
    dep => dep.Id,
    (e, d) => new
    {
        NombreCompleto = e.ToString(),
        Salario = e.SalarioAnual,
        Oficina = d.NombreLargo,
        Sigla = d.NombreCorto,
    }).Where(r => r.Salario >= 75000);

employeesList.Add(
    new Employee() { Id = 7, Nombres = "El Gado", Apellidos = "Villena", DepartamentoId = 3, SalarioAnual = 95000, });

foreach (var item in resultDto02)
{
    Console.WriteLine($"{item.NombreCompleto} {item.Salario.ToString("C")}, {item.Sigla} - {item.Oficina}.");
}

Console.WriteLine();
Console.WriteLine();

// Left Outer Join

Console.WriteLine("Left Join => (GroupBy, ToLookUp");
Console.WriteLine("===============================");

employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

departamentsList.Clear();
departamentsList = Data.Data.ObtenerDepartamentos();

departamentsList.Add(new Departamento
{
    Id = 9,
    NombreCorto = "COC",
    NombreLargo = "Cocina",
});

var resulDto03 = departamentsList.GroupJoin(employeesList,
    dept => dept.Id,
    emp => emp.DepartamentoId,
    (d, eG) => new
    {
        Empleados = eG,
        Departamento = d.NombreLargo,
    });

foreach (var item in resulDto03)
{
    Console.WriteLine($"Nombre del departamento: {item.Departamento}.");
    foreach (var emp in item.Empleados)
    {
        Console.WriteLine($"\t{emp.ToString()}");
    }
}

Console.WriteLine();


Console.WriteLine("Left Join Query Syntax");
Console.WriteLine("======================");

employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

departamentsList.Clear();
departamentsList = Data.Data.ObtenerDepartamentos();


departamentsList.Add(new Departamento
{
    Id = 9,
    NombreCorto = "COC",
    NombreLargo = "Cocina",
});

var resultDto04 = from dep in departamentsList
                  join emp in employeesList
                  on dep.Id equals emp.DepartamentoId
                  //al usar into creamos un grupo de empleados por cada departamento
                  into empleados
                  select new
                  {
                      Empleados = empleados,
                      Departamento = dep.NombreLargo,
                  };

foreach (var item in resultDto04)
{
    Console.WriteLine($"Nombre del departamento: {item.Departamento}.");
    foreach (var emp in item.Empleados)
    {
        Console.WriteLine($"\t{emp.ToString()}");
    }
}

Console.WriteLine();

Console.WriteLine("Left Join Query Syntax pero desde empleado");
Console.WriteLine("==========================================");

employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

employeesList.Add(new Employee() {
    Apellidos = "Cruz",
    DepartamentoId = 10,
    EsAdministrador = false,
    FechaNacimiento = new DateTime(1990,1,1), 
    Id = 15,
    Nombres = "Juan",
    SalarioAnual = 1500m, });

departamentsList.Clear();
departamentsList = Data.Data.ObtenerDepartamentos();

var resultDto05 = from emp in employeesList
                  join dep in departamentsList
                  on emp.DepartamentoId equals dep.Id
                  into departamentos
                  select new
                  {
                      Empleado = emp.ToString(),
                      Departamentos = departamentos,
                  };

foreach (var item in resultDto05)
{
    Console.WriteLine($"Nombre del Empleado: {item.Empleado}.");
    foreach (var dep in item.Departamentos)
    {
        Console.WriteLine($"\t{dep.NombreLargo}");
    }
}

Console.WriteLine();

//Operadores
//==========

//Sorting Operators
//  OrderBy
//  OrderByDescending
//  ThenBy
//  ThenByDescending

//Grouping Operators
//  GroupBy
//  ToLookup

//Quantifier Operators
//  All
//  Any
//  Contains

//Filter Operators
//  OfType
//  Where

//Element Operators
//  ElementAt
//  ElementAtOrDefault
//  First
//  FirstOrDefault
//  Last
//  LastOrDefault
//  Single
//  SingleOrDefault

Console.WriteLine("Sorting Operators Method");

employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

employeesList.Add(
    new Employee() { Id = 7, Nombres = "Eduardo", Apellidos = "Villena", DepartamentoId = 3, SalarioAnual = 95000, });

departamentsList.Clear();
departamentsList = Data.Data.ObtenerDepartamentos();

var resultDto06 = employeesList.Join(departamentsList,
    emp => emp.DepartamentoId,
    dep => dep.Id,
    (e, d) => new
    {
        Id = e.Id,
        Nombres = e.Nombres,
        Apellidos = e.Apellidos,
        Salario = e.SalarioAnual,
        Oficina = d.NombreLargo,
        Sigla = d.NombreCorto,
    })
    .OrderBy(e => e.Nombres).ThenByDescending(e => e.Apellidos);

foreach (var item in resultDto06)
{
    Console.WriteLine($"{item.Id, -5} {item.Nombres, -10} {item.Apellidos,-10} - {item.Salario.ToString("C"), -10}");
}

Console.WriteLine();
Console.WriteLine("Sorting Operators Query");

employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

employeesList.Add(
    new Employee() { Id = 7, Nombres = "Eduardo", Apellidos = "Villena", DepartamentoId = 3, SalarioAnual = 95000, });

departamentsList.Clear();
departamentsList = Data.Data.ObtenerDepartamentos();

var resultDto07 = from emp in employeesList
                  join dep in departamentsList
                  on emp.DepartamentoId equals dep.Id
                  orderby emp.Nombres, emp.Apellidos descending
                  select new
                  {
                      Id = emp.Id,
                      Nombres = emp.Nombres,
                      Apellidos = emp.Apellidos,
                      Salario = emp.SalarioAnual,
                      Oficina = dep.NombreLargo,
                      Sigla = dep.NombreCorto,
                  };

foreach (var item in resultDto07)
{
    Console.WriteLine($"{item.Id,-5} {item.Nombres,-10} {item.Apellidos,-10} - {item.Salario.ToString("C"),-10}");
}

Console.WriteLine("");

Console.WriteLine("Grouping Operators");
Console.WriteLine("==================");

var groupResultQuery = from emp in employeesList
                  group emp by emp.DepartamentoId;

var groupResultMethod = employeesList.GroupBy(emp => emp.DepartamentoId);

foreach (var groupResult in groupResultQuery)
{
    Console.WriteLine($"Departamento Id: {groupResult.Key}");
    foreach (var item in groupResult)
    {
        Console.WriteLine($"\tEmpleado: {item.ToString()}");
    }
}

Console.WriteLine();
Console.WriteLine("ToLookUp");

var groupResultToLookUp = employeesList.OrderBy(emp => emp.DepartamentoId).ToLookup(emp => emp.DepartamentoId);

foreach (var groupResult in groupResultToLookUp)
{
    Console.WriteLine($"Departamento Id: {groupResult.Key}");
    foreach (var item in groupResult)
    {
        Console.WriteLine($"\tEmpleado: {item.ToString()}");
    }
}

Console.WriteLine();
Console.WriteLine("Quantifier Operators");
Console.WriteLine("====================");

Console.WriteLine("Indicar que el operador All aplica para todos los elementos de la lista");
var sueldoPromedioAnual = 25000m;

var resultadoAll = employeesList.All(emp => emp.SalarioAnual > sueldoPromedioAnual);

if (resultadoAll)
{
    Console.WriteLine($"Si, todos los sueldos son mayores a {sueldoPromedioAnual}");
}
else
{
    Console.WriteLine($"No, todos los sueldos no son mayores a {sueldoPromedioAnual}");
}

Console.WriteLine();
Console.WriteLine("El operador any devuelve true o false si por lo menos uno satisface la condicion.");

var resultadoAny = employeesList.Any(emp => emp.SalarioAnual > sueldoPromedioAnual);
if (resultadoAny)
{
    Console.WriteLine($"Por lo menos un sueldo es mayor a {sueldoPromedioAnual}");
}
else
{
    Console.WriteLine($"Ningun sueldo es mayor a {sueldoPromedioAnual}");
}

Console.WriteLine();
Console.WriteLine("Contains Sirve parta identificar si un elemento esta contenido en una lista.");

var empleadoABuscar = new Employee()
{
    Id = 5,
    Nombres = "Carolin",
    Apellidos = "Lopez Zuniga",
    SalarioAnual = 70000m,
    EsAdministrador = false,
    DepartamentoId = 3,
    FechaNacimiento = new DateTime(1988, 10, 20),
};

var existeEmpleado = employeesList.Contains(empleadoABuscar);

if (existeEmpleado)
{
    Console.WriteLine($"El empleado {empleadoABuscar} existe");
}
else
{
    Console.WriteLine($"El empleado {empleadoABuscar} no existe");
}

Console.WriteLine();

Console.WriteLine("Filter Operators");
Console.WriteLine("================");

Console.WriteLine();

Console.WriteLine("OfType");

var datosCualquiera = Data.Data.DatosCualquiera();

var datosEnteros = from enteros in datosCualquiera.OfType<Departamento>()
                   select enteros;

foreach (var datos in datosEnteros)
{
    Console.WriteLine($"{datos}");
}

Console.WriteLine();

Console.WriteLine("Element Operators");
Console.WriteLine("=================");
Console.WriteLine();

Console.WriteLine("ElementAt (es basicamente la posicion), cuando el indice pasado no existe genera una exception.");
Console.WriteLine("Cuando se usa or default, este agarra el valor por defecto del tipo de dato si es que no existiese.");
Console.WriteLine("mas dettalle ver web: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/default-values");

var empleado = employeesList.ElementAtOrDefault(10);

if (empleado is not null)
{
    Console.WriteLine($"Mi Id es {empleado.Id} y me llamo {empleado}");
}

Console.WriteLine();
Console.WriteLine("First or FirstDefault (el primero)");
Console.WriteLine("Last or LastDefault (el ultimo)");
List<int> integerList = new List<int>() { 32, 43, 566, 76, 34, 566, 43, 45};

int resultInteger = integerList.FirstOrDefault(x => x % 6 == 0);

Console.WriteLine(resultInteger.ToString());
Console.WriteLine();

Console.WriteLine("Single or SingleOrDefault");
Console.WriteLine("este es especial, solo tiene que existir uno en la lista, caso contrario genera error.");
var empleadoUnico = employeesList.Single(x => x.Id == 1);

Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Otros operadores");

//Equality Operator
//  SequenceEqual
//Concatenation Operator
//  Concat
//Set Operators
//  Distinct
//  Except
//  Intersect
//  Union
//Generation Operators
//  DefaultIfempty
//  Empty
//  Range
//  Repeat
//Aggregate Operators
//  Aggregate
//  Average
//  Count
//  Sum
//  Max
//Parttioning Operators
//  Skip
//  SkipWhile
//  Take
//  TakeWhile
//Conversion Operators
//  ToList
//  ToDictionary
//  ToArray
//Projection Operatores
//  Select 
//  SelectMany
Console.WriteLine();
Console.WriteLine("Sequence Equal: Verifica que todos los elementos de una lista sean iguales a los de otra lista con el mismo valor y mismo orden.");

var integerList01 = new List<int>() { 2, 3, 4, 5, 6 };
var integerList02 = new List<int>() { 1, 2, 3, 4, 5, 6 };

var boolSequenceEqual = integerList01.SequenceEqual(integerList02);

employeesList.Clear();
employeesList = Data.Data.ObtenerEmpleados();

departamentsList.Clear();
departamentsList = Data.Data.ObtenerDepartamentos();

var employeesList01 = Data.Data.ObtenerEmpleados();

var boolSequenceEmpleados = employeesList.SequenceEqual(employeesList01);


Console.WriteLine(boolSequenceEmpleados);

Console.WriteLine();
Console.WriteLine("Concat: Une en una sola lista dos listas.");

var integerResultante = integerList01.Concat(integerList02);


Console.WriteLine(string.Join(" - ", integerResultante));

Console.WriteLine();

Console.WriteLine("Aggregate: aplica un acumulador sobre una secuencia.\r\nsimulamos un ejercicio de pago de fin de año mas doble aguinaldo");

var costoTotalAnualTodosLosEmpleados = employeesList.Aggregate<Employee, decimal>(0, (salarioAnual, e) =>
{
    var bono = e.EsAdministrador ? (e.SalarioAnual / 12) + 1000 : (e.SalarioAnual / 12) + 500;

    salarioAnual = (e.SalarioAnual + bono) + salarioAnual;

    return salarioAnual;
});

Console.WriteLine(costoTotalAnualTodosLosEmpleados);

Console.WriteLine();
Console.WriteLine("Lo hagamos con lista y nombres.");

var detalle = employeesList.Aggregate<Employee, string, string>("Detalle por Empleado: ",
    (s, e) =>
    {
        var bono = e.EsAdministrador ? (e.SalarioAnual / 12) + 1000 : (e.SalarioAnual / 12) + 500;

        s += $"{e} -> {e.SalarioAnual} + {bono} = {e.SalarioAnual + bono}, ";

        return s;
    }, s => s.Substring(0, s.Length - 2));

Console.WriteLine(detalle);

Console.WriteLine();

Console.WriteLine("Sacamos ahora el promedio de los administradores.");
var sueldoAdministradores = employeesList.Where(e => e.EsAdministrador == true).Average(e => e.SalarioAnual);

Console.WriteLine($"De los administradores es {sueldoAdministradores}.");

Console.ReadKey();