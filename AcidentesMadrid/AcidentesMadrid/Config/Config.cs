namespace AcidentesMadrid.Config;

public static class Config {
    public static readonly string BaseDirectory = AppContext.BaseDirectory;
    public static readonly string CsvPath = Path.Combine(BaseDirectory, "Data");
}