using comunes.primitivas.atributos;

namespace controlescolar.modelo.campi;

/// <summary>
/// Modelo en forma de árbol representando los campus de una cuenta
/// </summary>
[CQRSConsulta]
public class ConsultaCampusCuenta
{
    public List<ConsultaCampus> Campus { get; set; }
}
