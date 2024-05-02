namespace modelo.repositorio.cfdi
{
    public class Bitacora
    {
        public long RFCId { get; set; }
        public int Ano { get; set; }
        public TipoMonitoreo Tipo { get; set; }
        public int Enero { get; set; }
        public int Febrero { get; set; }
        public int Marzo { get; set; }
        public int Abril { get; set; }
        public int Mayo { get; set; }
        public int Junio { get; set; }
        public int Julio { get; set; }
        public int Agosto { get; set; }
        public int Septiembre { get; set; }
        public int Octubre { get; set; }
        public int Noviembre { get; set; }
        public int Diciembre { get; set; }
        public bool EsVerificacion { get; set; }
        public RFC RFC { get; set; }    
    }
}
