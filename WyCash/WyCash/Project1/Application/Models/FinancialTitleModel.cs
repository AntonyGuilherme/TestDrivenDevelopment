using WyCash.Domain;

namespace WyCash.Apllication.Model
{
    public class FinancialTitleModel
    {
        public FinancialTitleModel(FinancialTitle financialTitle)
        {
            Name = financialTitle.Name;
            Quantity = financialTitle.Quantity;
            Money = financialTitle.Money;
            TotalValue = financialTitle.TotalValue;
        }

        public string Name { get; }
        public int Quantity { get; }
        public Money Money { get; }
        public Money TotalValue { get; }
    }
}
