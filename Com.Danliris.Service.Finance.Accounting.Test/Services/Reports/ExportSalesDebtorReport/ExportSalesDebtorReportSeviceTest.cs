using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.CacheService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Rreports.ExportSalesDebtorReportController;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport.GarmentShippingPackingList;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.Reports.ExportSalesDebtorReport
{
    public class ExportSalesDebtorReportSeviceTest
    {
        private const string ENTITY = "ExportSalesDebtorReportService";
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
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtor"))))
                .ReturnsAsync(message);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/garment-debitur-balances"))))
                .ReturnsAsync(message);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtorNow"))))
                .ReturnsAsync(message);
            httpClientService
             .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garment-currencies/sales-debtor-currencies"))))
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
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });
                
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"rate\":\"14500\",\"invoiceNo\":\"no\",\"invoiceId\":\"1\",\"amount\":1,\"balanceAmount\":1,\"date\":\"2018/10/20\",\"truckingDate\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"CustomsType\",\"BeacukaiDate\",\"BeacukaiNo\",,\"POSerialNumber\"]}}");

            HttpResponseMessage messageC = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            messageC.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":{\"Rate\":14500.0,\"Uid\":\"no\",\"Date\":\"2018-10-20T17:00:00\",\"Code\":\"USD\"},\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":1,\"order\":{\"Date\":\"desc\"},\"select\":[\"Rate\"]}}");
            
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtor"))))
                .ReturnsAsync(message);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/garment-debitur-balances"))))
                .ReturnsAsync(message);
            httpClientService
              .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garment-currencies/sales-debtor-currencies"))))
              .ReturnsAsync(messageC);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtorNow"))))
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
            
            GarmentFinanceMemorialDetailService serviceMemo = new  GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialDetailModel modelMemo =  _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            BankCashReceiptDetailService serviceBankCash = new BankCashReceiptDetailService(serviceProviderMock.Object);
            BankCashReceiptDetailModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            ExportSalesDebtorReportService service = new ExportSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));
 
            var response = service.GetMonitoring(DateTimeOffset.Now.Month,DateTimeOffset.Now.Year,"IDR",7);

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
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"rate\":\"14500\",\"invoiceNo\":\"no\",\"invoiceId\":\"1\",\"amount\":1,\"balanceAmount\":1,\"date\":\"2018/10/20\",\"truckingDate\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"CustomsType\",\"BeacukaiDate\",\"BeacukaiNo\",,\"POSerialNumber\"]}}");

            HttpResponseMessage messageC = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            messageC.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":{\"Rate\":14500.0,\"Uid\":\"no\",\"Date\":\"2018-10-20T17:00:00\",\"Code\":\"USD\"},\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":1,\"order\":{\"Date\":\"desc\"},\"select\":[\"Rate\"]}}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtor"))))
                .ReturnsAsync(message);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/garment-debitur-balances"))))
                .ReturnsAsync(message);
            httpClientService
              .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garment-currencies/sales-debtor-currencies"))))
              .ReturnsAsync(messageC);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtorNow"))))
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
            GarmentFinanceMemorialDetailModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            BankCashReceiptDetailService serviceBankCash = new BankCashReceiptDetailService(serviceProviderMock.Object);
            BankCashReceiptDetailModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            ExportSalesDebtorReportService service = new ExportSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response = service.GenerateExcel(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year,"IDR");
            Assert.NotNull(response);
        }
        [Fact]
        public async Task Should_Success_Get_All_DataExcelTypeEndbalance()
        {
            var serviceProviderMock = GetServiceProvider();

            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var serviceProvider1 = new Mock<IServiceProvider>();

            var httpClientService = new Mock<IHttpClientService>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"rate\":\"14500\",\"invoiceNo\":\"no\",\"invoiceId\":\"1\",\"amount\":1,\"balanceAmount\":1,\"date\":\"2018/10/20\",\"truckingDate\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"CustomsType\",\"BeacukaiDate\",\"BeacukaiNo\",,\"POSerialNumber\"]}}");

            HttpResponseMessage messageC = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            messageC.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":{\"Rate\":14500.0,\"Uid\":\"no\",\"Date\":\"2018-10-20T17:00:00\",\"Code\":\"USD\"},\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":1,\"order\":{\"Date\":\"desc\"},\"select\":[\"Rate\"]}}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtor"))))
                .ReturnsAsync(message);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/garment-debitur-balances"))))
                .ReturnsAsync(message);
            httpClientService
              .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garment-currencies/sales-debtor-currencies"))))
              .ReturnsAsync(messageC);

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/invoices/exportSalesDebtorNow"))))
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
            GarmentFinanceMemorialDetailModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            BankCashReceiptDetailService serviceBankCash = new BankCashReceiptDetailService(serviceProviderMock.Object);
            BankCashReceiptDetailModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            ExportSalesDebtorReportService service = new ExportSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response = service.GenerateExcel(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year, "end");
            Assert.NotNull(response);
        }

    }
}