using System.Diagnostics;
using AcidentesMadrid.Mapper;
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

        var df = CargarDataFrames();
        
        var fechas = (DateTimeDataFrameColumn)df.Columns["fecha"];
        var horas = (DateTimeDataFrameColumn)df.Columns["hora"];
        
        Console.WriteLine("\n1. Total de accidentes");
        var uno = df.Rows.Count;
        Console.WriteLine($"Total de accidentes: {uno}");
        
        Console.WriteLine("\n2. Accidentes por distrito (Top 5)");
        var dos = df
            .GroupBy("distrito")
            .Count("num_expediente")
            .OrderByDescending("num_expediente_Count")
            .Head(5);
        foreach (var a in dos.Rows) {
            Console.WriteLine($"Distrito: {a[0]}, Accidentes: {a[1]}");
        }
        
        Console.WriteLine("\n3. Accidentes por tipo");
        var tres = df
            .GroupBy("tipo_accidente")
            .Count("num_expediente");
        foreach (var fila in tres.Rows) {
            Console.WriteLine($"Tipo: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n4. Accidentes por estado meteorológico");
        var cuatro = df
            .GroupBy("estado_meteorológico")
            .Count("num_expediente");
        foreach (var fila in cuatro.Rows) {
            Console.WriteLine($"Estado meteorologico: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n5. Accidentes por sexo");
        var cinco = df
            .GroupBy("sexo")
            .Count("num_expediente");
        foreach (var fila in cinco.Rows) {
            Console.WriteLine($"Sexo: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n6. Accidentes por rango de edad");
        var seis = df
            .GroupBy("rango_edad")
            .Count("num_expediente");
        foreach (var fila in seis.Rows) {
            Console.WriteLine($"Rango de edad: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n7. Positivos en alcohol");
        var columnaAlcohol = (BooleanDataFrameColumn)df.Columns["positiva_alcohol"];
        var siete = df.Filter(columnaAlcohol.ElementwiseEquals(true));
        foreach (var fila in siete.Rows) {
            Console.WriteLine(fila);
        }
        
        Console.WriteLine("\n8. Positivos en drogas");
        var columnaDroga = (BooleanDataFrameColumn)df.Columns["positiva_droga"];
        var ocho = df.Filter(columnaDroga.ElementwiseEquals(true));
        foreach (var fila in ocho.Rows) {
            Console.WriteLine(fila);
        }
        
        Console.WriteLine("\n9. Accidentes por día de la semana");
        var dias = new StringDataFrameColumn("dia_semana");
        for (long i = 0; i < fechas.Length; i++) {
            dias.Append(fechas[i]?.DayOfWeek.ToString());
        }
        var dfDias = new DataFrame(dias);
        var nueve = dfDias
            .GroupBy("dia_semana")
            .Count("dia_semana");
        foreach (var fila in nueve.Rows) {
            Console.WriteLine($"Día: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        
        Console.WriteLine("\n10. Accidentes por mes");
        var meses = new Int32DataFrameColumn("mes");
        for (long i = 0; i < fechas.Length; i++) {
            meses.Append(fechas[i]?.Month);
        }
        var dfMeses = new DataFrame(meses);
        var diez = dfMeses
            .GroupBy("mes")
            .Count("mes")
            .OrderBy("mes");
        foreach (var fila in diez.Rows) {
            Console.WriteLine($"Mes: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n11. Hora con más accidentes");
        var horasNumero = new Int32DataFrameColumn("hora_numero");
        for (long i = 0; i < horas.Length; i++) {
            horasNumero.Append(
                horas[i]?.Hour
            );
        }
        var dfHoras = new DataFrame(horasNumero);
        var once = dfHoras
            .GroupBy("hora_numero")
            .Count("hora_numero")
            .OrderByDescending("hora_numero_Count")
            .Head(1);
        foreach (var fila in once.Rows) {
            Console.WriteLine($"Hora: {fila[0]}:00, Accidentes: {fila[1]}");
        }

        Console.WriteLine("\n12. Lesiones más frecuentes");
        var doce = df
            .GroupBy("lesividad")
            .Count("num_expediente")
            .OrderByDescending("num_expediente_Count");
        foreach (var fila in doce.Rows) {
            Console.WriteLine($"Lesión: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n13. Tipo de vehículo más implicado");
        var trece = df
            .GroupBy("tipo_vehiculo")
            .Count("num_expediente")
            .OrderByDescending("num_expediente_Count")
            .Head(1);
        foreach (var fila in trece.Rows) {
            Console.WriteLine($"Vehículo: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n14. Accidentes con peatones");
        var columnaPersona = (StringDataFrameColumn)df.Columns["tipo_persona"];
        var catorce = df.Filter(columnaPersona.ElementwiseEquals("Peatón"));
        foreach (var fila in catorce.Rows) {
            Console.WriteLine(fila);
        }
        
        Console.WriteLine("\n15. Proporción hombre/mujer");
        var columnaSexo = (StringDataFrameColumn)df.Columns["sexo"];
        var hombres = df.Filter(columnaSexo.ElementwiseEquals("Hombre"));
        var mujeres = df.Filter(columnaSexo.ElementwiseEquals("Mujer"));
        var totalSexo = hombres.Rows.Count + mujeres.Rows.Count;
        var porcentajeHombres = (double)hombres.Rows.Count / totalSexo * 100;
        var porcentajeMujeres = (double)mujeres.Rows.Count / totalSexo * 100;
        Console.WriteLine($"Hombres: {hombres.Rows.Count} ({porcentajeHombres:F2}%)");
        Console.WriteLine($"Mujeres: {mujeres.Rows.Count} ({porcentajeMujeres:F2}%)");
        
        Console.WriteLine("\n16. Distritos con más peatones");
        var dieciseis = catorce
            .GroupBy("distrito")
            .Count("num_expediente")
            .OrderByDescending("num_expediente_Count")
            .Head(5);
        foreach (var fila in dieciseis.Rows) {
            Console.WriteLine($"Distrito: {fila[0]}, Peatones: {fila[1]}");
        }
        
        Console.WriteLine("\n17. Fin de semana vs entre semana");
        var tiposDia = new StringDataFrameColumn("tipo_dia");
        for (long i = 0; i < fechas.Length; i++) {
            var fecha = fechas[i];
            if (fecha?.DayOfWeek == DayOfWeek.Saturday || fecha?.DayOfWeek == DayOfWeek.Sunday) {
                tiposDia.Append("Fin de semana");
            } else {
                tiposDia.Append("Entre semana");
            }
        }
        var dfTiposDia = new DataFrame(tiposDia);
        var diecisiete = dfTiposDia
            .GroupBy("tipo_dia")
            .Count("tipo_dia");
        foreach (var fila in diecisiete.Rows) {
            Console.WriteLine(
                $"{fila[0]}: {fila[1]} accidentes"
            );
        }
        
        Console.WriteLine("\n18. Media de accidentes por día");
        var fechasDia = new StringDataFrameColumn("fecha_dia");
        for (long i = 0; i < fechas.Length; i++) {
            fechasDia.Append(
                fechas[i]?.ToString("yyyy-MM-dd")
            );
        }
        var dfFechasDia = new DataFrame(fechasDia);
        var accidentesPorDia = dfFechasDia
            .GroupBy("fecha_dia")
            .Count("fecha_dia");
        double sumaAccidentes = 0;
        foreach (var fila in accidentesPorDia.Rows) {
            sumaAccidentes += Convert.ToDouble(fila[1]);
        }
        var dieciocho = sumaAccidentes / accidentesPorDia.Rows.Count;
        Console.WriteLine($"Media de accidentes por día: {dieciocho:F2}");
        
        
        Console.WriteLine("\n19. Accidentes con alcohol + droga");
        var diecinueve = df.Filter(columnaAlcohol.ElementwiseEquals(true));
        diecinueve = diecinueve
            .Filter(((BooleanDataFrameColumn)diecinueve.Columns["positiva_droga"])
            .ElementwiseEquals(true)
        );
        foreach (var fila in diecinueve.Rows) {
            Console.WriteLine(fila);
        }
        
        Console.WriteLine("\n20. Rangos de edad más vulnerables (peatones)");
        var veinte = catorce
            .GroupBy("rango_edad")
            .Count("num_expediente")
            .OrderByDescending("num_expediente_Count");
        foreach (var fila in veinte.Rows) {
            Console.WriteLine($"Rango de edad: {fila[0]}, Peatones: {fila[1]}"
            );
        }
        
        Console.WriteLine("\n21. Distritos con más positivos en alcohol");
        var positivosAlcohol = df.Filter(columnaAlcohol.ElementwiseEquals(true));
        var veintiuno = positivosAlcohol
            .GroupBy("distrito")
            .Count("num_expediente")
            .OrderByDescending("num_expediente_Count")
            .Head(5);
        foreach (var fila in veintiuno.Rows) {
            Console.WriteLine($"Distrito: {fila[0]}, Positivos: {fila[1]}");
        }
        
        Console.WriteLine("\n22. Accidentes por código de distrito");
        var veintidos = df
            .GroupBy("cod_distrito")
            .Count("num_expediente")
            .OrderBy("cod_distrito");
        foreach (var fila in veintidos.Rows) {
            Console.WriteLine($"Código: {fila[0]}, Accidentes: {fila[1]}");
        }
        
        Console.WriteLine("\n23. Accidentes por año");
        var anios = new Int32DataFrameColumn("anio");
        for (long i = 0; i < fechas.Length; i++) {
            anios.Append(fechas[i]?.Year);
        }
        var dfAnios = new DataFrame(anios);
        var veintitres = dfAnios
            .GroupBy("anio")
            .Count("anio")
            .OrderBy("anio");
        foreach (var fila in veintitres.Rows) {
            Console.WriteLine($"Año: {fila[0]}, Accidentes: {fila[1]}"
            );
        }
        
        cronometro.Stop();
        return cronometro.ElapsedMilliseconds;    
    }
    
    private static DataFrame CargarDataFrames() {
    
        var rutas = Directory.GetFiles(Config.Config.CsvPath, "*.csv");
        if (!rutas.Any()) throw new FileNotFoundException($"No se encontraron archivos CSV en {Config.Config.CsvPath}");

        var dataFrames = rutas
            .Select(ruta => DataFrameMapper.Map(
                DataFrameMapper.CargarCsv(ruta)
            ))
            .ToList();

        var dataFrame = dataFrames.First().Clone();
        foreach (var df in dataFrames.Skip(1)) {
            dataFrame.Append(df.Rows, inPlace: true);
        }
        return dataFrame;
    }
}