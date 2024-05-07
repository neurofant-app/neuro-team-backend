using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comunes.primitivas.configuracion.mongo;

public class ConfigureConfiguracionMongoOptions : IConfigureOptions<ConfiguracionMongo>
{
    private readonly IConfiguration configuration;

    public ConfigureConfiguracionMongoOptions(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void Configure(ConfiguracionMongo options)
    {
        options.ConexionDefault = configuration["mongo:conexion-default"];
        options.ConexionDefault = configuration["mongo:conexion-default"];
        options.ConexionesEntidad = configuration.GetSection("mongo:conexiones-entidad")
                                                  .GetChildren()
                                                  .Select(section =>
                                                  {
                                                      return new ConexionEntidad
                                                      {
                                                          Entidad = section["entidad"],
                                                          Conexion = section["conexion"],
                                                          Esquema = section["esquema"],
                                                          Coleccion = section["coleccion"]
                                                      };
                                                  })
                                                  .ToList();
    }
}
