namespace disenocurricular.model;

public enum TipoPeriodoPlan
{
    SinPeriodos = 0,
    Mensual = 1,
    Bimestral = 2,
    Trimestral = 3,
    Cuatrimestral = 4,
    Semestral = 6,
    Anual = 12,
}


public enum TipoSeleccionTemario
{
    /// <summary>
    /// No hay una seleccion de temarios
    /// </summary>
    Ninguna = 0,
    /// <summary>
    /// Se require una cantidad fija de temarios seleccionada
    /// </summary>
    Fija = 1,
    /// <summary>
    /// Todos los temrarios so requeridos en el periodo
    /// </summary>
    Todos = 2,
    /// <summary>
    /// Culquiera de los temarios puede ser seleccionado
    /// </summary>
    Abierta = 3,
    /// <summary>
    /// El plan tiene un periodo único por ejemplo para academias que ofrecen un curso compuesto de pocos temarios que peuden tomars ecn cualrquier orden
    /// </summary>
    Unico = 4
}
