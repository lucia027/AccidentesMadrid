using System.Text;
using AcidentesMadrid.Config;
using AcidentesMadrid.Infrastructure;
using AcidentesMadrid.Repository;
using AcidentesMadrid.Service;
using Microsoft.Extensions.DependencyInjection;

Console.Title = "Accidentes de Madrid - LINQ vs DataFrame";
Console.OutputEncoding = Encoding.UTF8;


// Inyección de dependencias
var provider = DependenciesProvider.BuildServiceProvider();

using var scope = provider.CreateScope();

var repository = scope.ServiceProvider
    .GetRequiredService<IAccidentesRepository>();

var service = scope.ServiceProvider
    .GetRequiredService<IAccidenteService>();


// Presentación
Console.WriteLine("==============================================");
Console.WriteLine("        🚗 ACCIDENTES DE MADRID");
Console.WriteLine("          LINQ vs DATAFRAME");
Console.WriteLine("==============================================");
Console.WriteLine();


// Carga de datos
Console.WriteLine("📂 Cargando datos...");
Console.WriteLine();

var items = (await repository
    .CargarDatosAsync(Config.CsvPath))
    .ToList();

Console.WriteLine($"✅ Datos cargados correctamente: {items.Count} accidentes.");
Console.WriteLine();


// LINQ
Console.WriteLine("──────────────────────────────────────────────");
Console.WriteLine("🔵 CONSULTAS CON LINQ");
Console.WriteLine("──────────────────────────────────────────────");
Console.WriteLine();

var tiempoLinq = service.ConsultasLinqPlinq(items);

Console.WriteLine();
Console.WriteLine($"✅ LINQ finalizado en {tiempoLinq} ms.");
Console.WriteLine();


// DataFrame
Console.WriteLine("──────────────────────────────────────────────");
Console.WriteLine("🟣 CONSULTAS CON DATAFRAME");
Console.WriteLine("──────────────────────────────────────────────");
Console.WriteLine();

var tiempoDataFrame = service.ConsultasDataFrame();

Console.WriteLine();
Console.WriteLine($"✅ DataFrame finalizado en {tiempoDataFrame} ms.");
Console.WriteLine();


// Comparativa final
Console.WriteLine("==============================================");
Console.WriteLine("              📊 RESULTADOS");
Console.WriteLine("==============================================");
Console.WriteLine();

Console.WriteLine($"🔵 LINQ:       {tiempoLinq} ms");
Console.WriteLine($"🟣 DataFrame:  {tiempoDataFrame} ms");
Console.WriteLine();

var diferencia = Math.Abs(tiempoLinq - tiempoDataFrame);

if (tiempoLinq < tiempoDataFrame) {
    Console.WriteLine(
        $"⚡ LINQ ha sido más rápido por {diferencia} ms."
    );
}
else if (tiempoDataFrame < tiempoLinq) {
    Console.WriteLine(
        $"⚡ DataFrame ha sido más rápido por {diferencia} ms."
    );
}
else {
    Console.WriteLine(
        "🤝 LINQ y DataFrame han tardado exactamente lo mismo."
    );
}

Console.WriteLine();
Console.WriteLine("==============================================");
Console.WriteLine("        ✅ Comparativa finalizada");
Console.WriteLine("==============================================");