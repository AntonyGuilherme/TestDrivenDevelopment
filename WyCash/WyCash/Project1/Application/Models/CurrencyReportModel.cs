using System.Collections.Generic;
using System.Linq;
using WyCash.Domain;

namespace WyCash.Apllication.Model
{
    public class CurrencyReportModel
    {
        public CurrencyReportModel(IEnumerable<FinancialTitle> financialTitles)
        {
            FinancialTitles = financialTitles.Select(f => new FinancialTitleModel(f));
        }

        public IEnumerable<FinancialTitleModel> FinancialTitles { get; set; }
        public decimal TotalAtDollar => FinancialTitles.Select(f => f.TotalValueInDollar).Sum();
    }
}
