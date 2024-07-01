using apigenerica.model.servicios;
using creador.contenido.model;

namespace creador.contenido.services;
/// <summary>
/// Interfaz para el servicio de EspacioTrabajo
/// </summary>
public interface IServicioCreadorContenido 
                 : IServicioEntidadGenerica<EntidadEspacioTrabajo
                                           , CreaEspacioTrabajo
                                           , ActualizaEspacioTrabajo
                                           , ConsultaEspacioTrabajo, string>
{

}
