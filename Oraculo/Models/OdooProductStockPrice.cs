namespace Oraculo.Models
{
    public class OdooProductStockPrice
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
        public string InvntryUom { get; set; }
        public string SellItem { get; set; }
        public string PrchseItem { get; set; }

        // PRECIOS
        public decimal PLGDL { get; set; }
        public decimal PLColima { get; set; }
        public decimal PLGuzman { get; set; }
        public decimal PLTecoman { get; set; }
        public decimal PLManzanillo { get; set; }

        // STOCKS
        public decimal CedisGdlAdministrativo { get; set; }
        public decimal CedisGdlOperativo { get; set; }
        public decimal Mandarina { get; set; }
        public decimal Mercado { get; set; }
        public decimal Granadilla { get; set; }
        public decimal BaseAerea { get; set; }
        public decimal Tlajomulco { get; set; }
        public decimal TlajomulcoCentro { get; set; }
        public decimal OchoDeJulio { get; set; }
        public decimal JuanDeLaBarrera { get; set; }
        public decimal ChavezCarrillo { get; set; }
        public decimal NinosHeroes { get; set; }
        public decimal Tecoman { get; set; }
        public decimal CiudadGuzman { get; set; }
        public decimal Manzanillo { get; set; }
        public decimal CedisColima { get; set; }
        public decimal VillaDeAlvarez { get; set; }
    }
}
