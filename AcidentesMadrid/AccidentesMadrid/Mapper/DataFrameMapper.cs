using System.Globalization;
using AcidentesMadrid.Enum;
using Microsoft.Data.Analysis;

namespace AcidentesMadrid.Mapper;

public static class DataFrameMapper {

    /// <summary>
    /// Carga un CSV como DataFrame sin intentar adivinar los tipos.
    /// Todos los campos se cargan inicialmente como string.
    /// </summary>
    public static DataFrame CargarCsv(string ruta) {
        using var stream = File.OpenRead(ruta);

        return DataFrame.LoadCsv(
            stream,
            separator: ';',
            header: true,
            guessTypeFunction: _ => typeof(string)
        );
    }

    /// <summary>
    /// Limpia y convierte las columnas del DataFrame
    /// siguiendo las mismas reglas que AccidenteMapper.
    /// </summary>
    public static DataFrame Map(DataFrame raw) {

        // Campos simples
        var numeroExpediente = new StringDataFrameColumn("num_expediente");
        var localizacion = new StringDataFrameColumn("localizacion");
        var distrito = new StringDataFrameColumn("distrito");
        var estadoMeteorologico = new StringDataFrameColumn("estado_meteorológico");
        var tipoVehiculo = new StringDataFrameColumn("tipo_vehiculo");
        var tipoPersona = new StringDataFrameColumn("tipo_persona");
        var rangoEdad = new StringDataFrameColumn("rango_edad");
        var descripcionLesividad = new StringDataFrameColumn("lesividad");

        // Campos convertidos
        var fecha = new DateTimeDataFrameColumn("fecha");
        var hora = new DateTimeDataFrameColumn("hora");
        var numeroCalle = new Int32DataFrameColumn("numero");
        var codigoDistrito = new Int32DataFrameColumn("cod_distrito");
        var tipoAccidente = new StringDataFrameColumn("tipo_accidente");
        var sexo = new StringDataFrameColumn("sexo");
        var codigoLesividad = new StringDataFrameColumn("cod_lesividad");
        var coordenadaXutm = new DoubleDataFrameColumn("coordenada_x_utm");
        var coordenadaYutm = new DoubleDataFrameColumn("coordenada_y_utm");
        var positivoDroga = new BooleanDataFrameColumn("positiva_droga");
        var positivoAlcohol = new BooleanDataFrameColumn("positiva_alcohol");

        // Recorremos todas las filas
        for (long i = 0; i < raw.Rows.Count; i++) {

            // Campos simples
            numeroExpediente.Append(GetValue(raw, "num_expediente", i));
            localizacion.Append(GetValue(raw, "localizacion", i));
            distrito.Append(GetValue(raw, "distrito", i));
            estadoMeteorologico.Append(GetValue(raw, "estado_meteorológico", i));
            tipoVehiculo.Append(GetValue(raw, "tipo_vehiculo", i));
            tipoPersona.Append(GetValue(raw, "tipo_persona", i));
            rangoEdad.Append(GetValue(raw, "rango_edad", i));
            descripcionLesividad.Append(GetValue(raw, "lesividad", i));

            // Campos convertidos
            fecha.Append(ParseFecha(GetValue(raw, "fecha", i)));
            hora.Append(ParseHora(GetValue(raw, "hora", i)));
            numeroCalle.Append(ParseNumeroCalle(GetValue(raw, "numero", i)));
            codigoDistrito.Append(ParseCodigoDistrito(GetValue(raw, "cod_distrito", i)));
            tipoAccidente.Append(ParseTipoAccidente(GetValue(raw, "tipo_accidente", i)).ToString());
            sexo.Append(ParseSexo(GetValue(raw, "sexo", i)).ToString());
            codigoLesividad.Append(ParseCodigoLesividad(GetValue(raw, "cod_lesividad", i)).ToString());
            coordenadaXutm.Append(ParseCoordenada(GetValue(raw, "coordenada_x_utm", i)));
            coordenadaYutm.Append(ParseCoordenada(GetValue(raw, "coordenada_y_utm", i)));
            positivoAlcohol.Append(ParsePositivoAlcohol(GetValue(raw, "positiva_alcohol", i)));
            positivoDroga.Append(ParsePositivoDroga(GetValue(raw, "positiva_droga", i)));
        }

        return new DataFrame(
            numeroExpediente,
            fecha,
            hora,
            localizacion,
            numeroCalle,
            codigoDistrito,
            distrito,
            tipoAccidente,
            estadoMeteorologico,
            tipoVehiculo,
            tipoPersona,
            rangoEdad,
            sexo,
            codigoLesividad,
            descripcionLesividad,
            coordenadaXutm,
            coordenadaYutm,
            positivoDroga,
            positivoAlcohol
        );
    }
    
    /// <summary>
    /// Obtiene un valor del DataFrame de forma segura.
    /// </summary>
    private static string GetValue(
        DataFrame dataFrame,
        string columna,
        long fila
    ) {
        return dataFrame[columna][fila]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Convierte la fecha.
    /// Si no puede convertirla devuelve DateTime.MinValue,
    /// equivalente al default usado en DateOnly.
    /// </summary>
    private static DateTime ParseFecha(string? value) {
        return DateTime.TryParseExact(
            value,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var fecha
        ) ? fecha : default;
    }
    
    /// <summary>
    /// Convierte la hora.
    /// </summary>
    private static DateTime ParseHora(string? value) {
        if (TimeOnly.TryParse(value, out var hora)) return DateTime.MinValue.Add(hora.ToTimeSpan());
        return default;
    }
    
    /// <summary>
    /// Convierte el número de la calle.
    /// Si está vacío o es incorrecto devuelve null.
    /// </summary>
    private static int? ParseNumeroCalle(string? value) {
        return int.TryParse(value, out var numero) ? numero : null;
    }

    /// <summary>
    /// Convierte el código del distrito.
    /// </summary>
    private static int ParseCodigoDistrito(string? value) {
        return int.TryParse(value, out var codigo) ? codigo : 0;
    }


    /// <summary>
    /// Convierte el tipo de accidente.
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
    /// Convierte el sexo.
    /// </summary>
    private static Sexo ParseSexo(string? value) {
        return value?.Trim().ToLowerInvariant() switch {
            "mujer" => Sexo.Mujer,
            "hombre" => Sexo.Hombre,
            _ => Sexo.NoAsignado
        };
    }


    /// <summary>
    /// Convierte el código de lesividad.
    /// </summary>
    private static CodigoLesividad ParseCodigoLesividad(string? value) {
        if (string.IsNullOrWhiteSpace(value)) return CodigoLesividad.Desconocido;

        return int.TryParse(value, out var codigo)
               && System.Enum.IsDefined(
                   typeof(CodigoLesividad),
                   codigo
               )
            ? (CodigoLesividad)codigo
            : CodigoLesividad.Desconocido;
    }
    
    /// <summary>
    /// Convierte las coordenadas.
    /// </summary>
    private static double ParseCoordenada(string? value) {
        return double.TryParse(
            value,
            NumberStyles.Any,
            CultureInfo.GetCultureInfo("es-ES"),
            out var coordenada
        ) ? coordenada : 0;
    }
    
    /// <summary>
    /// S = true.
    /// Cualquier otro valor = false.
    /// </summary>
    private static bool ParsePositivoAlcohol(string? value) {
        return string.Equals(
            value?.Trim(),
            "S",
            StringComparison.OrdinalIgnoreCase
        );
    }
    
    /// <summary>
    /// 1 = true.
    /// Cualquier otro valor = false.
    /// </summary>
    private static bool ParsePositivoDroga(string? value) {
        return value?.Trim() == "1";
    }
}