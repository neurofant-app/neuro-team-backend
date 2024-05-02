using Microsoft.AspNetCore.Mvc;

namespace apigenerica.primitivas;

/// <summary>
/// INterfaz base para controlaodres de API para el crud de entidades
/// </summary>
/// <typeparam name="TipoBase">Entidad base</typeparam>
/// <typeparam name="TipoPOST">Entidad utilziada para POST</typeparam>
/// <typeparam name="TipoPUT">Entidad utilziada para PUT</typeparam>
/// <typeparam name="TipoId">Tipo del identificador</typeparam>
public interface IControladorCRUD<TipoBase, TipoPOST, TipoPUT, TipoId>
{


    Task<TipoBase> CreaEntidad([FromBody] TipoPOST data);

}

