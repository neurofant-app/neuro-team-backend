namespace sat.bot.modelo;

/// <summary>
/// Define la tarea de conciliacion
/// </summary>
public class TareaConciliacion
{
    /// <summary>
    /// Identificador unico de la tarea
    /// </summary>
    public Guid Id { get; set; }


    /// <summary>
    /// Identificador unico de la suscripcion
    /// </summary>
    public string SubscripcionId { get; set; }


    /// <summary>
    /// RFC a procesar
    /// </summary>
    public string RFC { get; set; }

    /// <summary>
    /// Define si el acceso se realiza utilizando el certificado y clave privada = true
    /// o rf, contraseña y captcha = false
    /// </summary>
    public bool LoginCertificado { get; set; }

    /// <summary>
    /// Identificador unico del secreto que almacena el PFX para el acceso con certificado
    /// </summary>
    public string? SecretoCertificadoPFX { get; set; }

    /// <summary>
    /// Identificador unico del secreto que almacena la contraseña para el PFX de acceso con certificado
    /// </summary>
    public string? SecretoCertificadoContrasenaPFX { get; set; }

    /// <summary>
    /// Contrasena para el acceso con RFC/Contrasena/Captcha
    /// </summary>
    public string? ContrasenaRFC { get; set; }

    /// <summary>
    /// Fecha de inicio de procesamiento
    /// </summary>
    public DateTime FechaInicio { get; set; }

    /// <summary>
    /// Fecha de fina de procesamiento
    /// </summary>
    public DateTime FechaFinal { get; set; }

    /// <summary>
    /// Determina si deben conciliarse los CFDI emitidos
    /// </summary>
    public bool ConciliarEmitidos { get; set; }

    /// <summary>
    /// Determina si deben conciliarse los CFDI recibidos
    /// </summary>
    public bool ConciliarRecibidos { get; set; }

    /// <summary>
    /// Determina si deben conciliarse los CFDI cancelados
    /// </summary>
    public bool ConciliarCancelados { get; set; }

    /// <summary>
    /// Determina si deben descargarse el XML de los CFDI
    /// </summary>
    public bool DescargarXML { get; set; }

    /// <summary>
    /// Determina si deben descargarse el PDF de los CFDI
    /// </summary>
    public bool DescargarPDF { get; set; }

    /// <summary>
    /// Identificador unico de la version que se obtendra del proceso
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Intervalo para la espera de captcha
    /// </summary>
    public int EsperaCaptcha { get; set; } = 60;

    /// <summary>
    /// Lista de telefonos 
    /// </summary>
    public List<string> Telefonos { get; set; } = new List<string>();

}
