namespace WyCash.Domain
{
    public class Money
    {
        public Money(string currency, decimal taxConstantConversionToDollar) 
        {
            Currency = currency;
            TaxConstantConversionToDollar = taxConstantConversionToDollar;
        }

        public string Currency { get; }
        public decimal TaxConstantConversionToDollar { get; }

        public static Money Dollar => new Money("USD", 1);

        public static Money SwissFranc => new Money("CHF", 1.5m);
    }
}
