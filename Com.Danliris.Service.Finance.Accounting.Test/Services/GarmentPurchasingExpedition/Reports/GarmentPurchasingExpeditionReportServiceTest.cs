using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition.Reports;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentPurchasingExpedition.Reports
{
    public class GarmentPurchasingExpeditionReportServiceTest
    {
        private const string Entity = "GarmentPurchasingExpeditions";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", Entity);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { TimezoneOffset = 0, Token = "token", Username = "username" });

            return serviceProviderMock;
        }

        private FinanceDbContext GetDbContext(string testName)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider);

            return new FinanceDbContext(optionsBuilder.Options);
        }

        [Fact]
        public void Should_Success_Generate_Excel_With_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            var expedition = new GarmentPurchasingExpeditionModel();
            expedition.SendToVerification("Test");
            EntityExtension.FlagForCreate(expedition, "Test", "Test");
            dbContext.GarmentPurchasingExpeditions.Add(expedition);
            dbContext.SaveChanges();

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
               .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionReportService(serviceProviderMock.Object);

            var reportResponse = service.GenerateExcel(0, 0, GarmentPurchasingExpeditionPosition.SendToVerification, DateTimeOffset.Now.AddDays(-7), DateTimeOffset.Now.AddDays(7), DateTimeOffset.Now.AddDays(-7), DateTimeOffset.Now.AddDays(7));

            Assert.NotNull(reportResponse);
        }

        [Fact]
        public void Should_Success_Generate_Excel()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
               .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionReportService(serviceProviderMock.Object);

            var reportResponse = service.GenerateExcel(1, 1, GarmentPurchasingExpeditionPosition.VerificationAccepted, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);
            var reportResponse2 = service.GenerateExcel(1, 1, GarmentPurchasingExpeditionPosition.SendToCashier, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);
            
            Assert.NotNull(reportResponse);
            Assert.NotNull(reportResponse2);
        }

        [Fact]
        public void Should_Success_Get_Report()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
               .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionReportService(serviceProviderMock.Object);

            var reportResponse = service.GetReport(1, 1, 0, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);
            var reportResponse2 = service.GetReport(1, 1, GarmentPurchasingExpeditionPosition.CashierAccepted, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);
            
            Assert.NotNull(reportResponse);
            Assert.NotNull(reportResponse2);
        }

        [Fact]
        public void Should_Success_Get_Report_ViewModel()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
               .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionReportService(serviceProviderMock.Object);

            var reportResponse = service.GetReportViewModel(1, 1, GarmentPurchasingExpeditionPosition.Purchasing, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);
            var reportResponse2 = service.GetReportViewModel(1, 1, GarmentPurchasingExpeditionPosition.AccountingAccepted, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);

            Assert.NotNull(reportResponse);
            Assert.NotNull(reportResponse2);
        }
    }
}
