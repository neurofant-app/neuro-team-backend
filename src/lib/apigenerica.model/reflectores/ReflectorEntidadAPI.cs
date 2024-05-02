using extensibilidad.metadatos;
using extensibilidad.metadatos.configuraciones;
using System.Reflection;


namespace apigenerica.model.reflectores;

/// <summary>
/// REfelcto para entidades de API genérica
/// </summary>
public class ReflectorEntidadAPI: IReflectorEntidadesAPI
{

    public Entidad ObtieneEntidad (Type Tipo)
    {

        Entidad entidad = new()
        {
            Nombre = Tipo.Name.ToString(),
            Id = Guid.NewGuid().ToString()
        };

        foreach (var propertyInfo in Tipo.GetProperties())
        {
            var propiedad = GetTipoPropiedad(propertyInfo);
            entidad.Propiedades.Add(propiedad);
        }
        return entidad;
    }
    public Entidad ObtieneEntidadUI(Type dtoFull,Type dtoInsertar,Type dtoActualizar,Type dtoDespliegue)
    {
        Entidad entidad = new()
        {
            Nombre = dtoFull.Name.ToString(),
            Id = Guid.NewGuid().ToString()
        };

        foreach (var propiedad in dtoFull.GetProperties())
        {
            if (dtoInsertar.GetProperties().Any(_=>_.Name==propiedad.Name) || dtoActualizar.GetProperties().Any(_ => _.Name == propiedad.Name) || dtoDespliegue.GetProperties().Any(_ => _.Name == propiedad.Name))
            {
                var tmp = GetTipoPropiedad(propiedad);
                tmp.HabilitadoCrear = dtoInsertar.GetProperties().Any(_ => _.Name == propiedad.Name);
                tmp.HabilitadoEditar = dtoActualizar.GetProperties().Any(_ => _.Name == propiedad.Name);
                tmp.HabilitadoDespliegue = dtoDespliegue.GetProperties().Any(_ => _.Name == propiedad.Name);
                entidad.Propiedades.Add(tmp);
            }

        }

            return entidad;
    }

    protected Propiedad GetTipoPropiedad(PropertyInfo propiedadObjeto)
    {
        Propiedad propiedad = ObtenerArgumentos(propiedadObjeto);

        switch (propiedadObjeto.PropertyType)
        {
            case Type type when type == typeof(string):

                propiedad.Id= propiedadObjeto.Name;
                propiedad.Nombre = propiedadObjeto.Name;
                propiedad.Tipo = TipoDatos.Texto;

                break;

            case Type type when type == typeof(decimal) || type == typeof(decimal?):

                propiedad.Id = propiedadObjeto.Name;
                propiedad.Nombre = propiedadObjeto.Name;
                propiedad.Tipo = TipoDatos.Decimal;
                break;

            case Type type when type == typeof(DateTime):

                propiedad.Id = propiedadObjeto.Name;
                propiedad.Nombre = propiedadObjeto.Name;
                propiedad.Tipo = TipoDatos.FechaHora;
                break;
            case Type type when type == typeof(int):

                propiedad.Id = propiedadObjeto.Name;
                propiedad.Nombre = propiedadObjeto.Name;
                propiedad.Tipo = TipoDatos.Entero;
                break;


            case Type type when type == typeof(bool):

                propiedad.Id = propiedadObjeto.Name;
                propiedad.Nombre = propiedadObjeto.Name;
                propiedad.Tipo = TipoDatos.Logico;
                break;

            case Type type when type == typeof(List<string>):

                propiedad.Id = propiedadObjeto.Name;
                propiedad.Nombre = propiedadObjeto.Name;
                propiedad.Tipo = TipoDatos.ListaSeleccionMultiple;
                break;
            default:

                propiedad.Id = propiedadObjeto.Name;
                propiedad.Nombre = propiedadObjeto.Name;
                propiedad.Tipo = TipoDatos.Desconocido;
                break;
        }
        return propiedad;
    }


    protected Propiedad ObtenerArgumentos (PropertyInfo InformacionPropiedad)
    {
        Propiedad propiedad = new Propiedad();
        foreach (var attribute in InformacionPropiedad.CustomAttributes)
        {
            switch (attribute.AttributeType.Name)
            {

                case "TablaAttribute":
                    propiedad.ConfiguracionTabular = ObtenerConfiguracionTabular(attribute);
                    break;

                default:
                    break;
            }
        }

        return propiedad;
    }
    protected ConfiguracionTabular ObtenerConfiguracionTabular(CustomAttributeData atributo)
    {
        ConfiguracionTabular configuracion = new();
        foreach (var argumento in atributo.Constructor.GetParameters())
        {
            var ValorDato = atributo.ConstructorArguments[argumento.Position].Value;
            configuracion.GetType().GetProperty($"{argumento.Name.Substring(0, 1).ToUpper()}{argumento.Name.Substring(1).ToLower()}").SetValue(configuracion, ValorDato);
        }
        return configuracion;
    }
}

