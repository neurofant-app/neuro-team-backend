﻿using System.Diagnostics.CodeAnalysis;

namespace extensibilidad.metadatos;


/// <summary>
/// Repreesenta una cadena de texto localizada para un idiona
/// </summary>
[ExcludeFromCodeCoverage]
public class DatoI18N<T>
{
    /// <summary>
    /// Idioma del texto, este corresponde a la combinación del código ISO_639-3 de idioma 
    /// unido al código de Pais a dos posiciones ISO_3166-1 separados por un guión, por ejemplo
    /// es-MX, es-ES, en-US, cuando no se cuente con el código de país será utilizado ccomo idioma
    /// por defecto ISO_639-3
    /// </summary>
    public required string Idioma { get; set; }

    /// <summary>
    /// Valor expresado en el idioma definido por la propiedad
    /// </summary>
    public T? Valor { get; set; }
}
