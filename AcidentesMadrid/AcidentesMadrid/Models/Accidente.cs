using AcidentesMadrid.Enum;
using CsvHelper.Configuration.Attributes;

namespace AcidentesMadrid.Models;

/// <summary>
/// Representa un accidente en el sistema.
/// </summary>
public record Accidente {
    [Name("num_expediente")]
    public string NumeroExpediente { get; init; } = string.Empty;

    [Name("fecha")]
    public DateOnly Fecha { get; init; }

    [Name("hora")]
    public TimeOnly Hora { get; init; }

    [Name("localizacion")]
    public string Localizacion { get; init; } = string.Empty;

    [Name("numero")]
    public int? NumeroCalle { get; init; }

    [Name("cod_distrito")]
    public int CodigoDistrito { get; init; }

    [Name("distrito")]
    public string Distrito { get; init; } = string.Empty;

    [Name("tipo_accidente")]
    public TipoAccidente TipoDeAccidente { get; init; }

    [Name("estado_meteorológico")]
    public string EstadoMeteorologico { get; init; } = string.Empty;

    [Name("tipo_vehiculo")]
    public string TipoVehiculo { get; init; } = string.Empty;

    [Name("tipo_persona")]
    public string TipoPersona { get; init; } = string.Empty;

    [Name("rango_edad")]
    public string RangoEdad { get; init; } = string.Empty;

    [Name("sexo")]
    public Sexo Sexo { get; init; }

    [Name("cod_lesividad")]
    public CodigoLesividad CodigoLesividad { get; init; }

    [Name("lesividad")]
    public string DescripcionLesividad { get; init; } = string.Empty;

    [Name("coordenada_x_utm")]
    public double CoordenadaXutm { get; init; }

    [Name("coordenada_y_utm")]
    public double CoordenadaYutm { get; init; }

    [Name("positiva_droga")]
    public bool PositivoDroga { get; init; }

    [Name("positiva_alcohol")]
    public bool PositivoAlcohol { get; init; }
    
    public override string ToString() {
        return $"Expediente: {NumeroExpediente} | " +
               $"Fecha: {Fecha:dd/MM/yyyy} {Hora:HH:mm} | " +
               $"Dirección: {Localizacion}, {NumeroCalle?.ToString() ?? "S/N"} | " +
               $"Distrito: {Distrito} ({CodigoDistrito}) | " +
               $"Accidente: {TipoDeAccidente} | " +
               $"Meteorología: {EstadoMeteorologico} | " +
               $"Vehículo: {TipoVehiculo} | " +
               $"Persona: {TipoPersona} | " +
               $"Edad: {RangoEdad} | " +
               $"Sexo: {Sexo} | " +
               $"Lesividad: {CodigoLesividad} - {DescripcionLesividad} | " +
               $"Coordenadas: ({CoordenadaXutm}, {CoordenadaYutm}) | " +
               $"Droga: {(PositivoDroga ? "Sí" : "No")} | " +
               $"Alcohol: {(PositivoAlcohol ? "Sí" : "No")}";
    }
}