namespace sat.bot.comun.mocks;

public class AdministradorSecretosMock : IAdministradorSecretos
{
    public Task<byte[]> ObtieneCertificadoPFX(string secretoId)
    {
        throw new NotImplementedException();
    }

    public Task<string> ObtieneContrasenaCertificadoPFX(string secretoId)
    {
        throw new NotImplementedException();
    }

    public Task<string> ObtieneContrasenaSAT(string secretoId)
    {
        throw new NotImplementedException();
    }
}
