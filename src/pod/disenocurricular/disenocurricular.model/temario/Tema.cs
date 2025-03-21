﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace disenocurricular.model;

/// <summary>
/// Repreenta un tema de un temario
/// </summary>
[ExcludeFromCodeCoverage]
public class Tema
{
    /// <summary>
    /// Identificador único del tema
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Índice para el depliegue, permite que la UI muestre el tema en una posición específica
    /// </summary>
    [BsonElement("in")]
    public int Indice { get; set; }

    /// <summary>
    /// Clave externa del tema
    /// </summary>
    [BsonElement("cl")]
    public string? Clave { get; set; }

    /// <summary>
    /// Nombre de del tema de aceuerdo al idioma
    /// </summary>
    [BsonElement("n")]
    public List<ValorI18N<string>> Nombre { get; set; } = [];

    /// <summary>
    /// Identificador único del tema padre, aquellas entidades con padreID Guid.Empty 
    /// son los elementos raíz del temrio durante el despliegue jerárquico
    /// </summary>
    [BsonElement("tid")]
    public Guid TemaId { get; set; } = Guid.Empty;

}

