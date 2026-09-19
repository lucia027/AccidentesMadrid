using AcidentesMadrid.Enum;

namespace AcidentesMadrid.Models;

public record Accidente {
    public string NumeroExpediente { get; init; } = string.Empty;
    public DateOnly Fecha { get; init; }
    public TimeOnly Hora { get; init; }
    public string Localizacion { get; init; } = string.Empty;
    public int? NumeroCalle { get; init; }
    public int CodigoDistrito { get; init; }
    public string Distrito { get; init; } = string.Empty;
    public TipoAccidente TipoDeAccidente { get; init; }
    public string EstadoMeteorologico { get; init; } = string.Empty;
    public string TipoVehiculo { get; init; } = string.Empty;
    public string TipoVpersona { get; init; } = string.Empty;
    public string RangoEdad { get; init; } = string.Empty;
    public Sexo Sexo { get; init; }
    public CodigoLesividad CodigoLesividad { get; init; }
    public string DescripcionLesividad { get; init; } = string.Empty;
    public double CordenadaXutm { get; init; }
    public double CordenadaYutm { get; init; }
    public bool PositivoDroga { get; init; }
    public bool PositivoAlchol { get; init; }
}