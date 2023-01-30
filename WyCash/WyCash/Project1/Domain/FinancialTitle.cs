namespace WyCash.Domain
{
    public class FinancialTitle
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Money Money { get; set; }
        public Money TotalValue => Money.Times(Quantity);
    }
}
