using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WyCash.Domain;

namespace WyCashTests.Domain
{
    [TestClass]
    public class CurrencyReportServiceTest
    {
        [TestMethod]
        public void Should_Be_Possible_To_Get_A_Empty_Currency_Report()
        {
            long clientId = 0;

            CurrencyReportService currencyReportService = new CurrencyReportService(Mock.Of<IFinancialTitleRepository>());

            CurrencyReportModel currencyReport = currencyReportService.GetCurrencyReportFrom(clientId);

            Assert.AreEqual(0, currencyReport.FinancialTitles.Count());
        }

        [TestMethod]
        public void Should_Be_Possible_To_Get_A_Currency_Report_When_The_Client_Have_Financial_Titles()
        {
            long clientId = 0;
            
            Mock<IFinancialTitleRepository> financialTitleRepositoryMock = new Mock<IFinancialTitleRepository>();
            
            IEnumerable<FinancialTitle> clientFinancialTitle = new List<FinancialTitle> 
            { 
                new FinancialTitle
                {
                    Name = "IBM",
                    Quantity = 1000,
                    Valuation = 25,
                }
            };

            financialTitleRepositoryMock
                .Setup(r => r.GetFinancialTitleFromCilentId(clientId))
                .Returns(clientFinancialTitle);

            CurrencyReportService currencyReportService = new CurrencyReportService(financialTitleRepositoryMock.Object);


            CurrencyReportModel currencyReport = currencyReportService.GetCurrencyReportFrom(clientId);


            Assert.AreEqual(1, currencyReport.FinancialTitles.Count());
            Assert.AreEqual("IBM", currencyReport.FinancialTitles.ElementAt(0).Name);
            Assert.AreEqual(1000, currencyReport.FinancialTitles.ElementAt(0).Quantity);
            Assert.AreEqual(25, currencyReport.FinancialTitles.ElementAt(0).Valuation);
            Assert.AreEqual(25000, currencyReport.FinancialTitles.ElementAt(0).TotalValue);
        }
    }
}