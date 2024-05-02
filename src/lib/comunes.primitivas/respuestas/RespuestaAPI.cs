namespace comunes.primitivas;

public enum HttpCode
{
    None = 0, Ok = 200, NotFound = 404, Conflict = 409, BadRequest = 400, UnprocessableEntity = 422, ServerError = 500
}

public class Respuesta
{
    public bool Ok { get; set; } = false;
    public ErrorProceso? Error { get; set; }
    public HttpCode HttpCode { get; set; } = HttpCode.None;
}

public class RespuestaPayload<T> : Respuesta where T : class
{
    public object? Payload { get; set; } = null;
}




