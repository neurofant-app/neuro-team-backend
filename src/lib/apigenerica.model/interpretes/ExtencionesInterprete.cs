
using apigenerica.model.modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;



namespace apigenerica.model.interpretes;

    public static class ExtencionesInterprete
    {
    public static IQueryable<T> OrdenarPor<T>(this IQueryable<T> origen, string columna, modelos.Ordenamiento ord=modelos.Ordenamiento.asc)
    {
        if (string.IsNullOrEmpty(columna))
        {
            return origen;
        }

        ParameterExpression parameter = Expression.Parameter(origen.ElementType, "");

        MemberExpression property = Expression.Property(parameter, columna);
        LambdaExpression lambda = Expression.Lambda(property, parameter);

        string methodName = ord == modelos.Ordenamiento.asc ? "OrderBy" : "OrderByDescending";
        Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                              [origen.ElementType, property.Type],
                              origen.Expression, Expression.Quote(lambda));

        return origen.Provider.CreateQuery<T>(methodCallExpression);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="origen">consulta origen</param>
    /// <param name="consulta">Consulta realizada</param>

    /// <returns></returns
    public static async Task<PaginaGenerica<T>> PaginadoAsync<T>(this IQueryable<T> origen,Consulta consulta,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var items = new List<T>();
        int count = 0;
        var indice = consulta.Paginado.Indice;
        var tamano = consulta.Paginado.Tamano;
        if (origen.Any())
        { 
            count = await origen.CountAsync(cancellationToken);
            int desde = (indice * tamano);

            items = await origen.Skip(desde)
                .Take(tamano).ToListAsync(cancellationToken);
        }
        var list = new PaginaGenerica<T>
        {
            ConsultaId = Guid.NewGuid().ToString(),
            Elementos = items,
            Milisegundos = 0,
            Paginado = new Paginado() { Indice = indice, Tamano = tamano, Ordenamiento = consulta.Paginado.Ordenamiento, ColumnaOrdenamiento = consulta.Paginado.ColumnaOrdenamiento },
            Total = count,
        };

        return list;
    }

    public static PaginaGenerica<T> Paginado<T>(this IQueryable<T> origen, Consulta consulta)
    {
        var items = new List<T>();
        int count = 0;
        var indice = consulta.Paginado.Indice;
        var tamano = consulta.Paginado.Tamano;
        if (origen.Any())
        {
            count = origen.Count();
            int desde = (indice * tamano);

            items = origen.Skip(desde)
                .Take(tamano).ToList();
        }
        var list = new PaginaGenerica<T>
        {
            ConsultaId = Guid.NewGuid().ToString(),
            Elementos = items,
            Milisegundos = 0,
            Paginado = new Paginado() { Indice = indice, Tamano = tamano, Ordenamiento = consulta.Paginado.Ordenamiento, ColumnaOrdenamiento = consulta.Paginado.ColumnaOrdenamiento },
            Total = count,
        };

        return list;
    }

    public static MethodCallExpression getWhereExpression<T>(this Expression<Func<T, bool>> lambda,Expression dbExpresion)
        {
        return  Expression.Call(
             typeof(Queryable),
             "Where",
             [typeof(T)],
             dbExpresion,lambda);

    }
}



