namespace modelo.repositorio.cfdi
{
    public class Estadistica
    {
        public long TotalXML { get; set; }
        public long TotalPDF { get; set; }
        public long BytesUtilizados { get; set; }
        public DateTime? FechaMinRecibidos { get; set; }
        public DateTime? FechaMaxRecibidos { get; set; }
        public DateTime? FechaMinEmitidos { get; set; }
        public DateTime? FechaMaxEmitidos { get; set; }
        public long TotalRecibidos { get; set; }
        public long TotalEmitidos { get; set; }
        public long TotalCancelados { get; set; }
        public long RFCId { get; set; }

        public RFC RFC { get; set; }
    }
}
