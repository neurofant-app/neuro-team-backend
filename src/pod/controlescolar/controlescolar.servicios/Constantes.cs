namespace controlescolar.servicios;

/// <summary>
/// DEfine constantes del módulos
/// </summary>
public class Constantes
{

    #region Permisos 

    /// <summary>
    /// Permite obtener la lista de campus
    /// </summary>
    public const string AplicacionId = "00000000-0000-0000-1000-000000000001";
    public const string CE_CAMPUS_PERM_LIST = "ce-campus-perm-list";

    /// <summary>
    /// Permite visualizar las propiedades del campus
    /// </summary>
    public const string CE_CAMPUS_PERM_VIEW = "ce-campus-perm-visor";

    /// <summary>
    /// Permite crear, editar y eliminar campus
    /// </summary>
    public const string CE_CAMPUS_PERM_ADMIN = "ce-campus-perm-admin";

    #endregion


    #region Roles 

    /// <summary>
    /// Tiene todos los permisos para administrar campus
    /// </summary>
    public const string CE_CAMPUS_ROL_ADMIN = "ce-campus-rol-admin";

    /// <summary>
    /// Tiene permisos para visualizar los campus y sus propieades
    /// </summary>
    public const string CE_CAMPUS_ROL_VISOR = "ce-campus-rol-visor";

    #endregion

}