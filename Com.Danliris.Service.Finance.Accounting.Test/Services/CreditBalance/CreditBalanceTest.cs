using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.CreditBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.CreditBalance
{
    public class CreditBalanceTest
    {
        private const string ENTITY = "CreditBalance";
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

        private CreditorAccountDataUtil _dataUtil(CreditorAccountService service)
        {

            GetServiceProvider();
            return new CreditorAccountDataUtil(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });


            return serviceProvider;
        }

        [Fact]
        public async Task Should_Success_Get_Report()
        {
            //Arrange
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            //Act
            var reportResponse = creditBalanceService.GetReport(false, 1, 25, "", data.Date.Month, data.Date.Year, 7, false, 11);

            //Assert
            Assert.NotNull(reportResponse.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Report_ForeignCurrency()
        {
            //Arrange
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            unitData.Currency = "USD";
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            data.InvoiceNo = unitData.InvoiceNo;
            
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            //Act
            var reportResponse = creditBalanceService.GetReport(true, 1, 25, data.SupplierName, data.Date.Month, data.Date.Year, 7, true, unitData.DivisionId);

            //Assert
            Assert.NotEmpty(reportResponse.Data);
        }


        //[Fact]
        //public async Task Should_Success_Get_Report_January()
        //{
        //    //Arrange
        //    CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
        //    var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
        //    unitData.Date = new DateTimeOffset(unitData.Date.Year, 1, unitData.Date.Day, unitData.Date.Hour, unitData.Date.Minute, unitData.Date.Second, unitData.Date.Offset);
        //    data.SupplierCode = unitData.SupplierCode;
        //    data.SupplierName = unitData.SupplierName;
        //    data.InvoiceNo = unitData.InvoiceNo;
        //    var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
        //    var Response = await service.CreateFromBankExpenditureNoteAsync(data);

        //    //Act
        //    var reportResponse = creditBalanceService.GetReport(true, 1, 25, unitData.SupplierName, 1, unitData.Date.Year, 7, false, unitData.DivisionId);
            
        //    //Assert
        //    Assert.NotEmpty(reportResponse.Data);
        //}

        [Fact]
        public async Task Should_Success_Get_Excel_Impor()
        {
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            var reportResponse = creditBalanceService.GenerateExcel(true, data.SupplierName, data.Date.Month, data.Date.Year, 7, false, 11);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Get_Excel_Lokal()
        {
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            unitData.SupplierIsImport = false;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            var reportResponse = creditBalanceService.GenerateExcel(false, data.SupplierName, data.Date.Month, data.Date.Year, 7, false, 11);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Get_Excel_Empty_Lokal()
        {
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            unitData.SupplierIsImport = false;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            var reportResponse = creditBalanceService.GenerateExcel(false, "", data.Date.AddMonths(1).Month, data.Date.AddYears(1).Year, 7, false, 11);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Get_Excel_Empty_Impor()
        {
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            unitData.SupplierIsImport = false;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            var reportResponse = creditBalanceService.GenerateExcel(true, "", data.Date.Month, data.Date.Year, 7, false, 11);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public void Should_Success_Get_Excel_Empty_Local()
        {
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var reportResponse = creditBalanceService.GenerateExcel(false, "", 1, 2020, 7, false, 11);
            Assert.NotNull(reportResponse);
        }


        [Fact]
        public async Task Should_Success_GeneratePdf()
        {
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            var reportResponse = creditBalanceService.GeneratePdf(true, data.SupplierName, data.Date.Month, data.Date.Year, 7, true, unitData.DivisionId);
            Assert.True(0 <= reportResponse.Count());
            Assert.NotEmpty(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Get_Report_Detail()
        {
            //Arrange
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);

            //Act
            var reportResponse = creditBalanceService.GetReportDetail(false, data.SupplierCode, data.Date.Month, data.Date.Year, 7, false, unitData.DivisionId);

            //Assert
            Assert.NotNull(reportResponse.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Excel_Detail()
        {
            CreditorAccountService service = new CreditorAccountService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            CreditBalanceService creditBalanceService = new CreditBalanceService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service).GetBankExpenditureNotePostedViewModel();
            var unitData = _dataUtil(service).GetNewData_UnitReceiptNotePostedViewModel();
            data.SupplierCode = unitData.SupplierCode;
            data.SupplierName = unitData.SupplierName;
            data.InvoiceNo = unitData.InvoiceNo;
            var tempResponse = await service.CreateFromUnitReceiptNoteAsync(unitData);
            var Response = await service.CreateFromBankExpenditureNoteAsync(data);
            var detail = creditBalanceService.GetReportDetail(false, data.SupplierCode, data.Date.Month, data.Date.Year, 7, false, unitData.DivisionId);

            var reportResponse = creditBalanceService.GenerateExcelDetail(detail, unitData.DivisionId, data.Date.Month, data.Date.Year);
            Assert.NotNull(reportResponse);
        }
    }
}
