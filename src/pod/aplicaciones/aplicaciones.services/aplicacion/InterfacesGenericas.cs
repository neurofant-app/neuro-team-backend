﻿using apigenerica.model.servicios;
using aplicaciones.model;
namespace aplicaciones.services.aplicacion;


/// <summary>
/// Interfaz para el servicio de Aplicacion
/// </summary>
public interface IServicioAplicacion : IServicioEntidadGenerica<Aplicacion, AplicacionInsertar, AplicacionActualizar, AplicacionDesplegar, string>
{
}

