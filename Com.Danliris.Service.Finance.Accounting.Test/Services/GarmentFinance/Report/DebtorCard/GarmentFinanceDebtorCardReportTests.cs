using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.DebtorCard;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.MemorialDetail;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.Report.DebtorCard
{
    public class GarmentFinanceDebtorCardReportTests
    {
        private const string ENTITY = "DebtorCardReportService";
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


        private BankCashReceiptDetailDataUtil _dataUtilBankCash(BankCashReceiptDetailService service)
        {
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var bankCashReceiptService = new BankCashReceiptService(serviceProviderMock.Object);
            var bankCashReceiptDataUtil = new BankCashReceiptDataUtil(bankCashReceiptService);
            return new BankCashReceiptDetailDataUtil(service, bankCashReceiptDataUtil);
        }
        private GarmentFinanceMemorialDetailDataUtil _dataUtilMemo(GarmentFinanceMemorialDetailService service, string testname)
        {
            var memorialService = new GarmentFinanceMemorialService(GetServiceProvider().Object, service.DbContext);
            var memorialDataUtil = new GarmentFinanceMemorialDataUtil(memorialService);
            return new GarmentFinanceMemorialDetailDataUtil(service, memorialDataUtil);
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
            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());


            serviceProvider1
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            GarmentFinanceMemorialDetailService serviceMemo = new GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialDetailModel modelMemo = await _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetTestData();
            BankCashReceiptDetailService serviceBankCash = new BankCashReceiptDetailService(serviceProviderMock.Object);
            BankCashReceiptDetailModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            GarmentFinanceDebtorCardReportService service = new GarmentFinanceDebtorCardReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));
            

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
            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());


            serviceProvider1
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            serviceProvider1
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            GarmentFinanceMemorialDetailService serviceMemo = new GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialDetailModel modelMemo = await _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetTestData();
            BankCashReceiptDetailService serviceBankCash = new BankCashReceiptDetailService(serviceProviderMock.Object);
            BankCashReceiptDetailModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            GarmentFinanceDebtorCardReportService service = new GarmentFinanceDebtorCardReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));


            var response = service.GenerateExcel(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year, modelMemo.Items.First().BuyerCode, 7);
            Assert.NotNull(response);
        }
    }
}
