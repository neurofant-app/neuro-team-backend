using comunes.primitivas.atributos;

namespace controlescolar.modelo.campi;

/// <summary>
/// Modelo en forma de árbol representando los campus de una cuenta
/// </summary>
[CQRSConsulta]
public class ConsultaCampusCuenta: CampusBase
{
    public List<CampusBase> Campus { get; set; }
}
