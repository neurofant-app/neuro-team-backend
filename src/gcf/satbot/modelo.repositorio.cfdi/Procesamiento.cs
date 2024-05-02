namespace modelo.repositorio.cfdi
{
    public class Procesamiento
    {
        public string UUID { get; set; } //string(64)
        public string LinkXML { get; set; } //string(500)
        public string LinkPDF { get; set; } //string(500)
        public bool XMLDescargado { get; set; }
        public bool PDFDescargado { get; set; }
        public bool XMLAnalizado { get; set; }
        public bool AlmacenamientoCompleto { get; set; }
    }
}
