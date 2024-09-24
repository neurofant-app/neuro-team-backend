using extensibilidad.metadatos.configuraciones;
using extensibilidad.metadatos.validadores;
using System.Reflection;

namespace extensibilidad.metadatos.atributos;

public static class ExtensionesAtributos
{

    public static Propiedad? ObtieneMetadatos(this PropertyInfo propertyInfo)
    {
        Propiedad propiedad = propertyInfo.ObtieneDatosDefaultPropiedad(new Propiedad());

        Attribute[] attrs = propertyInfo.GetCustomAttributes().ToArray();


        // Se llenan los arubitos base en la primera pasada
        foreach (Attribute attr in attrs)
        {
            if (attr is ProtegidoAttribute p)
            {
                // Los atributos protegidos no se exponen a la API
                return null;
            }

            // Obtiene las propiedades base
            if (attr is PropiedadAttribute a)
            {
                propiedad = a.ObtieneDatosPropiedad(propertyInfo, propiedad);
            }
        }

        // Luego se llenan los atributos opcionales
        foreach (Attribute attr in attrs)
        {
            if (attr is ListaAtttribute l)
            {
                propiedad.Tipo = l.Multiple ? TipoDatos.ListaSeleccionMultiple : TipoDatos.ListaSeleccionSimple;
                propiedad.Lista = ObtieneConfiguracionLista(l);
            }

            // Obtiene atributos de despliegue tabular
            if (attr is TablaAttribute t)
            {
                propiedad.ConfiguracionTabular = t.ObtieneConfiguracionTabular();
            }

            // Obtiene la configuración de l apropiedad en un formulario
            if (attr is FormularioAttribute f)
            {
                propiedad.ConfiguracionFormulario = f.ObtieneConfiguracionFormulario();
            }

            // DEtermin la obligatoriedad de la propiedad en el CRUD
            if (attr is ValidarRequeridaAttribute r)
            {
                if (!propiedad.Requerida.Any(p => p == r.Requerida))
                {
                    propiedad.Requerida.Add(r.Requerida);
                }
            }

            if (attr is ValidarTextoAttribute vt)
            {
                propiedad.ValidadorTexto = ObtieneValidadorTexto(vt);
            }


            if (attr is ValidarDecimalAttribute vd)
            {
                propiedad.ValidadorDecimal = ObtieneValidadorDecimal(vd);
            }

            if (attr is ValidarEnteroAttribute vint)
            {
                propiedad.ValidadorEntero = ObtieneValidadorEntero(vint);
            }

            if (attr is ValidarFechaAttribute vf)
            {
                propiedad.ValidadorFecha = ObtieneValidadorFecha(vf);
            }
        }

        return propiedad;
    }

    private static Propiedad ObtieneDatosDefaultPropiedad(this PropertyInfo propertyInfo, Propiedad propiedad)
    {
        propiedad.Nombre = propertyInfo.Name;
        propiedad.Id = propertyInfo.Name;
        propiedad.Buscable = true;
        propiedad.Visible = true;
        propiedad.ValorDefault = null;
        propiedad.Tipo = GetTipoDato(propertyInfo);
        return propiedad;
    }


    private static Propiedad ObtieneDatosPropiedad(this PropiedadAttribute a, PropertyInfo propertyInfo, Propiedad propiedad)
    {
        propiedad.Nombre = propertyInfo.Name;
        propiedad.Id = propertyInfo.Name;
        propiedad.Buscable = a.Buscable;
        propiedad.Visible = a.Visible;
        propiedad.ValorDefault = a.ValorDefault;

        if (a.TipoDatos != TipoDatos.SinAsignar)
        {
            propiedad.Tipo = a.TipoDatos;
        }
        else
        {
            // Fallback si la propiedad no tiene un tipo
            propiedad.Tipo = GetTipoDato(propertyInfo);
        }
        return propiedad;
    }


    private static ValidadorFecha? ObtieneValidadorFecha(this ValidarFechaAttribute v)
    {
        return new ValidadorFecha()
        {
            Maximo = v.Maximo,
            Minimo = v.Minimo
        };
    }

    private static ValidadorEntero? ObtieneValidadorEntero(this ValidarEnteroAttribute v)
    {
        return new ValidadorEntero()
        {
            Maximo = v.Maximo,
            Minimo = v.Minimo
        };
    }

    private static ValidadorDecimal? ObtieneValidadorDecimal(this ValidarDecimalAttribute v)
    {
        return new ValidadorDecimal()
        {
            Maximo = v.Maximo,
            Minimo = v.Minimo
        };
    }

    private static ValidadorTexto? ObtieneValidadorTexto(this ValidarTextoAttribute t)
    {
        return new ValidadorTexto()
        {
            LongitudMaxima = t.LongitudMaxima,
            LongitudMinima = t.LongitudMinima,
            RegExp = t.RegExp,
        };
    }


    private static ConfiguracionFormulario? ObtieneConfiguracionFormulario(this FormularioAttribute t)
    {
        return new ConfiguracionFormulario()
        {
            Ancho = t.Ancho,
            Indice = t.Indice,
            Renglon = t.Renglon,
            TipoDespliegue = t.TipoDespliegue,
            Visible = t.Visible,
        };
    }

    private static Lista? ObtieneConfiguracionLista(this ListaAtttribute l)
    {
        Lista lista = new()
        {
            DatosRemotos = l.Remota,
            Elementos = [],
            Endpoint = l.Endpoint,
            EndpointBusqueda = l.EndpointBusqueda,
            Id = l.ClaveLocal ?? Guid.NewGuid().ToString(),
            Ordenamiento = l.OrdenamientoLista,
            SeleccionMinima = l.SeleccionMinima,
        };

        return lista;
    }


    private static ConfiguracionTabular? ObtieneConfiguracionTabular(this TablaAttribute t)
    {
        return new ConfiguracionTabular()
        {
            Alternable = t.Alternable,
            Ancho = t.Ancho,
            Indice = t.Indice,
            Ordenable = t.Ordenable,
            Visible = t.Visible
        };
    }

    private static TipoDatos GetTipoDato(PropertyInfo propiedadObjeto)
    {
        switch (propiedadObjeto.PropertyType)
        {
            case Type type when type == typeof(string):
                return TipoDatos.Texto;

            case Type type when type == typeof(decimal) || type == typeof(decimal?):
                return TipoDatos.Decimal;


            case Type type when type == typeof(DateTime):
                return TipoDatos.FechaHora;

            case Type type when type == typeof(int):
                return TipoDatos.Entero;

            case Type type when type == typeof(bool):
                return TipoDatos.Logico;

            case Type type when type == typeof(List<string>):
                return TipoDatos.ListaSeleccionMultiple;

            default:
                return TipoDatos.SinAsignar;
        }
    }


}
