namespace sat.bot.comun.mocks;

public class GestorArchivosMock : IGestorArchivos
{
    public Task<bool> AlmacenaDBSqlite(string rfc, string subscripcionId, string ruta, string version)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AlmacenaPDF(string rfc, string subscripcionId, Guid UUID, byte[] pdf)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AlmacenaXML(string rfc, string subscripcionId, Guid UUID, byte[] xml)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]?> LeePDF(string rfc, string subscripcionId, Guid UUID)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]?> LeeXML(string rfc, string subscripcionId, Guid UUID)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> RutaRWDBSqlite(string rfc, string subscripcionId, string ruta)
    {
        return @"Data Source = C:\Proyectos\contabee\contabee-backend\src\pod\cfdi.consulta\cfdi.consulta.api\contabee.db"; 
    }
}
