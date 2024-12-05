using apigenerica.model.servicios;
using productos.model.categoria;

namespace productos.services.categoria;

public interface IServicioCategoria : IServicioEntidadGenerica<Categoria, CategoriaInsertar, CategoriaActualizar, CategoriaDespliegue, Guid>
{
}
