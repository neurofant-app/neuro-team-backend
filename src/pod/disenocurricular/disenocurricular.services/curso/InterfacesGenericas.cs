﻿using apigenerica.model.servicios;
using comunes.primitivas;
using disenocurricular.model;

namespace disenocurricular.services.curso;

public interface IServicioCurso : IServicioEntidadGenerica<Curso, Curso, Curso, Curso, string>
{
    Task<Respuesta> ActualizaDbSetCurso(Curso curso);
}

