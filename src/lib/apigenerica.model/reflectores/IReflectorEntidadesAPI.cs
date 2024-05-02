using extensibilidad.metadatos;

namespace apigenerica.model.reflectores;

public interface IReflectorEntidadesAPI
{

    Entidad ObtieneEntidad(Type Tipo);
    Entidad ObtieneEntidadUI(Type dtoFull,Type dtoInsertar, Type dtoActualizar, Type dtoDespliegue);
}
