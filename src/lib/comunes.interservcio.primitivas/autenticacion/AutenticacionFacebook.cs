namespace comunes.interservicio.primitivas.autenticacion
{
    /// <summary>
    /// Define la configuración de autenticación por facebook
    /// </summary>
    public class AutenticacionFacebook : InfoAutenticacion
    {
        /// <summary>
        /// Uri para validar token de facebook
        /// </summary>
        public string DebugTokenUri { get; set; } = string.Empty;

        /// <summary>
        /// Uri para obtener información de usuario
        /// </summary>
        public string MeUri { get; set; } = string.Empty;
    }
}