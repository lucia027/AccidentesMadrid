namespace AcidentesMadrid.Repository.Common;

/// <summary>
/// interfaz generica con el metodo cargar datos.
/// </summary>
public interface IRepository<T> {
    public Task<IEnumerable<T>> CargarDatosAsync(string path);
}