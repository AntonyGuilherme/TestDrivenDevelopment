
using System;
using System.Collections.Generic;

namespace WyCash.Domain
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

    public class CurrencyReportModel
    {
        public CurrencyReportModel(IEnumerable<FinancialTitle> financialTitles)
        {
            FinancialTitles = financialTitles;
        }

        public IEnumerable<FinancialTitle> FinancialTitles { get; set; }
    }

    public interface IFinancialTitleRepository
    {
        IEnumerable<FinancialTitle> GetFinancialTitleFromCilentId(long clientId);
    }

    public class FinancialTitle
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Valuation { get; set; }
        public long TotalValue => Quantity * Valuation;
    }
}
