namespace WyCash.Domain
{
    public class FinancialTitle
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Valuation { get; set; }
        public decimal TotalValue => Quantity * Valuation;
        public Money Money { get; set; }
        public decimal TotalValueInDollar => TotalValue / Money.TaxConstantConversionToDollar;
    }
}
