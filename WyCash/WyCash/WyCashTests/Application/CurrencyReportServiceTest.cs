using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WyCash.Apllication.Model;
using WyCash.Apllication.Repositories;
using WyCash.Apllication.Services;
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
                    Money = Money.Dollar
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
            Assert.AreEqual(25000, currencyReport.TotalAtDollar);
        }

        [TestMethod]
        public void Should_Be_Possible_To_Get_A_Currency_Report_When_The_Client_Have_Financial_Titles_With_Diferents_Currencys()
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
                    Money = Money.Dollar
                },
                new FinancialTitle
                {
                    Name = "Novartis",
                    Quantity = 400,
                    Valuation = 150,
                    Money = Money.SwissFranc
                }
            };

            financialTitleRepositoryMock
                .Setup(r => r.GetFinancialTitleFromCilentId(clientId))
                .Returns(clientFinancialTitle);

            CurrencyReportService currencyReportService = new CurrencyReportService(financialTitleRepositoryMock.Object);


            CurrencyReportModel currencyReport = currencyReportService.GetCurrencyReportFrom(clientId);


            Assert.AreEqual(2, currencyReport.FinancialTitles.Count());

            Assert.AreEqual("IBM", currencyReport.FinancialTitles.ElementAt(0).Name);
            Assert.AreEqual(1000, currencyReport.FinancialTitles.ElementAt(0).Quantity);
            Assert.AreEqual(25, currencyReport.FinancialTitles.ElementAt(0).Valuation);
            Assert.AreEqual(25000, currencyReport.FinancialTitles.ElementAt(0).TotalValue);

            Assert.AreEqual("Novartis", currencyReport.FinancialTitles.ElementAt(1).Name);
            Assert.AreEqual(400, currencyReport.FinancialTitles.ElementAt(1).Quantity);
            Assert.AreEqual(150, currencyReport.FinancialTitles.ElementAt(1).Valuation);
            Assert.AreEqual(60000, currencyReport.FinancialTitles.ElementAt(1).TotalValue);

            Assert.AreEqual(65000, currencyReport.TotalAtDollar);
        }
    }
}