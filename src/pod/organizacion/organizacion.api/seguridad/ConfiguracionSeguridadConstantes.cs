namespace organizacion.api.seguridad;

public class ConfiguracionSeguridadConstantes
{
    public const string AplicacionId = "00000000-0000-0000-0000-000000000001";
    #region Permisos

    /// <summary>
    /// Permite crear, editar y eliminar dominio
    /// </summary>
    public const string ORG_DOMINIO_PERM_ADMIN = "org-dominio-perm-admin";
    /// <summary>
    /// Permite crear, editar y eliminar dominio
    /// </summary>
    public const string ORG_DOMINIO_PERM_LIST = "org-dominio-perm-list";
    /// <summary>
    /// Permite crear, editar y eliminar UnidadesOrganizacionales
    /// </summary>
    public const string ORG_UNIDADORGANIZACIONAL_PERM_ADMIN = "org-unidadorganizacionl-perm-admin";

    /// <summary>
    /// Permite crear, editar y eliminar UnidadesOrganizacionales
    /// </summary>
    public const string ORG_USUARIOGRUPO_PERM_ADMIN = "org-usuariogrupo-perm-admin";
    #endregion


    #region Roles 

    /// <summary>
    /// Tiene todos los permisos para administrar dominios
    /// </summary>
    public const string ORG_DOMINIO_ROL_ADMIN = "org-dominio-rol-admin";
    /// <summary>
    /// Tiene todos los permisos para administrar dominios
    /// </summary>
    public const string ORG_UNIDADORGANIZACIONAL_ROL_ADMIN = "org-unidadorganizacionl-rol-admin";
    /// <summary>
    /// Tiene todos los permisos para administrar dominios
    /// </summary>
    public const string ORG_usuariogrupo_ROL_ADMIN = "org-usuariogrupo-rol-admin";

    #endregion

}
