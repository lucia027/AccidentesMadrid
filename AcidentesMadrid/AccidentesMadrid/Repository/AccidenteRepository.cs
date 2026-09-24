using System.Globalization;
using AcidentesMadrid.Infrastructure.Interfaces;
using AcidentesMadrid.Mapper;
using AcidentesMadrid.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace AcidentesMadrid.Repository;

/// <summary>
/// Implementación de la interfaz IAccidentesRepository.
/// </summary>
public class AccidenteRepository : IAccidentesRepository, IScopedService {

    /// <summary>
    /// Lee todos los ficheros CSV del directorio de forma asíncrona
    /// y concurrente, combinándolos en una única colección.
    /// </summary>
    public async Task<IEnumerable<Accidente>> CargarDatosAsync(string path) {
        if (!Directory.Exists(path)) throw new DirectoryNotFoundException($"No existe el directorio {path}"); 

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            HasHeaderRecord = true,
            Delimiter = ";",
            MissingFieldFound = null,
            HeaderValidated = null,
            TrimOptions = TrimOptions.Trim
        };

        var archivos = Directory.GetFiles(path, "*.csv");

        var proceso = archivos.Select(async archivo => {
            using var reader = new StreamReader(archivo);
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<AccidenteMapper>();

            var accidentes = new List<Accidente>();
            await foreach (var accidente in csv.GetRecordsAsync<Accidente>()) {
                accidentes.Add(accidente);
            }
            return accidentes;
        });

        var resultados = await Task.WhenAll(proceso);

        return resultados
            .SelectMany(accidentes => accidentes)
            .ToList();
    }
}