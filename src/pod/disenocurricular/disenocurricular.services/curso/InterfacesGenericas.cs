using apigenerica.model.servicios;
using comunes.interservicio.primitivas;
using comunes.primitivas;
using disenocurricular.model;

namespace disenocurricular.services.curso;

public interface IServicioCurso : IServicioEntidadGenerica<Curso, Curso, Curso, Curso, string>
{
    Task<Respuesta> ActualizaDbSetCurso(Curso curso); 
    /// <summary>
    /// Método temporal para realizar pruebas con el proxy interservicio EspacioTrabajo
    /// </summary>
    /// <param name="UsuarioId"></param>
    /// <returns></returns>
    Task<RespuestaPayload<List<EspacioTrabajoUsuario>>> ObtieneEspacios(string UsuarioId);
}


