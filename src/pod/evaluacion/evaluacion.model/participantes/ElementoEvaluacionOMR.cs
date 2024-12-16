using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.participantes;

/// <summary>
/// Almacena los procetajes para la calificacion de OMR
/// </summary>
[ExcludeFromCodeCoverage]
public class ElementoEvaluacionOMR
{
    /// <summary>
    /// Identificador de la posicion del reactivo
    /// </summary>
    [BsonElement("i")]
    public int Posicion { get; set; }

    /// <summary>
    /// Porcentajes de cada alveolo evaluado en el mismo orden en que son presentadas
    /// las respuestas regularmete a,b,c,d = 1,2,3,4
    /// </summary>
    [BsonElement("p")]
    public List<int> Porcentajes { get; set; } = [];

    /// <summary>
    /// Resultado del proceso de OMR
    /// </summary>
    [BsonElement("r")]
    public ResultadoEvaluacionOMR Resultado { get; set; } = ResultadoEvaluacionOMR.SinProcesar;
}
