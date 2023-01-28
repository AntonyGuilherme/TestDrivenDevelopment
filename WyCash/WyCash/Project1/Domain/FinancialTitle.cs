namespace WyCash.Domain
{
    public class FinancialTitle
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Valuation { get; set; }
        public decimal TotalValue => Quantity * Valuation;
        public Currency Currency { get; set; }
        public decimal TotalValueInDollar => TotalValue / Currency.ConversionConstantToDollar;
    }
}
