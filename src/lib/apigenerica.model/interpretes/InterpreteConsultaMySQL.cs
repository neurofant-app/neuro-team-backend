using extensibilidad.metadatos;
using apigenerica.model.abstracciones;
using apigenerica.model.modelos;
using System.Globalization;
using System.Linq.Expressions;

namespace apigenerica.model.interpretes;

/// <summary>
/// INterprete de consultas para MySQL
/// </summary>
public class InterpreteConsultaMySQL : IInterpreteConsulta
{

    public string CrearConsulta(Consulta consulta, Entidad entidad, string coleccion)
    {
        string sql = $"SELECT * FROM {coleccion} ";

        if (consulta.Filtros != null)
        {
            List<string> condiciones = [];

            consulta.Filtros.ForEach(filtro =>
            {
                TipoDatos tipo = TipoParaCampo(filtro.Campo, entidad);
                if (tipo != TipoDatos.SinAsignar)
                {
                    string condicion = CondidionSQL(filtro, tipo);
                    if (!string.IsNullOrEmpty(condicion))
                    {
                        condiciones.Add(condicion);
                    }
                }
            });

        
            if (condiciones.Count > 0)
            {
                sql = $"{sql} WHERE";
                if (condiciones.Count == 1)
                {
                    sql = $"{sql} {condiciones.First()}";
                }
                else
                {
                    sql = $"{sql} {string.Join(" AND ", condiciones)}";
                }
            }
        }

        sql = $"{sql} {OrdenarConsulta(consulta)} {PaginarConsulta(consulta)}";

        return sql;
    }


    /// <summary>
    /// Devuelve el tipo de campo para cada propiedad
    /// </summary>
    /// <param name="nombre"></param>
    /// <returns></returns>
    public TipoDatos TipoParaCampo(string nombre, Entidad entidad)
    {

        var propiedad = entidad.Propiedades.FirstOrDefault(p=>p.Nombre == nombre);
        if (propiedad == null) {
            return TipoDatos.SinAsignar;
        }
        return propiedad.Tipo;
    }




    /// <summary>
    /// Creea una condicion a partir de un elemento filtro
    /// </summary>
    /// <param name="filtro"></param>
    /// <param name="tipo"></param>
    /// <returns></returns>
    private string CondidionSQL( Filtro filtro, TipoDatos tipo)
    {
        string condicion = string.Empty;
        switch (tipo)
        {//Agregar los tipos de sentencias de sql dependiendo de que tipo de dato sea ******

            case TipoDatos.Fecha:
                condicion = CondicionFechaSQL(filtro);
                break;

            case TipoDatos.Decimal:
            case TipoDatos.Entero:
                condicion = CondicionNumeroSQL(filtro);
                break;

            case TipoDatos.Texto:
                condicion = CondicionTextoSQL(filtro);
                break;

            case TipoDatos.Logico:
                condicion = CondicionBoolSQL(filtro);
                break;
        }
        if (!String.IsNullOrEmpty(condicion))
        {
            return $"{VerificarNegacion(filtro)}{condicion})";
        }
        return condicion;
    }

    /// <summary>
    /// Convierte un filtro de tipo bool a su representación SQL.
    /// En el caso de sqlite, los bool se almacenan como Integer.
    /// Por lo que se valida -> 0 = No tiene, 1 = Tiene.
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns></returns>
    private string CondicionBoolSQL( Filtro filtro)
    {
        int numero;
        if (filtro.Valores.Count == 0 || !int.TryParse(filtro.Valores[0], out numero))
        {
            return string.Empty;
        }
        if (numero < 0 || numero > 1)
        {
            return string.Empty;
        }

        switch (filtro.Operador)
        {
            case OperadorFiltro.Igual:
                return $"{filtro.Campo} = {filtro.Valores[0]}";
        }
        return string.Empty;
    }

    /// <summary>
    /// Convierte un filtro de tipo texto a su representación SQL.
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns></returns>
    private static string CondicionTextoSQL( Filtro filtro)
    {
        foreach (var valor in filtro.Valores)
        {
            if (filtro.Valores.Count == 0 || string.IsNullOrEmpty(valor))
            {
                return string.Empty;
            }
        }

        switch (filtro.Operador)
        {
            case OperadorFiltro.Igual:
                return $"{filtro.Campo} = '{filtro.Valores[0]}'";
            case OperadorFiltro.ComienzaCon:
                return $"{filtro.Campo} LIKE '{filtro.Valores[0]}%'";
            case OperadorFiltro.TerminaCon:
                return $"{filtro.Campo} LIKE '%{filtro.Valores[0]}'";
            case OperadorFiltro.Contiene:
                return $"{filtro.Campo} regexp '{filtro.Valores[0]}'";
        }
        return string.Empty;
    }

    /// <summary>
    /// Convierte un filtro de tipo Numero a su representación SQL.
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns></returns>
    private static string CondicionNumeroSQL( Filtro filtro)
    {
        List<decimal> numeros = new();
        foreach (var valor in filtro.Valores)
        {

            if (decimal.TryParse(valor, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal n))
            {
                numeros.Add(n);
            }
            else
            {
                return string.Empty;
            }
        }

        if (numeros.Count == 0 || (filtro.Operador == OperadorFiltro.Entre && numeros.Count != 2 && numeros[0] == numeros[1]))
        {
            return string.Empty;
        }

        switch (filtro.Operador)
        {
            case OperadorFiltro.Mayor:
                return $"{filtro.Campo} > {numeros[0]}";

            case OperadorFiltro.Menor:
                return $"{filtro.Campo} < {numeros[0]}";

            case OperadorFiltro.Igual:
                return $"{filtro.Campo} = {numeros[0]}";

            case OperadorFiltro.MayorIgual:
                return $"{filtro.Campo} >= {numeros[0]}";

            case OperadorFiltro.MenorIgual:
                return $"{filtro.Campo} <= {numeros[0]}";

            case OperadorFiltro.Entre:
                return numeros[0] < numeros[1]
                ? $"{filtro.Campo} BETWEEN {numeros[0]} AND {numeros[1]}"
                : $"{filtro.Campo} BETWEEN {numeros[1]} AND {numeros[0]}";
        }
        return string.Empty;
    }

    /// <summary>
    /// Convierte un filtro de fecha a su representación SQL
    /// En el caso de SQLite las fechas se almacenan como long debido a que  debe realizarse una conversión
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns>Cadena SQL si l filtro es válido o una cadena vacia</returns>
    private static string CondicionFechaSQL( Filtro filtro)
    {
        List<long> fechas = new();

        foreach (var valor in filtro.Valores)
            if (DateTime.TryParse(valor, out DateTime fecha))
            {
                fechas.Add(fecha.Ticks);
            }
            else
            {
                return string.Empty;
            }

        if (fechas.Count == 0 || (filtro.Operador == OperadorFiltro.Entre && fechas.Count != 2 && fechas[0] == fechas[1]))
        {
            return string.Empty;
        }

        switch (filtro.Operador)
        {
            case OperadorFiltro.Mayor:
                return $"{filtro.Campo} > {fechas[0]}";

            case OperadorFiltro.Menor:
                return $"{filtro.Campo} < {fechas[0]}";

            case OperadorFiltro.Igual:
                return $"{filtro.Campo} = {fechas[0]}";

            case OperadorFiltro.MayorIgual:
                return $"{filtro.Campo} >= {fechas[0]}";

            case OperadorFiltro.MenorIgual:
                return $"{filtro.Campo} <= {fechas[0]}";

            case OperadorFiltro.Entre:
                return fechas[0] < fechas[1]
                    ? $"{filtro.Campo} BETWEEN {fechas[0]} AND {fechas[1]}"
                    : $"{filtro.Campo} BETWEEN {fechas[1]} AND {fechas[0]}";
        }
        // Si no hay un operador válido devuelve una cadena vacía
        return string.Empty;
    }
    /// <summary>
    /// Verifica si el resultado del filtro sera negado 
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns>bool</returns>
    private static string VerificarNegacion( Filtro filtro)
    {
        if (filtro.Negar == true)
        {
            return "(NOT ";
        }
        return "(";
    }

    /// <summary>
    /// Devuelve el comando sql correspondiente para  paginar la consulta realizada
    /// </summary>
    /// <param name="pagina"></param>
    /// <returns>string</returns>
    private static string PaginarConsulta(Consulta consulta)
    {
        int limit = consulta.Paginado.Tamano;
        int offset = consulta.Paginado.Tamano * consulta.Paginado.Indice;
        return $"LIMIT {limit} OFFSET {offset} ";
    }

    /// <summary>
    /// Devuelve el comando sql correspondiente para ordenar el resultado de la consulta.
    /// </summary>
    /// <param name="consulta"></param>
    /// <returns>stringreturns>
    private static  string OrdenarConsulta( Consulta consulta)
    {
        //CFDI objCFDI = new CFDI();
        //if (objCFDI.GetType().GetProperty(consulta.Paginado.ColumnaOrdenamiento) == null)
        //{
        //    consulta.ColumnaOrdenamiento = "FechaCFDI ";
        //}
        string ordenConsulta = "ORDER BY ";
        ordenConsulta += consulta.Paginado.ColumnaOrdenamiento + " ";
        ordenConsulta += consulta.Paginado.Ordenamiento == 0 ? Ordenamiento.ASC : Ordenamiento.DESC;

        return ordenConsulta;
    }

    public Expression<Func<T, bool>> CrearConsultaExpresion<T>(Consulta consulta, Entidad entidad)
    {
        throw new NotImplementedException();
    }
}
