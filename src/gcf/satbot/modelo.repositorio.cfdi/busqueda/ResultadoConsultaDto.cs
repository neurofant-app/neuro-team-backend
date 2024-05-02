
namespace modelo.repositorio.cfdi.busqqueda;
/// <summary>
/// DTO para transportar el resultado de la busqueda de texto completo
/// </summary>
public class ResultadoConsultaDto
{
    public List<string> UUIDs{ get; set; }
    public int Total { get; set; }
}
