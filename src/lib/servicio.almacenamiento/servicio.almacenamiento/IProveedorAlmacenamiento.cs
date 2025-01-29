using FluentStorage.Blobs;

namespace servicio.almacenamiento;

/// <summary>
/// Un proveedor de almacenamiento ofrece los méetodos especializados de lectura y escritura 
/// para una proveedor específico por ejemplo GCP Bukcet o Filesystem
/// </summary>
public interface IProveedorAlmacenamiento: IBlobStorage
{

}
