using FluentStorage;
using FluentStorage.Blobs;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace servicio.almacenamiento.proveedores;


/// <summary>
/// Estructura base para los proveedores de almacenamiento
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class ProveedorAlmacenamientoBase : IProveedorAlmacenamiento
{
    internal IBlobStorage? _storage;
    internal readonly ILogger _logger;

    public ProveedorAlmacenamientoBase(ILogger logger) => _logger = logger;

    public virtual Task DeleteAsync(IEnumerable<string> fullPaths, CancellationToken cancellationToken = default)
    {
        if(_storage==null) { throw new Exception("Null IBlobStoraege"); }
        return  _storage.DeleteAsync(fullPaths, cancellationToken);
    }

    public virtual void Dispose()
    {
        
    }

    public virtual Task<IReadOnlyCollection<bool>> ExistsAsync(IEnumerable<string> fullPaths, CancellationToken cancellationToken = default)
    {
        if (_storage == null) { throw new Exception("Null IBlobStoraege"); }
        return _storage.ExistsAsync(fullPaths, cancellationToken);
    }

    public virtual Task<IReadOnlyCollection<Blob>> GetBlobsAsync(IEnumerable<string> fullPaths, CancellationToken cancellationToken = default)
    {
        if(_storage==null) { throw new Exception("Null IBlobStoraege"); }
        return _storage.GetBlobsAsync(fullPaths, cancellationToken);
    }

    public virtual Task<IReadOnlyCollection<Blob>> ListAsync(ListOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (_storage == null) { throw new Exception("Null IBlobStoraege"); }
        return _storage.ListAsync(options, cancellationToken);
    }

    public virtual Task<Stream> OpenReadAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        if (_storage == null) { throw new Exception("Null IBlobStoraege"); }
        return _storage.OpenReadAsync(fullPath, cancellationToken); 
    }

    public virtual Task<ITransaction> OpenTransactionAsync()
    {
        if (_storage == null) { throw new Exception("Null IBlobStoraege"); }
        return _storage.OpenTransactionAsync(); 
    }

    public virtual Task SetBlobsAsync(IEnumerable<Blob> blobs, CancellationToken cancellationToken = default)
    {
        if (_storage == null) { throw new Exception("Null IBlobStoraege"); }
        return _storage.SetBlobsAsync(blobs,   cancellationToken);    
    }

    public virtual Task WriteAsync(string fullPath, Stream dataStream, bool append = false, CancellationToken cancellationToken = default)
    {
        if (_storage == null) { throw new Exception("Null IBlobStoraege"); }
        return _storage.WriteAsync(fullPath, dataStream, append, cancellationToken);
    }
}
