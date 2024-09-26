using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalBankCashReceiptMonthlyRecap;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.Report.LocalBankCashReceiptMonthlyRecap
{
    public class GarmentFinanceLocalBankCashReceiptMonthlyRecapServiceTest
    {
        private string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

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

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        private GarmentFinanceBankCashReceiptDetailLocalDataUtil _dataUtil(GarmentFinanceBankCashReceiptDetailLocalService service, string testname)
        {
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var bankCashReceiptService = new BankCashReceiptService(serviceProviderMock.Object);
            var bankCashReceiptDataUtil = new BankCashReceiptDataUtil(bankCashReceiptService);
            return new GarmentFinanceBankCashReceiptDetailLocalDataUtil(service, bankCashReceiptDataUtil);
        }

        private BankCashReceiptDataUtil _dataUtilReceipt(BankCashReceiptService service, string testname)
        {
            return new BankCashReceiptDataUtil(service);
        }

        [Fact]
        public async Task Should_Success_Get_All_Data()
        {
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            GarmentFinanceLocalBankCashReceiptMonthlyRecapService serviceReport = new GarmentFinanceLocalBankCashReceiptMonthlyRecapService(serviceProviderMock.Object, dbContext);


            var response = serviceReport.GetMonitoring(DateTimeOffset.Now.AddDays(-3), DateTimeOffset.Now.AddDays(3), 7);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Get_All_Data_Null_Date()
        {
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            GarmentFinanceLocalBankCashReceiptMonthlyRecapService serviceReport = new GarmentFinanceLocalBankCashReceiptMonthlyRecapService(serviceProviderMock.Object, dbContext);


            var response = serviceReport.GetMonitoring(null, null, 7);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Get_All_DataExcel()
        {
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            GarmentFinanceLocalBankCashReceiptMonthlyRecapService serviceReport = new GarmentFinanceLocalBankCashReceiptMonthlyRecapService(serviceProviderMock.Object, dbContext);


            var response = serviceReport.GenerateExcel(DateTimeOffset.Now.AddDays(-3), DateTimeOffset.Now.AddDays(3), 7);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Get_All_DataExcel_ZeroData()
        {
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            GarmentFinanceLocalBankCashReceiptMonthlyRecapService serviceReport = new GarmentFinanceLocalBankCashReceiptMonthlyRecapService(serviceProviderMock.Object, dbContext);


            var response = serviceReport.GenerateExcel(DateTimeOffset.Now.AddDays(-3), DateTimeOffset.Now.AddDays(-3), 7);
            Assert.NotNull(response);
        }
    }
}
