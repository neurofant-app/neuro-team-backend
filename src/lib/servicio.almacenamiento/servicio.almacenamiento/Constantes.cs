namespace servicio.almacenamiento;

/// <summary>
/// Define los proveedores de almacenamiento disponibles
/// </summary>
public enum TipoProveedorAlmacenamiento { 
    Ninguno = 0,
    FilesystemLocal=1,
    BucketGCP = 2
}
