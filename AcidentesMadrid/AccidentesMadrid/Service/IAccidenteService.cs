using AcidentesMadrid.Models;

namespace AcidentesMadrid.Service;

public interface IAccidenteService {
    long ConsultasLinqPlinq(IEnumerable<Accidente> items);
    long ConsultasDataFrame();
}