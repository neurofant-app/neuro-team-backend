namespace comunicaciones.servicios.email;

public interface IMessageBuilder
{
    string FromTemplate(string template, string jsonData);
}
