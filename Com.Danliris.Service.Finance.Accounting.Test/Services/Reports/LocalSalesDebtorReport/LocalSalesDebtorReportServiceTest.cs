using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Reports.LocalSalesDebtorReport;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.Reports.LocalSalesDebtorReport
{
    public class LocalSalesDebtorReportServiceTest
    {
        private const string ENTITY = "LocalSalesDebtorReportService";

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
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/localSalesDebtor"))))
                .ReturnsAsync(message);

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            return serviceProvider;
        }

        private GarmentFinanceBankCashReceiptDetailLocalDataUtil _dataUtilBankCash(GarmentFinanceBankCashReceiptDetailLocalService service)
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
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"data\":[{\"salesContractNo\":null,\"localSalesContractId\":0,\"noteNo\":\"21/LBJ/00003\",\"date\":\"2021-08-31T17:00:00+00:00\",\"transactionType\":null,\"buyer\":{\"id\":63,\"code\":\"IJA\",\"name\":\"PT INDORENZA JAYA ABADI\",\"address\":null,\"npwp\":null,\"country\":null,\"nik\":null,\"kaberType\":null},\"tempo\":0,\"expenditureNo\":null,\"dispositionNo\":null,\"useVat\":false,\"remark\":null,\"isUsed\":false,\"paymentType\":null,\"amount\":55000000.0,\"items\":null,\"id\":29,\"active\":false,\"createdUtc\":\"0001 - 01 - 01T00: 00:00\",\"createdBy\":null,\"createdAgent\":null,\"lastModifiedUtc\":\"0001 - 01 - 01T00: 00:00\",\"lastModifiedBy\":null,\"lastModifiedAgent\":null,\"isDeleted\":false,\"deletedUtc\":\"0001 - 01 - 01T00: 00:00\",\"deletedBy\":null,\"deletedAgent\":null},]}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/localSalesDebtor"))))
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
            GarmentFinanceMemorialDetailLocalModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            LocalSalesDebtorReportService service = new LocalSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response = await service.GetMonitoring(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year, 7);

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
            message.Content = new StringContent("{\"data\":[{\"salesContractNo\":null,\"localSalesContractId\":0,\"noteNo\":\"21/LBJ/00003\",\"date\":\"2021-08-31T17:00:00+00:00\",\"transactionType\":null,\"buyer\":{\"id\":63,\"code\":\"IJA\",\"name\":\"PT INDORENZA JAYA ABADI\",\"address\":null,\"npwp\":null,\"country\":null,\"nik\":null,\"kaberType\":null},\"tempo\":0,\"expenditureNo\":null,\"dispositionNo\":null,\"useVat\":false,\"remark\":null,\"isUsed\":false,\"paymentType\":null,\"amount\":55000000.0,\"items\":null,\"id\":29,\"active\":false,\"createdUtc\":\"0001 - 01 - 01T00: 00:00\",\"createdBy\":null,\"createdAgent\":null,\"lastModifiedUtc\":\"0001 - 01 - 01T00: 00:00\",\"lastModifiedBy\":null,\"lastModifiedAgent\":null,\"isDeleted\":false,\"deletedUtc\":\"0001 - 01 - 01T00: 00:00\",\"deletedBy\":null,\"deletedAgent\":null},]}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/localSalesDebtor"))))
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
            GarmentFinanceMemorialDetailLocalModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            LocalSalesDebtorReportService service = new LocalSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response1 = service.GenerateExcel(1, DateTimeOffset.Now.Year);
            Assert.NotNull(response1);

            var response2 = service.GenerateExcel(2, DateTimeOffset.Now.Year);
            Assert.NotNull(response2);

            var response3 = service.GenerateExcel(3, DateTimeOffset.Now.Year);
            Assert.NotNull(response3);

            var response4 = service.GenerateExcel(4, DateTimeOffset.Now.Year);
            Assert.NotNull(response4);

            var response5 = service.GenerateExcel(5, DateTimeOffset.Now.Year);
            Assert.NotNull(response5);

            var response6 = service.GenerateExcel(6, DateTimeOffset.Now.Year);
            Assert.NotNull(response6);

            var response7 = service.GenerateExcel(7, DateTimeOffset.Now.Year);
            Assert.NotNull(response7);

            var response8 = service.GenerateExcel(8, DateTimeOffset.Now.Year);
            Assert.NotNull(response8);

            var response9 = service.GenerateExcel(9, DateTimeOffset.Now.Year);
            Assert.NotNull(response9);

            var response10 = service.GenerateExcel(10, DateTimeOffset.Now.Year);
            Assert.NotNull(response10);

            var response11 = service.GenerateExcel(11, DateTimeOffset.Now.Year);
            Assert.NotNull(response11);

            var response12 = service.GenerateExcel(12, DateTimeOffset.Now.Year);
            Assert.NotNull(response12);
        }

        [Fact]
        public async Task Should_Success_Get_Empty_DataExcel()
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
            message.Content = new StringContent("{\"data\":[]}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/localSalesDebtor"))))
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
            GarmentFinanceMemorialDetailLocalModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            LocalSalesDebtorReportService service = new LocalSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response1 = service.GenerateExcel(1, DateTimeOffset.Now.Year);
            Assert.NotNull(response1);

            var response2 = service.GenerateExcel(2, DateTimeOffset.Now.Year);
            Assert.NotNull(response2);

            var response3 = service.GenerateExcel(3, DateTimeOffset.Now.Year);
            Assert.NotNull(response3);

            var response4 = service.GenerateExcel(4, DateTimeOffset.Now.Year);
            Assert.NotNull(response4);

            var response5 = service.GenerateExcel(5, DateTimeOffset.Now.Year);
            Assert.NotNull(response5);

            var response6 = service.GenerateExcel(6, DateTimeOffset.Now.Year);
            Assert.NotNull(response6);

            var response7 = service.GenerateExcel(7, DateTimeOffset.Now.Year);
            Assert.NotNull(response7);

            var response8 = service.GenerateExcel(8, DateTimeOffset.Now.Year);
            Assert.NotNull(response8);

            var response9 = service.GenerateExcel(9, DateTimeOffset.Now.Year);
            Assert.NotNull(response9);

            var response10 = service.GenerateExcel(10, DateTimeOffset.Now.Year);
            Assert.NotNull(response10);

            var response11 = service.GenerateExcel(11, DateTimeOffset.Now.Year);
            Assert.NotNull(response11);

            var response12 = service.GenerateExcel(12, DateTimeOffset.Now.Year);
            Assert.NotNull(response12);
        }

        [Fact]
        public async Task Should_Success_Get_All_DataExcelSummary()
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
            message.Content = new StringContent("{\"data\":[{\"salesContractNo\":null,\"localSalesContractId\":0,\"noteNo\":\"21/LBJ/00003\",\"date\":\"2021-08-31T17:00:00+00:00\",\"transactionType\":null,\"buyer\":{\"id\":63,\"code\":\"IJA\",\"name\":\"PT INDORENZA JAYA ABADI\",\"address\":null,\"npwp\":null,\"country\":null,\"nik\":null,\"kaberType\":null},\"tempo\":0,\"expenditureNo\":null,\"dispositionNo\":null,\"useVat\":false,\"remark\":null,\"isUsed\":false,\"paymentType\":null,\"amount\":55000000.0,\"items\":null,\"id\":29,\"active\":false,\"createdUtc\":\"0001 - 01 - 01T00: 00:00\",\"createdBy\":null,\"createdAgent\":null,\"lastModifiedUtc\":\"0001 - 01 - 01T00: 00:00\",\"lastModifiedBy\":null,\"lastModifiedAgent\":null,\"isDeleted\":false,\"deletedUtc\":\"0001 - 01 - 01T00: 00:00\",\"deletedBy\":null,\"deletedAgent\":null},]}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/localSalesDebtor"))))
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
            GarmentFinanceMemorialDetailLocalModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            LocalSalesDebtorReportService service = new LocalSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response1 = service.GenerateExcelSummary(1, DateTimeOffset.Now.Year);
            Assert.NotNull(response1);

            var response2 = service.GenerateExcelSummary(2, DateTimeOffset.Now.Year);
            Assert.NotNull(response2);

            var response3 = service.GenerateExcelSummary(3, DateTimeOffset.Now.Year);
            Assert.NotNull(response3);

            var response4 = service.GenerateExcelSummary(4, DateTimeOffset.Now.Year);
            Assert.NotNull(response4);

            var response5 = service.GenerateExcelSummary(5, DateTimeOffset.Now.Year);
            Assert.NotNull(response5);

            var response6 = service.GenerateExcelSummary(6, DateTimeOffset.Now.Year);
            Assert.NotNull(response6);

            var response7 = service.GenerateExcelSummary(7, DateTimeOffset.Now.Year);
            Assert.NotNull(response7);

            var response8 = service.GenerateExcelSummary(8, DateTimeOffset.Now.Year);
            Assert.NotNull(response8);

            var response9 = service.GenerateExcelSummary(9, DateTimeOffset.Now.Year);
            Assert.NotNull(response9);

            var response10 = service.GenerateExcelSummary(10, DateTimeOffset.Now.Year);
            Assert.NotNull(response10);

            var response11 = service.GenerateExcelSummary(11, DateTimeOffset.Now.Year);
            Assert.NotNull(response11);

            var response12 = service.GenerateExcelSummary(12, DateTimeOffset.Now.Year);
            Assert.NotNull(response12);
        }

        [Fact]
        public async Task Should_Success_Get_Empty_DataExcelSummary()
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
            message.Content = new StringContent("{\"data\":[]}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/localSalesDebtor"))))
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
            GarmentFinanceMemorialDetailLocalModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            LocalSalesDebtorReportService service = new LocalSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response1 = service.GenerateExcelSummary(1, DateTimeOffset.Now.Year);
            Assert.NotNull(response1);

            var response2 = service.GenerateExcelSummary(2, DateTimeOffset.Now.Year);
            Assert.NotNull(response2);

            var response3 = service.GenerateExcelSummary(3, DateTimeOffset.Now.Year);
            Assert.NotNull(response3);

            var response4 = service.GenerateExcelSummary(4, DateTimeOffset.Now.Year);
            Assert.NotNull(response4);

            var response5 = service.GenerateExcelSummary(5, DateTimeOffset.Now.Year);
            Assert.NotNull(response5);

            var response6 = service.GenerateExcelSummary(6, DateTimeOffset.Now.Year);
            Assert.NotNull(response6);

            var response7 = service.GenerateExcelSummary(7, DateTimeOffset.Now.Year);
            Assert.NotNull(response7);

            var response8 = service.GenerateExcelSummary(8, DateTimeOffset.Now.Year);
            Assert.NotNull(response8);

            var response9 = service.GenerateExcelSummary(9, DateTimeOffset.Now.Year);
            Assert.NotNull(response9);

            var response10 = service.GenerateExcelSummary(10, DateTimeOffset.Now.Year);
            Assert.NotNull(response10);

            var response11 = service.GenerateExcelSummary(11, DateTimeOffset.Now.Year);
            Assert.NotNull(response11);

            var response12 = service.GenerateExcelSummary(12, DateTimeOffset.Now.Year);
            Assert.NotNull(response12);
        }

        [Fact]
        public async Task Should_Error_Get_Local_Sales_Data()
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

            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            message.Content = new StringContent("{\"data\":[{\"salesContractNo\":null,\"localSalesContractId\":0,\"noteNo\":\"21/LBJ/00003\",\"date\":\"2021-08-31T17:00:00+00:00\",\"transactionType\":null,\"buyer\":{\"id\":63,\"code\":\"IJA\",\"name\":\"PT INDORENZA JAYA ABADI\",\"address\":null,\"npwp\":null,\"country\":null,\"nik\":null,\"kaberType\":null},\"tempo\":0,\"expenditureNo\":null,\"dispositionNo\":null,\"useVat\":false,\"remark\":null,\"isUsed\":false,\"paymentType\":null,\"amount\":55000000.0,\"items\":null,\"id\":29,\"active\":false,\"createdUtc\":\"0001 - 01 - 01T00: 00:00\",\"createdBy\":null,\"createdAgent\":null,\"lastModifiedUtc\":\"0001 - 01 - 01T00: 00:00\",\"lastModifiedBy\":null,\"lastModifiedAgent\":null,\"isDeleted\":false,\"deletedUtc\":\"0001 - 01 - 01T00: 00:00\",\"deletedBy\":null,\"deletedAgent\":null},]}");

            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("garment-shipping/local-sales-notes/localSalesDebtor"))))
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
            GarmentFinanceMemorialDetailLocalModel modelMemo = _dataUtilMemo(serviceMemo, GetCurrentMethod()).GetNewData();

            GarmentFinanceBankCashReceiptDetailLocalService serviceBankCash = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            GarmentFinanceBankCashReceiptDetailLocalModel cashReceiptDetailModel = await _dataUtilBankCash(serviceBankCash).GetTestData();
            LocalSalesDebtorReportService service = new LocalSalesDebtorReportService(serviceProvider1.Object, _dbContext(GetCurrentMethod()));

            var response = await service.GetMonitoring(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year, 7);

            Assert.Empty(response);
        }

    }
}
