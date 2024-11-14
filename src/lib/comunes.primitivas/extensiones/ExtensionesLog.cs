using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace comunes.primitivas.extensiones;

/// <summary>
/// Extensiones de bitácora
/// </summary>
public static class ExtensionesLog
{

    #region Auxiliares privadas
    /// <summary>
    /// Nombres de métodos a excluir
    /// </summary>
    private static List<string> NO_VALIDOS = [.. "MoveNext,Start,StartAsync,Microsoft".Split(',')];
    
    /// <summary>
    /// Obtiene el frame del metodo
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    private static StackFrame? ObtieneLogFrame(StackFrame[]? frames) {
        if (frames != null)
        {
            for (int i = 1; i < frames.Length; i++)
            {
                bool valid = true;
                var frame = frames[i];
                var name = frame.GetMethod()?.Name;
                if (name != null)
                {

                    foreach (string invalid in NO_VALIDOS)
                    {
                        if (name.Contains(invalid, StringComparison.InvariantCultureIgnoreCase))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        return frame;
                    }
                }
            }
        }
        return null;
    }


    /// <summary>
    /// Obtiene los parámetros para realizar el registro en la bitácora
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    private static PrefixLogParams GetPrefixLogParams(string? texto, ref object?[]? parametros)
    {
        var prefijo = InfoMetodo().PrefijoLog();
        string s = "{0}";
        int newLen = 1;
        if (!string.IsNullOrEmpty(texto))
        {
            newLen++;
        }

        if (parametros != null)
        {
            newLen += parametros.Length;
        }

        int index = 0;
        object?[] temp = new object?[newLen];
        temp[index] = prefijo;
        index++;
        if (!string.IsNullOrEmpty(texto))
        {
            temp[index] = texto;
            s += " {" + $"{index}" + "}";
            index++;
        }

        if (parametros != null)
        {
            foreach (var param in parametros)
            {
                temp[index] = param;
                s += " {" + $"{index}" + "}";
                index++;
            }
        }

        return new PrefixLogParams() { parametros = temp, Texto = s };
    }

    #endregion
    /// <summary>
    /// Obtiene información del métoso
    /// </summary>
    /// <returns></returns>
    public static LogInfoMetodo InfoMetodo()
    {
        var frames =  (new StackTrace()).GetFrames();
        var frame = ObtieneLogFrame(frames);
        if (frame != null) {

            return new LogInfoMetodo(frame.GetMethod()?.DeclaringType?.Name, frame!.GetMethod()?.Name);
        }

        return new LogInfoMetodo(null, null);
    }

    /// <summary>
    /// Prefijo para las bitacoras
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static string PrefijoLog(this LogInfoMetodo info) => $"{info.Clase}-{info.Nombre}";
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    public static void PrefixDebug(this ILogger logger, string texto, EventId? eventId = null)
    {
        PrefixDebug(logger, texto, null, eventId); 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    /// <param name="parametros"></param>
    public static void PrefixDebug(this ILogger logger, string? texto, object?[]? parametros, EventId? eventId = null)
    {
        var info = GetPrefixLogParams(texto, ref parametros);

        if (eventId != null) {
            logger.LogDebug(eventId.Value, info.Texto, info.parametros);
        } else
        {
            logger.LogDebug(info.Texto, info.parametros);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    public static void PrefixCritical(this ILogger logger, string texto, EventId? eventId = null)
    {
        PrefixCritical(logger, texto, null, eventId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    /// <param name="parametros"></param>
    public static void PrefixCritical(this ILogger logger, string? texto, object?[]? parametros, EventId? eventId = null)
    {
        var info = GetPrefixLogParams(texto, ref parametros);

        if (eventId != null)
        {
            logger.LogCritical(eventId.Value, info.Texto, info.parametros);
        }
        else
        {
            logger.LogCritical(info.Texto, info.parametros);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    public static void PrefixError(this ILogger logger, string texto, EventId? eventId = null)
    {
        PrefixError(logger, texto, null, eventId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    /// <param name="parametros"></param>
    public static void PrefixError(this ILogger logger, string? texto, object?[]? parametros, EventId? eventId = null)
    {
        var info = GetPrefixLogParams(texto, ref parametros);

        if (eventId != null)
        {
            logger.LogError(eventId.Value, info.Texto, info.parametros);
        }
        else
        {
            logger.LogError(info.Texto, info.parametros);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    public static void PrefixInformation(this ILogger logger, string texto, EventId? eventId = null)
    {
        PrefixInformation(logger, texto, null, eventId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    /// <param name="parametros"></param>
    public static void PrefixInformation(this ILogger logger, string? texto, object?[]? parametros, EventId? eventId = null)
    {
        var info = GetPrefixLogParams(texto, ref parametros);

        if (eventId != null)
        {
            logger.LogInformation(eventId.Value, info.Texto, info.parametros);
        }
        else
        {
            logger.LogInformation(info.Texto, info.parametros);
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    public static void PrefixWarning(this ILogger logger, string texto, EventId? eventId = null)
    {
        PrefixWarning(logger, texto, null, eventId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    /// <param name="parametros"></param>
    public static void PrefixWarning(this ILogger logger, string? texto, object?[]? parametros, EventId? eventId = null)
    {
        var info = GetPrefixLogParams(texto, ref parametros);

        if (eventId != null)
        {
            logger.LogWarning(eventId.Value, info.Texto, info.parametros);
        }
        else
        {
            logger.LogWarning(info.Texto, info.parametros);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    public static void PrefixTrace(this ILogger logger, string texto, EventId? eventId = null)
    {
        PrefixTrace(logger, texto, null, eventId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="texto"></param>
    /// <param name="parametros"></param>
    public static void PrefixTrace(this ILogger logger, string? texto, object?[]? parametros, EventId? eventId = null)
    {
        var info = GetPrefixLogParams(texto, ref parametros);

        if (eventId != null)
        {
            logger.LogTrace(eventId.Value, info.Texto, info.parametros);
        }
        else
        {
            logger.LogTrace(info.Texto, info.parametros);
        }
    }
}

