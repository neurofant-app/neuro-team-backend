using apigenerica.model.reflectores;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace apigenerica.primitivas;

/// <summary>
/// Mantiene las funciones comunes de introspeccion de ensamblados para las API genericas
/// </summary>
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
public static class IntrospeccionEnsamblados
{
    /// <summary>
    /// OBtiene la ruta donde se encuentran los ensambaldos de la aplicacion
    /// </summary>
    /// <returns></returns>
    public static string ObtieneRutaBin()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public static List<string> OntieneRutasControladorGenrico()
    {
        List<string> rutas = new List<string>();
        string Ruta = ObtieneRutaBin();
        var assemblyPath = Directory.GetFiles(Ruta, "api.comunes.dll", new EnumerationOptions() { RecurseSubdirectories = true }).FirstOrDefault();
        if (assemblyPath != null)
        {
            var assembly = Assembly.LoadFile(assemblyPath);

            var Tipo = assembly.GetTypes()
                    .Where(t =>
                    t.IsAbstract &&
                    typeof(ControladorEntidadGenerico).IsClass)
                    .FirstOrDefault();


            {
                var methods = Tipo.GetMethods();
                foreach (var m in methods)
                {
                    var atributos = m.GetCustomAttributes();
                    foreach (var att in atributos)
                    {
                        string template = null;
                        switch (att)
                        {
                            case HttpGetAttribute get:

                                template = ((HttpGetAttribute)att).Template;
                                break;

                            case HttpDeleteAttribute get:
                                template = ((HttpDeleteAttribute)att).Template;
                                break;

                            case HttpPostAttribute get:
                                template = ((HttpPostAttribute)att).Template;
                                break;

                            case HttpPutAttribute get:
                                template = ((HttpPutAttribute)att).Template;
                                break;

                            case HttpPatchAttribute get:
                                template = ((HttpPatchAttribute)att).Template;
                                break;
                        }

                        if (!string.IsNullOrEmpty(template))
                        {
                            rutas.Add(template);
                        }
                    }
                }
            }
        }
        return rutas;
    }


    /// <summary>
    /// Devuelve una lista de las clases que implementan IEntidadAPI
    /// </summary>
    /// <returns></returns>
    public static List<ServicioEntidadAPI> ObtienesServiciosICatalogoAPI()
    {
        List<ServicioEntidadAPI> l = new();
        string Ruta = ObtieneRutaBin();

        var assemblies = Directory.GetFiles(Ruta, "*.dll", new EnumerationOptions() { RecurseSubdirectories = true });

        foreach (var ensamblado in assemblies)
        {
            try
            {
                var assembly = Assembly.LoadFile(ensamblado);
                var Tipos = assembly.GetTypes()
                        .Where(t =>
                        !t.IsAbstract &&
                        typeof(IServicioCatalogoAPI).IsAssignableFrom(t))
                        .ToArray();

                foreach (var t in Tipos)
                {
                    var atributoAPI = t.GetCustomAttribute(typeof(ServicioCatalogoEntidadAPIAttribute));
                    if (atributoAPI != null)
                    {
                        FileInfo fi = new FileInfo(ensamblado);
                        ServicioEntidadAPI s = new()
                        {
                            NombreEnsamblado = t.FullName,
                            NombreRuteo = ((ServicioCatalogoEntidadAPIAttribute)atributoAPI).Entidad.Name,
                            Ruta = ensamblado
                        };
                        l.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        l.ForEach(i =>
        {
            Console.WriteLine($"{JsonSerializer.Serialize(i)}");
        });

        return l;
    }


    /// <summary>
    /// Devuelve una lista de las clases que implementan IEntidadAPI
    /// </summary>
    /// <returns></returns>
    public static List<ServicioEntidadAPI> ObtienesServiciosIEntidadAPI()
    {
        List<ServicioEntidadAPI> l = new();
        string Ruta = ObtieneRutaBin();

        var assemblies = Directory.GetFiles(Ruta, "*.dll", new EnumerationOptions() { RecurseSubdirectories = true });

        foreach (var ensamblado in assemblies)
        {
            try
            {
                var assembly = Assembly.LoadFile(ensamblado);
                var Tipos = assembly.GetTypes()
                        .Where(t =>
                        !t.IsAbstract &&
                        typeof(IServicioEntidadAPI).IsAssignableFrom(t))
                        .ToArray();

                foreach (var t in Tipos)
                {
                    var atributoAPI = t.GetCustomAttribute(typeof(ServicioEntidadAPIAttribute));
                    if (atributoAPI != null)
                    {
                        FileInfo fi = new FileInfo(ensamblado);
                        ServicioEntidadAPI s = new()
                        {
                            NombreEnsamblado = t.FullName,
                            NombreRuteo = ((ServicioEntidadAPIAttribute)atributoAPI).Entidad.Name,
                            Ruta = ensamblado,
                            Driver= ((ServicioEntidadAPIAttribute)atributoAPI).Driver,
                        };
                        l.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        l.ForEach(i =>
        {
            Console.WriteLine($"{JsonSerializer.Serialize(i)}");
        });

        return l;
    }
    /// <summary>
    /// Devuelve una lista de las clases que implementan IEntidadAPI
    /// </summary>
    /// <returns></returns>
    public static List<ServicioEntidadAPI> ObtienesServiciosIEntidadHijoAPI()
    {
        List<ServicioEntidadAPI> l = new();
        string Ruta = ObtieneRutaBin();

        var assemblies = Directory.GetFiles(Ruta, "*.dll", new EnumerationOptions() { RecurseSubdirectories = true });

        foreach (var ensamblado in assemblies)
        {
            try
            {
                var assembly = Assembly.LoadFile(ensamblado);
                var Tipos = assembly.GetTypes()
                        .Where(t =>
                        !t.IsAbstract &&
                        typeof(IServicioEntidadHijoAPI).IsAssignableFrom(t))
                        .ToArray();

                foreach (var t in Tipos)
                {
                    var atributoAPI = t.GetCustomAttribute(typeof(ServicioEntidadAPIAttribute));
                    if (atributoAPI != null)
                    {
                        FileInfo fi = new FileInfo(ensamblado);
                        ServicioEntidadAPI s = new()
                        {
                            NombreEnsamblado = t.FullName,
                            NombreRuteo = ((ServicioEntidadAPIAttribute)atributoAPI).Entidad.Name,
                            Ruta = ensamblado
                        };
                        l.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        l.ForEach(i =>
        {
            Console.WriteLine($"{JsonSerializer.Serialize(i)}");
        });

        return l;
    }
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.