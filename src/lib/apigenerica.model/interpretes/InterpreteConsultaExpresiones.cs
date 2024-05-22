using extensibilidad.metadatos;
using apigenerica.model.abstracciones;
using apigenerica.model.modelos;
using System.Linq.Expressions;
using System.Reflection;

namespace apigenerica.model.interpretes;

/// <summary>
/// INterprete de consultas para MySQL
/// </summary>
public class InterpreteConsultaExpresiones : IInterpreteConsulta
{
    public Expression<Func<T, bool>> CrearConsultaExpresion<T>(Consulta consulta, Entidad entidad)
    {
        Expression final = null;
        Type type = typeof(T);
        ParameterExpression pe = Expression.Parameter(type, "_");
        if (consulta.Filtros !=null && consulta.Filtros.Count > 0)
        {
            List<Expression> expressions = new List<Expression>();

            foreach(var filtro in consulta.Filtros)
            {
                Expression e = null;
                var p = entidad.Propiedades.Where(x => x.Id.ToLower() == filtro.Campo.ToLower()).FirstOrDefault();
                if(p!=null)
                {

                    switch (p.Tipo)
                    {
                        case TipoDatos.Entero:
                        case TipoDatos.Decimal:
                        case TipoDatos.flotante:
                        case TipoDatos.Long:
                            e = GetNumExpression(pe, p, filtro.Operador, string.Join(',',filtro.Valores), filtro.Negar);
                            break;
                        case TipoDatos.FechaHora:
                            e = GetDateTimeExpression(pe, p, filtro.Operador, string.Join(',', filtro.Valores), filtro.Negar);
                            break;

                        case TipoDatos.Logico:
                            e = GetBoolExpression(pe, p, filtro.Operador, string.Join(',', filtro.Valores), filtro.Negar);
                            break;

                        case TipoDatos.Texto:
                            e = GetStringExpression(pe, p, filtro.Operador, string.Join(',', filtro.Valores), filtro.Negar);
                            break;

                        case TipoDatos.Guid:
                            e = GetGuidExpression(pe, p, filtro.Operador, string.Join(',', filtro.Valores), filtro.Negar);
                            break;

                        default:
                            break;
                    }

                    if (e != null)
                    {
                        expressions.Add(e);
                    }
                }
            }
            foreach (Expression expression in expressions)
            {
                final = (final == null) ? expression : Expression.AndAlso(final, expression);
            }
        }

        return final != null ? Expression.Lambda<Func<T, bool>>(final, pe) : null;
    } 

    #region expression composer

    private Expression GetDateTimeExpression(ParameterExpression x, Propiedad p,
        OperadorFiltro Operador, string Value, bool negar)
    {
        //2020-07-14T12:33:12-05:00,2020-11-27T12:43:08-06:00
        string[] valores = Value.Split(',');

        List<DateTime> fechas = new List<DateTime>();
        foreach (string v in valores)
        {
            if (DateTime.TryParse(v, out DateTime f))
            {
                fechas.Add(f);
            }
        }

        if (fechas.Count == 0)
        {
            return null;
        }
        else
        {
            switch (Operador)
            {
                case OperadorFiltro.Entre:
                    if (fechas.Count < 2) return null;
                    if (fechas[0] > fechas[1]) return null;
                    break;
                default:
                    if (fechas.Count >= 2) return null;
                    break;
            }


         }



        Expression exp = Expression.Property(x, p.Id);
        Expression comparison = null;
        Expression hasValueExpression = null;
        Expression valueExpression = null;

        if (p.Nullable)
        {
            hasValueExpression = Expression.Property(exp, "HasValue");
            valueExpression = Expression.Property(exp, "Value");
        }

        switch (Operador)
        {

            case OperadorFiltro.Entre:
                if (p.Nullable)
                {
                    Expression despuesE = Expression.GreaterThanOrEqual(valueExpression, Expression.Constant(fechas[0]));
                    Expression anteE = Expression.LessThanOrEqual(valueExpression, Expression.Constant(fechas[1]));
                    Expression despuesEV = Expression.AndAlso(hasValueExpression, despuesE);
                    Expression anteEV = Expression.AndAlso(hasValueExpression, anteE);
                    comparison = Expression.AndAlso(despuesEV, anteEV);
                }
                else
                {
                    Expression despuesE = Expression.GreaterThanOrEqual(exp, Expression.Constant(fechas[0]));
                    Expression anteE = Expression.LessThanOrEqual(exp, Expression.Constant(fechas[1]));
                    comparison = Expression.AndAlso(despuesE, anteE);
                }
                break;

            case OperadorFiltro.Igual:
                if (p.Nullable)
                {
                    Expression tmp = Expression.Equal(valueExpression, Expression.Constant(fechas[0]));
                    comparison = Expression.AndAlso(hasValueExpression, tmp);
                }
                else
                {
                    comparison = Expression.Equal(exp, Expression.Constant(fechas[0]));
                }
                break;

            case OperadorFiltro.Mayor:
                if (p.Nullable)
                {
                    Expression tmp = Expression.GreaterThan(valueExpression, Expression.Constant(fechas[0]));
                    comparison = Expression.AndAlso(hasValueExpression, tmp);
                }
                else
                {
                    comparison = Expression.GreaterThan(exp, Expression.Constant(fechas[0]));
                }

                break;

            case OperadorFiltro.MayorIgual:
                if (p.Nullable)
                {
                    Expression tmp = Expression.GreaterThanOrEqual(valueExpression, Expression.Constant(fechas[0]));
                    comparison = Expression.AndAlso(hasValueExpression, tmp);
                }
                else
                {
                    comparison = Expression.GreaterThanOrEqual(exp, Expression.Constant(fechas[0]));
                }

                break;

            case OperadorFiltro.Menor:
                if (p.Nullable)
                {
                    Expression tmp = Expression.LessThan(valueExpression, Expression.Constant(fechas[0]));
                    comparison = Expression.AndAlso(hasValueExpression, tmp);
                }
                else
                {
                    comparison = Expression.LessThan(exp, Expression.Constant(fechas[0]));
                }
                break;

            case OperadorFiltro.MenorIgual:
                if (p.Nullable)
                {
                    Expression tmp = Expression.LessThanOrEqual(valueExpression, Expression.Constant(fechas[0]));
                    comparison = Expression.AndAlso(hasValueExpression, tmp);
                }
                else
                {
                    comparison = Expression.LessThanOrEqual(exp, Expression.Constant(fechas[0]));
                }
                break;

            case OperadorFiltro.Contiene:
                break;

            default:
                return null;
        }

        if (!negar)
        {
            return comparison;
        }
        else
        {
            return Expression.Not(comparison);
        }


    }



    private Expression GetNumExpression(ParameterExpression x, Propiedad p, OperadorFiltro Operador, string Values, bool negar)
    {
        Expression pe = Expression.Property(x, p.Id);
        Expression constantExpression = null;
        Expression constantExpression2 = null;
        Expression final = null;
        string[] Valores = Values.Split(',');
        List<string> numeros = new List<string>();
        foreach (string v in Valores)
        {
            if (decimal.TryParse(v, out decimal f))
            {
                numeros.Add(v);
            }
        }

        if (numeros.Count == 0)
        {
            return null;
        }
        else
        {
            switch (Operador)
            {
                case OperadorFiltro.Entre:
                    if (numeros.Count < 2) return null;
                    if (decimal.Parse(numeros[0]) > decimal.Parse(numeros[1])) return null;
                    break;
                default:
                    if (numeros.Count >=2) return null;
                    break;
            }
        }


        switch (p.Tipo)
        {
            case TipoDatos.Entero:
                constantExpression = Expression.Constant(int.Parse(numeros[0]));
                constantExpression2 = numeros.Count < 2 ? null : Expression.Constant(int.Parse(numeros[1]));
                break;

            case TipoDatos.Long:
                constantExpression = Expression.Constant(long.Parse(numeros[0]));
                constantExpression2 = numeros.Count < 2 ? null : Expression.Constant(long.Parse(numeros[1]));
                break;

            case TipoDatos.flotante:
                constantExpression = Expression.Constant(float.Parse(numeros[0]));
                constantExpression2 = numeros.Count < 2 ? null : Expression.Constant(float.Parse(numeros[1]));
                break;

            case TipoDatos.Decimal:
                constantExpression = Expression.Constant(decimal.Parse(numeros[0]));
                constantExpression2 = numeros.Count < 2 ? null : Expression.Constant(decimal.Parse(numeros[1]));
                break;
        }
        if (constantExpression != null)
        {

            switch (Operador)
            {

                case OperadorFiltro.Entre:
                        Expression despuesE = Expression.GreaterThanOrEqual(pe, constantExpression);
                        Expression anteE = Expression.LessThanOrEqual(pe, constantExpression2);
                        final = Expression.AndAlso(despuesE, anteE);
                    
                    break;

                case OperadorFiltro.Igual:

                    final = Expression.Equal(pe, constantExpression);
                    break;

                case OperadorFiltro.Mayor:

                    final = Expression.GreaterThan(pe, constantExpression);
                    break;

                case OperadorFiltro.MayorIgual:

                    final = Expression.GreaterThanOrEqual(pe, constantExpression);
                    break;

                case OperadorFiltro.Menor:

                    final = Expression.LessThan(pe, constantExpression);
                    break;

                case OperadorFiltro.MenorIgual:

                    final = Expression.LessThanOrEqual(pe, constantExpression);
                    break;

                case OperadorFiltro.Contiene:

                    switch (p.Tipo)
                    {
                        case TipoDatos.Entero:

                            List<int> lint = new List<int>();
                            foreach (string s in Values.Split(',').ToList())
                            {
                                if (int.TryParse(s, out int i))
                                {
                                    lint.Add(i);
                                }
                            }
                            Expression<Func<ICollection<int>>> valsrefint = () => lint;
                            var valsint = valsrefint.Body;
                            var miContainsint = valsrefint.Body.Type.GetMethod("Contains", new[] { typeof(int) });
                            final = Expression.Call(valsint, miContainsint, pe);
                            break;

                        case TipoDatos.Long:
                            List<long> llong = new List<long>();
                            foreach (string s in Values.Split(',').ToList())
                            {
                                if (long.TryParse(s, out long i))
                                {
                                    llong.Add(i);
                                }
                            }
                            Expression<Func<ICollection<long>>> valsreflong = () => llong;
                            var valslong = valsreflong.Body;
                            var miContainslong = valsreflong.Body.Type.GetMethod("Contains", new[] { typeof(long) });
                            final = Expression.Call(valslong, miContainslong, pe);
                            break;


                        case TipoDatos.flotante:
                            List<float> lfloat = new List<float>();
                            foreach (string s in Values.Split(',').ToList())
                            {
                                if (float.TryParse(s, out float i))
                                {
                                    lfloat.Add(i);
                                }
                            }
                            Expression<Func<ICollection<float>>> valsreffloat = () => lfloat;
                            var valsfloat = valsreffloat.Body;
                            var miContainsfloat = valsreffloat.Body.Type.GetMethod("Contains", new[] { typeof(float) });
                            final = Expression.Call(valsfloat, miContainsfloat, pe);
                            break;

                        case TipoDatos.Decimal:
                            List<decimal> ldecimal = new List<decimal>();
                            foreach (string s in Values.Split(',').ToList())
                            {
                                if (decimal.TryParse(s, out decimal i))
                                {
                                    ldecimal.Add(i);
                                }
                            }
                            Expression<Func<ICollection<decimal>>> valsrefdecimal = () => ldecimal;
                            var valsdecimal = valsrefdecimal.Body;
                            var miContainsdecimal = valsrefdecimal.Body.Type.GetMethod("Contains", new[] { typeof(decimal) });
                            final = Expression.Call(valsdecimal, miContainsdecimal, pe);
                            break;
                    }

                    break;

                default:
                    return null;
            }
        }



        if (!negar)
        {
            return final;
        }
        else
        {
            return Expression.Not(final);
        }

    }

    private Expression GetStringExpression(ParameterExpression x, Propiedad p, OperadorFiltro Operador, string Value, bool negar)
    {
        Expression pe = Expression.Property(x, p.Nombre);
        BinaryExpression isNotNull = Expression.NotEqual(pe, Expression.Constant(null, typeof(object)));
        BinaryExpression IsNUll = Expression.Equal(pe, Expression.Constant(null, typeof(object)));
        Expression constantExpression = Expression.Constant(Value.ToLower(), typeof(string));
        Expression toLower = Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));


        Expression final = null;

        if (!string.IsNullOrEmpty(Value))
        {
            switch (Operador)
            {
                case OperadorFiltro.Igual:
                    final = Expression.AndAlso(isNotNull, Expression.Equal(toLower, constantExpression));
                    break;

                case OperadorFiltro.Contiene:
                    MethodInfo methodcontain = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                    final = Expression.AndAlso(isNotNull, Expression.Call(toLower, methodcontain, constantExpression));
                    break;

                case OperadorFiltro.ComienzaCon:
                    MethodInfo methodstart = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    final = Expression.AndAlso(isNotNull, Expression.Call(toLower, methodstart, constantExpression));
                    break;

                case OperadorFiltro.TerminaCon:
                    MethodInfo methodend = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    final = Expression.AndAlso(isNotNull, Expression.Call(toLower, methodend, constantExpression));
                    break;

                default:
                    return null;
            }
        }
        if (!negar)
        {
            return final;
        }
        else
        {
            return Expression.Not(final);
        }
    }

    private Expression GetGuidExpression(ParameterExpression x, Propiedad p, OperadorFiltro Operador, string Values, bool negar)
    {
        Expression pe = Expression.Property(x, p.Nombre);

        string[] Valores = Values.Split(',');
        List<Guid> guids = new List<Guid>();
        foreach (string v in Valores)
        {
            if (Guid.TryParse(v, out Guid f))
            {
                guids.Add(f);
            }
        }

        if (guids.Count != 1 || Operador!=OperadorFiltro.Igual)
        {
            return null;
        }

        Expression constantExpression = Expression.Constant(guids[0], typeof(Guid));


        Expression final = Expression.Equal(pe, constantExpression);

        if (!negar)
        {
            return final;
        }
        else
        {
            return Expression.Not(final);
        }
    }

    private Expression GetBoolExpression(ParameterExpression x, Propiedad p, OperadorFiltro Operador, string Value, bool negar)
    {

        Expression pe = Expression.Property(x, p.Nombre);
        bool boolValue = Value.ToLower() != "true" ? false : true;
        Expression constantExpression = Expression.Constant(boolValue);
        Expression final = null;

        switch (Operador)
        {
            case OperadorFiltro.Igual:
                final = Expression.Equal(pe, constantExpression);
                break;
            default:
                return null;
        }

        if (!negar)
        {
            return final;
        }
        else
        {
            return Expression.Not(final);
        }
    }

    public string CrearConsulta(Consulta consulta, Entidad entidad, string coleccion)
    {
        throw new NotImplementedException();
    }

    #endregion
}
