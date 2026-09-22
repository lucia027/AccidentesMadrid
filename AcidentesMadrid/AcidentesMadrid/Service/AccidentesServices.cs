using System.Diagnostics;
using AcidentesMadrid.Models;
using AcidentesMadrid.Repository;
using Microsoft.Data.Analysis;

namespace AcidentesMadrid.Service;

public class AccidentesServices() : IAccidenteService {
    
    public long ConsultasLinqPlinq(IEnumerable<Accidente> items) {
        var cronometro = new Stopwatch();
        cronometro.Start();
        
        Console.WriteLine("\n1. Total de accidentes");
        var uno = items.Count();
        Console.WriteLine($"Total de accidentes: {uno}");
        
        Console.WriteLine("\n2. Accidentes por distrito (Top 5)");
        var dos = items
            .GroupBy(a => a.Distrito)
            .Select(a => new { Distrito = a.Key, Accidentes = a.Count() })
            .OrderByDescending(a => a.Accidentes)
            .Take(5);
        foreach (var i in dos) {
            Console.WriteLine($"Distrito: {i.Distrito}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n3. Accidentes por tipo");
        var tres = items.GroupBy(a => a.TipoDeAccidente);
        foreach (var i in tres) {
            Console.WriteLine(i.Key);
            foreach (var a in i) {
                Console.WriteLine(a);
            }
        }
        
        Console.WriteLine("\n4. Accidentes por estado meteorológico");
        var cuatro = items.GroupBy(a => a.EstadoMeteorologico);
        foreach (var i in cuatro) {
            Console.WriteLine(i.Key);
            foreach (var a in i) {
                Console.WriteLine(a);
            }
        }
        
        Console.WriteLine("\n5. Accidentes por sexo");
        var cinco = items.GroupBy(a => a.Sexo);
        foreach (var i in cinco) {
            Console.WriteLine(i.Key);
            foreach (var a in i) {
                Console.WriteLine(a);
            }
        }
        
        Console.WriteLine("\n6. Accidentes por rango de edad");
        var seis = items
            .GroupBy(a => a.RangoEdad)
            .Select(a => new { RangoEdad = a.Key, Accidentes = a.Count() });
        foreach (var i in seis) {
            Console.WriteLine($"Rango de edad: {i.RangoEdad}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n7. Positivos en alcohol");
        var siete = items.Where(a => a.PositivoAlcohol == true);
        foreach (var i in siete) {
            Console.WriteLine(i);
        }
        
        Console.WriteLine("\n8. Positivos en drogas");
        var ocho = items.Where(a => a.PositivoDroga == true);
        foreach (var i in ocho) {
            Console.WriteLine(i);
        }
        
        Console.WriteLine("\n9. Accidentes por día de la semana");
        var nueve = items.GroupBy(a => a.Fecha.DayOfWeek);
        foreach (var i in nueve) {
            Console.WriteLine(i.Key);
            foreach (var a in i) {
                Console.WriteLine(a);
            }
        }
        
        Console.WriteLine("\n10. Accidentes por mes");
        var diez= items.GroupBy(a => a.Fecha.Month);
        foreach (var i in diez) {
            Console.WriteLine(i.Key);
            foreach (var a in i) {
                Console.WriteLine(a);
            }
        }        
        
        Console.WriteLine("\n11. Hora con más accidentes");
        var once = items
            .GroupBy(a => a.Hora.Hour)
            .Select(a => new { Hora = a.Key, Accidentes = a.Count() })
            .OrderByDescending(a => a.Accidentes)
            .First();
        Console.WriteLine($"Hora: {once.Hora}:00, Accidentes: {once.Accidentes}");
        
        Console.WriteLine("\n12. Lesiones más frecuentes");
        var doce = items
            .GroupBy(a => a.DescripcionLesividad)
            .Select(a => new { Lesion = a.Key, Accidentes = a.Count() })
            .OrderByDescending(a => a.Accidentes);
        foreach (var i in doce) {
            Console.WriteLine($"Lesión: {i.Lesion}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n13. Tipo de vehículo más implicado");
        var trece = items
            .GroupBy(a => a.TipoVehiculo)
            .Select(a => new { Vehiculo = a.Key, Accidentes = a.Count() })
            .OrderByDescending(a => a.Accidentes)
            .First();
        Console.WriteLine($"Vehículo: {trece.Vehiculo}, Accidentes: {trece.Accidentes}");
        
        Console.WriteLine("\n14. Accidentes con peatones");
        var catorce = items
            .Where(a => a.TipoPersona == "Peatón");
        foreach (var i in catorce) {
            Console.WriteLine(i);
        }
        
        Console.WriteLine("\n15. Proporción hombre/mujer");
        var quince = items
            .Where(a => a.Sexo.ToString() == "Hombre" || a.Sexo.ToString() == "Mujer")
            .GroupBy(a => a.Sexo)
            .Select(a => new {
                Sexo = a.Key,
                Total = a.Count(),
                Porcentaje = (double)a.Count() /
                             items.Count(i =>
                                 i.Sexo.ToString() == "Hombre" ||
                                 i.Sexo.ToString() == "Mujer") * 100
            });
        foreach (var i in quince) {
            Console.WriteLine($"Sexo: {i.Sexo}, Total: {i.Total}, Porcentaje: {i.Porcentaje:F2}%");
        }
        
        Console.WriteLine("\n16. Distritos con más peatones");
        var dieciseis = items
            .Where(a => a.TipoPersona == "Peatón")
            .GroupBy(a => a.Distrito)
            .Select(a => new { Distrito = a.Key, Peatones = a.Count() })
            .OrderByDescending(a => a.Peatones)
            .Take(5);
        foreach (var i in dieciseis) {
            Console.WriteLine($"Distrito: {i.Distrito}, Peatones: {i.Peatones}");
        }
        
        Console.WriteLine("\n17. Fin de semana vs entre semana");
        var diecisiete = items
            .GroupBy(a =>
                a.Fecha.DayOfWeek == DayOfWeek.Saturday ||
                a.Fecha.DayOfWeek == DayOfWeek.Sunday
                    ? "Fin de semana"
                    : "Entre semana")
            .Select(a => new { TipoDia = a.Key, Accidentes = a.Count() });
        foreach (var i in diecisiete) {
            Console.WriteLine($"{i.TipoDia}: {i.Accidentes} accidentes");
        }
        
        Console.WriteLine("\n18. Media de accidentes por día");
        var dieciocho = items
            .GroupBy(a => a.Fecha)
            .Average(a => a.Count());
        Console.WriteLine($"Media de accidentes por día: {dieciocho:F2}");
        
        Console.WriteLine("\n19. Accidentes con alcohol + droga");
        var diecinueve = items
            .Where(a => a.PositivoAlcohol == true && a.PositivoDroga == true);
        foreach (var i in diecinueve) {
            Console.WriteLine(i);
        }
        
        Console.WriteLine("\n20. Rangos de edad más vulnerables (peatones)");
        var veinte = items
            .Where(a => a.TipoPersona == "Peatón")
            .GroupBy(a => a.RangoEdad)
            .Select(a => new { RangoEdad = a.Key, Peatones = a.Count() })
            .OrderByDescending(a => a.Peatones);
        foreach (var i in veinte) {
            Console.WriteLine($"Rango de edad: {i.RangoEdad}, Peatones: {i.Peatones}");
        }
        
        Console.WriteLine("\n21. Distritos con más positivos en alcohol");
        var veintiuno = items
            .Where(a => a.PositivoAlcohol == true)
            .GroupBy(a => a.Distrito)
            .Select(a => new { Distrito = a.Key, Positivos = a.Count() })
            .OrderByDescending(a => a.Positivos)
            .Take(5);
        foreach (var i in veintiuno) {
            Console.WriteLine($"Distrito: {i.Distrito}, Positivos: {i.Positivos}");
        }
        
        Console.WriteLine("\n22. Accidentes por código de distrito");
        var veintidos = items
            .GroupBy(a => a.CodigoDistrito)
            .Select(a => new { CodigoDistrito = a.Key, Accidentes = a.Count() })
            .OrderBy(a => a.CodigoDistrito);
        foreach (var i in veintidos) {
            Console.WriteLine($"Código de distrito: {i.CodigoDistrito}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n23. Accidentes por año");
        var veintitres = items
            .GroupBy(a => a.Fecha.Year)
            .Select(a => new { Año = a.Key, Accidentes = a.Count() })
            .OrderBy(a => a.Año);
        foreach (var i in veintitres) {
            Console.WriteLine($"Año: {i.Año}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n24. Evolución mensual por año");
        var veinticuatro = items
            .GroupBy(a => new {
                Año = a.Fecha.Year,
                Mes = a.Fecha.Month
            })
            .Select(a => new {
                Año = a.Key.Año,
                Mes = a.Key.Mes,
                Accidentes = a.Count()
            })
            .OrderBy(a => a.Año)
            .ThenBy(a => a.Mes);
        foreach (var i in veinticuatro) {
            Console.WriteLine($"Año: {i.Año}, Mes: {i.Mes}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n25. Distrito con más accidentes por año");
        var veinticinco = items
            .GroupBy(a => a.Fecha.Year)
            .Select(a => a
                .GroupBy(d => d.Distrito)
                .Select(d => new {
                    Año = a.Key,
                    Distrito = d.Key,
                    Accidentes = d.Count()
                })
                .OrderByDescending(d => d.Accidentes)
                .First())
            .OrderBy(a => a.Año);
        foreach (var i in veinticinco) {
            Console.WriteLine($"Año: {i.Año}, Distrito: {i.Distrito}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n26. Tendencia de alcohol por año");
        var veintiseis = items
            .Where(a => a.PositivoAlcohol == true)
            .GroupBy(a => a.Fecha.Year)
            .Select(a => new {
                Año = a.Key,
                Positivos = a.Count()
            })
            .OrderBy(a => a.Año);
        foreach (var i in veintiseis) {
            Console.WriteLine($"Año: {i.Año}, Positivos en alcohol: {i.Positivos}");
        }
        
        Console.WriteLine("\n27. Comparativa fin de semana vs entre semana por año");
        var veintisiete = items
            .GroupBy(a => new {
                Año = a.Fecha.Year,
                TipoDia =
                    a.Fecha.DayOfWeek == DayOfWeek.Saturday ||
                    a.Fecha.DayOfWeek == DayOfWeek.Sunday
                        ? "Fin de semana"
                        : "Entre semana"
            })
            .Select(a => new {
                Año = a.Key.Año,
                TipoDia = a.Key.TipoDia,
                Accidentes = a.Count()
            })
            .OrderBy(a => a.Año);
        foreach (var i in veintisiete) {
            Console.WriteLine($"Año: {i.Año}, {i.TipoDia}: {i.Accidentes} accidentes");
        }
        
        Console.WriteLine("\n28. Hora pico por año");
        var veintiocho = items
            .GroupBy(a => a.Fecha.Year)
            .Select(a => a
                .GroupBy(h => h.Hora.Hour)
                .Select(h => new {
                    Año = a.Key,
                    Hora = h.Key,
                    Accidentes = h.Count()
                })
                .OrderByDescending(h => h.Accidentes)
                .First())
            .OrderBy(a => a.Año);
        foreach (var i in veintiocho) {
            Console.WriteLine($"Año: {i.Año}, Hora pico: {i.Hora}:00, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n29. Lesión más frecuente por año");
        var veintinueve = items
            .GroupBy(a => a.Fecha.Year)
            .Select(a => a
                .GroupBy(l => l.DescripcionLesividad)
                .Select(l => new {
                    Año = a.Key,
                    Lesion = l.Key,
                    Accidentes = l.Count()
                })
                .OrderByDescending(l => l.Accidentes)
                .First())
            .OrderBy(a => a.Año);
        foreach (var i in veintinueve) {
            Console.WriteLine($"Año: {i.Año}, Lesión: {i.Lesion}, Accidentes: {i.Accidentes}");
        }
        
        Console.WriteLine("\n30. Evolución de peatones por año");
        var treinta = items
            .Where(a => a.TipoPersona == "Peatón")
            .GroupBy(a => a.Fecha.Year)
            .Select(a => new {
                Año = a.Key,
                Peatones = a.Count()
            })
            .OrderBy(a => a.Año);
        foreach (var i in treinta) {
            Console.WriteLine($"Año: {i.Año}, Peatones: {i.Peatones}");
        }
        
        cronometro.Stop();
        return cronometro.ElapsedMilliseconds;
    }

    public long ConsultasDataFrame() {
        var cronometro = new Stopwatch();
        cronometro.Start();
        
        var df = DataFrame.LoadCsv("datos.csv");
        
        Console.WriteLine("\n1. Total de accidentes");
        
        Console.WriteLine("\n2. Accidentes por distrito (Top 5)");
        
        Console.WriteLine("\n3. Accidentes por tipo");
        
        Console.WriteLine("\n4. Accidentes por estado meteorológico");
        
        Console.WriteLine("\n5. Accidentes por sexo");
        
        Console.WriteLine("\n6. Accidentes por rango de edad");
        
        Console.WriteLine("\n7. Positivos en alcohol");
        
        Console.WriteLine("\n8. Positivos en drogas");
        
        Console.WriteLine("\n9. Accidentes por día de la semana");
        
        Console.WriteLine("\n10. Accidentes por mes");
        
        Console.WriteLine("\n11. Hora con más accidentes");
        
        Console.WriteLine("\n12. Lesiones más frecuentes");
        
        Console.WriteLine("\n13. Tipo de vehículo más implicado");
        
        Console.WriteLine("\n14. Accidentes con peatones");
        
        Console.WriteLine("\n15. Proporción hombre/mujer");
        
        Console.WriteLine("\n16. Distritos con más peatones");
        
        Console.WriteLine("\n17. Fin de semana vs entre semana");
        
        Console.WriteLine("\n18. Media de accidentes por día");
        
        Console.WriteLine("\n19. Accidentes con alcohol + droga");
        
        Console.WriteLine("\n20. Rangos de edad más vulnerables (peatones)");
        
        Console.WriteLine("\n21. Distritos con más positivos en alcohol");
        
        Console.WriteLine("\n22. Accidentes por código de distrito");
        
        Console.WriteLine("\n23. Accidentes por año");
        
        Console.WriteLine("\n24. Evolución mensual por año");
        
        Console.WriteLine("\n25. Distrito con más accidentes por año");
        
        Console.WriteLine("\n26. Tendencia de alcohol por año");
        
        Console.WriteLine("\n27. Comparativa fin de semana vs entre semana por año");
        
        Console.WriteLine("\n28. Hora pico por año");
        
        Console.WriteLine("\n29. Lesión más frecuente por año");
        
        Console.WriteLine("\n30. Evolución de peatones por año");
        
        cronometro.Stop();
        return cronometro.ElapsedMilliseconds;    }
}