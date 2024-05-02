namespace modelo.repositorio.cfdi
{
    public class Resumen
    {
        public int IAno { get; set; }
        public int IMes { get; set; }
        public int IDia { get; set; }
        public decimal Total { get; set; }
        public bool EsIngreso { get; set; }
        public decimal TotalIRetenidos { get; set; }
        public decimal TotalITrasladados { get; set; }
        public string? Moneda { get; set; } //string(5)  
        public long RFCId { get; set; }

        public RFC RFC { get; set; }
    }
}
