namespace comunes.interservicio.primitivas.autenticacion
{
    /// <summary>
    /// Contiene los objetos de configuración para autenticar por redes sociales
    /// </summary>
    public class SocialAuthConfig
    {
        /// <summary>
        /// Configuración de autenticación por facebook
        /// </summary>
        public AutenticacionFacebook Facebook { get; set; }

        /// <summary>
        /// Configuración de autentición por Google
        /// </summary>
        public AutenticacionGoogle Google { get; set; }
    }
}