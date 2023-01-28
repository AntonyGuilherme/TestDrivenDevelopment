
using WyCash.Apllication.Model;
using WyCash.Apllication.Repositories;

namespace WyCash.Apllication.Services
{
    public class CurrencyReportService
    {
        public CurrencyReportService(IFinancialTitleRepository financialRepository)
        {
            FinancialTitleRepository = financialRepository;
        }

        public IFinancialTitleRepository FinancialTitleRepository { get; }

        public CurrencyReportModel GetCurrencyReportFrom(long clientId)
        {
            var clientFinancialTitles = FinancialTitleRepository.GetFinancialTitleFromCilentId(clientId);

            return new CurrencyReportModel(clientFinancialTitles);
        }
    }
}
