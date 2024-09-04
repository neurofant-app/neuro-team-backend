namespace espaciotrabajo.api.seguridad;

public partial class ConfiguracionSeguridad
{

    #region Id Aplicaciones

    /// <summary>
    /// ID unico de la app de espacios de trabajo NeuroPad
    /// </summary>
    public const string APP_ID_WS_NEUROPAD = "10000001-0000-0000-0000-000000000001";

    /// <summary>
    /// ID unico de la app de espacios de trabajo NeuroTeam
    /// </summary>
    public const string APP_ID_WS_NEUROTEAM = "10000002-0000-0000-0000-000000000001";
    #endregion


    #region Galerías
    /// <summary>
    /// Rol de administrador total de la galería
    /// </summary>
    public const string GALERIA_ROL_ADMIN = "gal-r-admin";

    /// <summary>
    /// Permiso para listar galerías
    /// </summary>
    public const string GAL_PERM_LIST = "gal-p-list";

    /// <summary>
    /// Permiso para administrar galerías 
    /// </summary>
    public const string GAL_PERM_ADMIN = "gal-p-admin";

    /// <summary>
    /// Permiso para administrar contenido de la galería
    /// </summary>
    public const string GAL_PERM_CONTENIDO_ADMIN = "gal-p-contenido-admin";

    /// <summary>
    /// Permiso para visualir y utilziar el cotenido de la galería
    /// </summary>
    public const string GAL_PERM_CONTENIDO_VIEW = "gal-p-contenido-view";
    #endregion


    #region Control de acceso
    /// <summary>
    /// Rol de administrador del control de acceso
    /// </summary>
    public const string ACL_ROL_ADMIN = "acl-r-admin";

    /// <summary>
    /// Permiso para administrar el control de acceso
    /// </summary>
    public const string ACL_PERM_ADMIN = "acl-p-admin";

    /// <summary>
    /// Permiso para administrar roles del espacio de trabajo
    /// </summary>
    public const string ACL_PERM_ROLES = "acl-p-roles";

    /// <summary>
    /// Permiso para administrar los miembors del espacio de trabajo
    /// </summary>
    public const string ACL_PERM_MIEMBROS = "acl-p-miembros";
    #endregion 




}

