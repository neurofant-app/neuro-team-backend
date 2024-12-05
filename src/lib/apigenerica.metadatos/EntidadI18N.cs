﻿namespace extensibilidad.metadatos;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
/// <summary>
/// Define una entdad en termios de metadatos
/// </summary>
public class EntidadI18N
{

    /// <summary>
    /// Identificador único de la entidad, para reflexi{on es el nombre completo del objeto 
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Nombre de la entidad o clave para internacionalización en la UI
    /// </summary>
    public List<DatoI18N<string>> Nombre { get; set; }


    /// <summary>
    /// Ruta relativa del endpoint en la API de PIKA
    /// </summary>
    public string? EndpointAPI { get; set; }

    /// <summary>
    /// Lista de propiedades de la entidad
    /// </summary>
    public List<PropiedadI18N> Propiedades { get; set; } = [];
}
#pragma 