using System.Globalization;
using AcidentesMadrid.Mapper;
using AcidentesMadrid.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace AcidentesMadrid.Repository;

/// <summary>
/// Implementación de la interfaz IAccidentesRepository.
/// </summary>
public class AccidenteRepository : IAccidentesRepository {
    
    /// <summary>
    /// Lee todos los ficheros CSV del directorio de forma asíncrona
    /// y concurrente, combinándolos en una única colección.
    /// </summary>
    public async Task<IEnumerable<Accidente>> CargarDatosAsync(string path) {
        if (!Directory.Exists(path)) throw new DirectoryNotFoundException($"No existe el directorio {path}");
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            HasHeaderRecord = true, //Tiene cabecera
            Delimiter = ";", // Indicamos el separador.
            MissingFieldFound = null, //Para que no salte excepcion si faltan campos.
            HeaderValidated = null, //Desactiva la validacion de cabeceras.
            TrimOptions = TrimOptions.Trim //Elimina espacios.  
        };

        var archivos = Directory.GetFiles(path, "*.csv");

        var proceso = archivos.Select(async archivo => {
            using var reader = new StreamReader(archivo);
            using var csv = new CsvReader(reader, config);

            //Mapea los datos para que sigan el formato correcto.
            csv.Context.RegisterClassMap<AccidenteMapper>();

            var accidentes = new List<Accidente>();
            await foreach (var accidente in csv.GetRecordsAsync<Accidente>()) accidentes.Add(accidente);
            return accidentes;
        });

        var resultados = await Task.WhenAll(proceso);
        return resultados
            .SelectMany(accidentes => accidentes)
            .ToList();
    }
}