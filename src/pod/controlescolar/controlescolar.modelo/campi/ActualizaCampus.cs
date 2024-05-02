using comunes.primitivas.atributos;

namespace controlescolar.modelo.campi;

[CQRSActualizar]
public class ActualizaCampus: CampusBase
{    
    /// <summary>
     /// Identificador único del campus en el repositorio, se genera al crear un registro
     /// </summary>
    public virtual Guid Id { get; set; }

}
