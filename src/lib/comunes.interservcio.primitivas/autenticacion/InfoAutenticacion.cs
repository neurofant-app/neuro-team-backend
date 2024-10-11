namespace comunes.interservicio.primitivas.autenticacion
{
    /// <summary>
    /// Define información de autenticación externa
    /// </summary>
    public class InfoAutenticacion
    {
        /// <summary>
        /// tipo de grant a autenticar
        /// </summary>
        public string GrantType { get; set; } = string.Empty;

        /// <summary>
        /// Id de cliente o aplicación
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Secreto de cliente o aplicación
        /// </summary>
        public string Secret { get; set; } = string.Empty;
    }
}