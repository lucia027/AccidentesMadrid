using System.Text;
using AcidentesMadrid.Service;

Console.Title = "Accidentes de Madrid - LINQ vs DataFrame";
Console.OutputEncoding = Encoding.UTF8;

// Variables del programa
var service = new AccidentesServices();

// Aquí dejas la carga que ya utilizas actualmente
// para obtener tu colección de accidentes.
var items = 


Console.WriteLine("-------------------------------------");
Console.WriteLine("           Consultas LINQ            ");
Console.WriteLine("-------------------------------------");

var tiempoLinq = service.ConsultasLinqPlinq(items);

Console.WriteLine();
Console.WriteLine("Consultas LINQ terminadas.");
Console.WriteLine($"⏱️ Tiempo total: {tiempoLinq} milisegundos.");
Console.WriteLine();


Console.WriteLine("-------------------------------------");
Console.WriteLine("        Consultas DataFrame          ");
Console.WriteLine("-------------------------------------");

var tiempoDataFrame = service.ConsultasDataFrame();

Console.WriteLine();
Console.WriteLine("Consultas DataFrame terminadas.");
Console.WriteLine($"⏱️ Tiempo total: {tiempoDataFrame} milisegundos.");
Console.WriteLine();


Console.WriteLine("-------------------------------------");
Console.WriteLine("       Comparativa de tiempos        ");
Console.WriteLine("-------------------------------------");

Console.WriteLine($"LINQ:      {tiempoLinq} milisegundos.");
Console.WriteLine($"DataFrame: {tiempoDataFrame} milisegundos.");
Console.WriteLine();

var diferencia = Math.Abs(tiempoLinq - tiempoDataFrame);

if (tiempoLinq < tiempoDataFrame) {
    Console.WriteLine($"🏆 LINQ ha sido más rápido por {diferencia} milisegundos.");
}
else if (tiempoDataFrame < tiempoLinq) {
    Console.WriteLine($"🏆 DataFrame ha sido más rápido por {diferencia} milisegundos.");
}
else {
    Console.WriteLine("🤝 LINQ y DataFrame han tardado exactamente lo mismo.");
}

Console.WriteLine();
Console.WriteLine("Comparativa terminada.");