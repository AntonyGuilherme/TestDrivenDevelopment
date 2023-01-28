namespace WyCash.Domain
{
    public class Currency
    {
        public string Name { get; set; }
        public string Acronym { get; set; }
        public decimal ConversionConstantToDollar { get; set; }

        public static Currency Dollar => new Currency {
                Name = "Dollar",
                Acronym = "USD",
                ConversionConstantToDollar = 1
        };

        public static Currency SwissFranc => new Currency {
                Name = "CreateSwissFranc",
                Acronym = "CHF",
                ConversionConstantToDollar = 1.5m
        };
    }
}
