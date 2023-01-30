using System.Collections.Generic;
using System.Linq;
using WyCash.Domain;

namespace WyCash.Apllication.Model
{
    public class CurrencyReportModel
    {
        public CurrencyReportModel(IEnumerable<FinancialTitle> financialTitles)
        {
            var financialTitlesModel = new List<FinancialTitleModel>();
            TotalAtDollar = Money.Dollar(0);

            foreach (var financialTitle in financialTitles) 
            {
                financialTitlesModel.Add(new FinancialTitleModel(financialTitle));
                TotalAtDollar = TotalAtDollar.SumUsingAsBaseDollar(financialTitlesModel.Last().TotalValue);
            }

            FinancialTitles = financialTitlesModel;
        }

        public IEnumerable<FinancialTitleModel> FinancialTitles { get; set; }
        public Money TotalAtDollar { get; private set; }
    }
}
