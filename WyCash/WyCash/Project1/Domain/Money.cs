using System;

namespace WyCash.Domain
{
    public class Money
    {
        public Money(string currency, decimal amount, decimal taxConstantConversionToDollar)
        {
            Currency = currency;
            Amount = amount;
            TaxConstantConversionToDollar = taxConstantConversionToDollar;
        }

        public string Currency { get; }
        public decimal Amount { get; }
        public decimal TaxConstantConversionToDollar { get; }
        private decimal AmountAtDollar => Amount / TaxConstantConversionToDollar;

        public static Money Dollar(decimal amount) => new Money("USD", amount, 1);

        public static Money SwissFranc(decimal amount) => new Money("CHF", amount, 1.5m);

        public Money Times(decimal valuation)
        {
            return new Money(Currency, Amount * valuation, TaxConstantConversionToDollar);
        }

        public Money SumUsingAsBaseDollar(Money money) => Dollar(AmountAtDollar + money.AmountAtDollar);

        public override bool Equals(object obj)
        {
            var money = obj as Money;

            return money != null && Currency == money.Currency && Amount == money.Amount;
        }
    }
}
