using System.Globalization;

namespace extensibilidad.metadatos.validadores;

/// <summary>
/// Permite estabkecer valos mínimos ymáximos para las fechas, en el caso de fecha y hora sólo se toma la porción adecuada
/// En el caso de valores FechaHora se toma el valor completo especificado como UTC
/// </summary>
/// <remarks>
/// 
/// </remarks>
/// <param name="minimo"></param>
/// <param name="maximo"></param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ValidarFechaAttribute : Attribute
{

    public const string FECHA_DMY = "d/M/yyyy";
    public const string HORA_HMS24 = "HH:mm:ss";
    public const string FECHA_HORA24_ISO = "yyyy-MM-ddTHH:mm:ssZ";

    private readonly DateTime? _minimo = null;
    private readonly DateTime? _maximo = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minimo">Valor de fecha mínima en formato ISO UTC, nulo para ignorar</param>
    /// <param name="maximo">Valor de fecha maxima en formato ISO UTC, nulo para ignorar</param>
    /// <param name="formato">Formato de la fecha por default es la fecha hora en formato ISO, puede utilzairse cualquier formato de https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings?redirectedfrom=MSDN </param>
    public ValidarFechaAttribute(string minimo = "", string maximo = "", TipoDatos tipo = TipoDatos.FechaHora, string formato = FECHA_HORA24_ISO)
    {


        if (!string.IsNullOrEmpty(minimo) &&
            DateTime.TryParseExact(minimo, formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime min))
        {
            _minimo = AjustarTipo(min, tipo);
        }

        if (!string.IsNullOrEmpty(maximo) &&
            DateTime.TryParseExact(maximo, formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime max))
        {
            _maximo = AjustarTipo(max, tipo); ;
        }
    }

    private DateTime AjustarTipo(DateTime fecha, TipoDatos tipo)
    {
        switch (tipo)
        {
            case TipoDatos.Hora:
                return new DateTime(1900, 1, 1, fecha.Hour, fecha.Minute, fecha.Second);

            case TipoDatos.Fecha:
                return new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);

            default:
                return fecha;
        }
    }


    /// <summary>
    /// Valor mínimo aceptable, si es nulo no se valúa
    /// </summary>
    public DateTime? Minimo { get { return _minimo; } }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public DateTime? Maximo { get { return _maximo; } }
}
