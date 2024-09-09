using aprendizaje.model.almacenamiento;
using aprendizaje.model.galeria;
using System.ComponentModel.DataAnnotations.Schema;
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
    /// Determina el tipo base para el concepto de la tarjeta
    /// Sólo son adminitidos los tipos simples
    /// </summary>
    public TipoBaseFlashcard TipoConcepto { get; set; } = TipoBaseFlashcard.Texto;

    /// <summary>
    /// Determina el tipo base del contenido de la tarjeta
    /// </summary>
    public TipoBaseFlashcard TipoContendo { get; set; }

    /// <summary>
    /// Contenido personalizado asociado, 
    /// el tipo base debe establecerse como Personalizado para su uso
    /// </summary>
    public Guid? ContenidoPersonalizadoId { get; set; }

    /// <summary>
    /// Concepto del flashcard, el usuario regularmente lo ve com el título de la tarjeta 
    /// </summary>
    public List<ValorI18N<string>> Concepto { get; set; } = [];

    /// <summary>
    /// Contenido del flashcard para las de tipo multimedia y texto, puede almacenar referencia a medios existentes en la galería
    /// </summary>
    public List<ValorI18N<string>> Comtenido { get; set; } = [];

    /// <summary>
    /// Para las tajetas de tipo básico, a excepción de las de texto, almacena la referencia al elemento de la galería que apunta al contenido
    /// </summary>
    public List<ValorI18N<VinculoContenidoGaleria>> ContenidoGaleria { get; set; } = [];


    /// <summary>
    /// Contenidos de aprendizaje vinculados a la flashcard
    /// </summary>
    public List<Anexo> MediosAnexos { get; set; } = [];


    /// <summary>
    /// Contenido personalizado asociado
    /// </summary>
    [NotMapped]
    public ContenidoPersonalizado? ContenidoPersonalizado { get; set; }

}
