using aprendizaje.model.almacenamiento;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.flashcard;

//  - - - DETALLES
//  
//  Esta entidad se almacena directamente en un sistema de archivos 
//
//  - - - 



/// <summary>
/// Representa una tarjeta de memorización o flashcard
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class FlashCard
{
    /// <summary>
    /// Identificador único de la tarjeta, este Id se genera y se controla 
    /// al interior de la neuora para minimizar el espacio de almaenamiento 
    /// en Mongo
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador único de la Neurona a la que pertenece el flashcard
    /// </summary>
    public Guid NeuronaId { get; set; }

    /// <summary>
    /// IDentificador único del tema al que pertenece la tarjeta
    /// </summary>
    public Guid TemaId { get; set; }

    /// <summary>
    /// Concepto del flashcard 
    /// </summary>
    public List<ValorI18N<string>> Concepto { get; set; } = [];


    /// <summary>
    /// Contenido del flashcard 
    /// </summary>
    public List<ValorI18N<string>> Comtenido { get; set; } = [];


    /// <summary>
    /// Contenidos de aprendizaje vinculados a la flashcard
    /// </summary>
    public List<ContenidoAnexo> MediosAnexos { get; set; } = [];

}
