using System.Globalization;
using AcidentesMadrid.Enum;
using AcidentesMadrid.Models;
using CsvHelper.Configuration;

namespace AcidentesMadrid.Mapper;

/// <summary>
/// Mapea los datos del CSV al modelo Accidente.
/// </summary>
public class AccidenteMapper : ClassMap<Accidente> {
    public AccidenteMapper() {
        
        // Campos simples
        Map(a => a.NumeroExpediente).Name("num_expediente");
        Map(a => a.Localizacion).Name("localizacion");
        Map(a => a.Distrito).Name("distrito");
        Map(a => a.EstadoMeteorologico).Name("estado_meteorológico");
        Map(a => a.TipoVehiculo).Name("tipo_vehiculo");
        Map(a => a.TipoPersona).Name("tipo_persona");
        Map(a => a.RangoEdad).Name("rango_edad");
        Map(a => a.DescripcionLesividad).Name("lesividad");

        // Campos a convertir
        Map(a => a.Fecha).Convert(args => DateOnly.TryParseExact(args.Row.GetField("fecha"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha) ? fecha : default);
        Map(a => a.Hora).Convert(args => TimeOnly.TryParse(args.Row.GetField("hora"), out var hora) ? hora : default);
        Map(a => a.NumeroCalle).Convert(args => int.TryParse(args.Row.GetField("numero"), out var numero) ? numero : null);
        Map(a => a.CodigoDistrito).Convert(args => int.TryParse(args.Row.GetField("cod_distrito"), out var codigo) ? codigo : 0);
        Map(a => a.TipoDeAccidente).Convert(args => ParseTipoAccidente(args.Row.GetField("tipo_accidente")));
        Map(a => a.Sexo).Convert(args => ParseSexo(args.Row.GetField("sexo"))); 
        Map(a => a.CodigoLesividad).Convert(args => ParseCodigoLesividad(args.Row.GetField("cod_lesividad")));
        Map(a => a.CoordenadaXutm).Convert(args => double.TryParse(args.Row.GetField("coordenada_x_utm"), NumberStyles.Any, CultureInfo.GetCultureInfo("es-ES"), out var coordenada) ? coordenada : 0);
        Map(a => a.CoordenadaYutm).Convert(args => double.TryParse(args.Row.GetField("coordenada_y_utm"), NumberStyles.Any, CultureInfo.GetCultureInfo("es-ES"), out var coordenada) ? coordenada : 0);
        Map(a => a.PositivoAlcohol).Convert(args => string.Equals(args.Row.GetField("positiva_alcohol")?.Trim(), "S", StringComparison.OrdinalIgnoreCase));
        Map(a => a.PositivoDroga).Convert(args => args.Row.GetField("positiva_droga")?.Trim() == "1");
    }
    
    /// <summary>
    /// Convierte el tipo de accidente del CSV al enum TipoAccidente.
    /// </summary>
    private static TipoAccidente ParseTipoAccidente(string? value) {
        return value?.Trim() switch {
            "Colisión doble" => TipoAccidente.ColisionDoble,
            "Colisión múltiple" => TipoAccidente.ColisionMultiple,
            "Alcance" => TipoAccidente.Alcance,
            "Choque contra obstáculo o elemento de la vía" => TipoAccidente.CocheContraObstaculoOElementoDeLaVia,
            "Atropello a persona" => TipoAccidente.AtropelloAPersona,
            "Vuelco" => TipoAccidente.Vuelco,
            "Caída" => TipoAccidente.Caida,
            "Otras causas" => TipoAccidente.OtrasCausas,
            _ => TipoAccidente.OtrasCausas
        };
    }
    
    /// <summary>
    /// Convierte el sexo del CSV al enum Sexo.
    /// </summary>
    private static Sexo ParseSexo(string? value) {
        return value?.Trim().ToLowerInvariant() switch {
            "mujer" => Sexo.Mujer,
            "hombre" => Sexo.Hombre,
            _ => Sexo.NoAsignado
        };
    }
    
    /// <summary>
    /// Convierte el código de lesividad al enum CodigoLesividad.
    /// </summary>
    private static CodigoLesividad ParseCodigoLesividad(string? value) {
        if (string.IsNullOrWhiteSpace(value)) return CodigoLesividad.Desconocido;
        return int.TryParse(value, out var codigo) && System.Enum.IsDefined(typeof(CodigoLesividad), codigo) ? (CodigoLesividad)codigo : CodigoLesividad.Desconocido;
    }
}