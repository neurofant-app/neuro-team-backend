namespace modelo.repositorio.cfdi.busqueda;

public enum Ordenamiento
{
    ASC=1, DESC=0
}

public enum OperadorFiltro
{
    Igual = 0, Contiene = 1, ComienzaCon = 2, TerminaCon = 3,
    Mayor = 4, MayorIgual = 5, Menor = 6, MenorIgual = 7, Entre = 8,
    TextoCompleto = 100, BusquedaAvanzada = 101
}
