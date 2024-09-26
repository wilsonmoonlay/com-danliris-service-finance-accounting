using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalDebtorCard;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.Report.LocalDebtorCard
{
    public class GarmentFinanceLocalDebtorCardReportTests
    {
        private const string ENTITY = "LocalDebtorCardReportService";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            var httpClientService = new Mock<IHttpClientService>();
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"invoiceNo\":\"no\",\"amount\":1,\"balanceAmount\":1,\"date\":\"2018/10/20\",\"truckingDate\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"CustomsType\",\"BeacukaiDate\",\"BeacukaiNo\",,\"POSerialNumber\"]}}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/packing-list-for-debtor-card-now"))))
                .ReturnsAsync(message);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/garment-debitur-balances"))))
                .ReturnsAsync(message);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/packing-list-for-debtor-card"))))
                .ReturnsAsync(message);
            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());


            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            return serviceProvider;
        }


        private GarmentFinanceBankCashReceiptDetailLocalDataUtil _dataUtilBankCash(GarmentFinanceBankCashReceiptDetailLocalService service, string testname)
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

        private GarmentFinanceMemorialDetailLocalDataUtil _dataUtilMemo(GarmentFinanceMemorialDetailLocalService service, string testname)
        {
            var memorialService = new GarmentFinanceMemorialService(GetServiceProvider().Object, service.DbContext);
            var memorialDataUtil = new GarmentFinanceMemorialDataUtil(memorialService);
            return new GarmentFinanceMemorialDetailLocalDataUtil(service, memorialDataUtil);
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
        private string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        [Fact]
        public async Task Should_Success_Get_All_Data()
        {
            var serviceProviderMock = GetServiceProvider();

            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var serviceProvider1 = new Mock<IServiceProvider>();

            var httpClientService = new Mock<IHttpClientService>();
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"SalesNoteNo\":\"no\",\"amount\":1,\"balanceAmount\":1,\"Date\":\"2018/10/20\",\"truckingDate\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"CustomsType\",\"BeacukaiDate\",\"BeacukaiNo\",,\"POSerialNumber\"]}}");

            httpClientService
                 .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/finance-reports"))))
                 .ReturnsAsync(message);

            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());


            serviceProvider1
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            GarmentFinanceMemorialDetailLocalService serviceMemo = new GarmentFinanceMemorialDetailLocalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialDetailLocalModel modelMemo = await _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetTestData();
            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash, GetCurrentAsyncMethod()).GetTestData();
            GarmentFinanceLocalDebtorCardReportService service = new GarmentFinanceLocalDebtorCardReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));
            

            var response = service.GetMonitoring(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year, modelMemo.Items.First().BuyerCode, 7);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Get_All_DataExcel()
        {
            var serviceProviderMock = GetServiceProvider();

            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var serviceProvider1 = new Mock<IServiceProvider>();

            var httpClientService = new Mock<IHttpClientService>();
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"SalesNoteNo\":\"no\",\"amount\":1,\"balanceAmount\":1,\"Date\":\"2018/10/20\",\"truckingDate\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"CustomsType\",\"BeacukaiDate\",\"BeacukaiNo\",,\"POSerialNumber\"]}}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/finance-reports"))))
                .ReturnsAsync(message);

            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());


            serviceProvider1
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            GarmentFinanceMemorialDetailLocalService serviceMemo = new GarmentFinanceMemorialDetailLocalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialDetailLocalModel modelMemo = await _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetTestData();
            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash, GetCurrentAsyncMethod()).GetTestData();
            GarmentFinanceLocalDebtorCardReportService service = new GarmentFinanceLocalDebtorCardReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));


            var response = service.GenerateExcel(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year, modelMemo.Items.First().BuyerCode, 7);
            Assert.NotNull(response);
        }
    }
}
