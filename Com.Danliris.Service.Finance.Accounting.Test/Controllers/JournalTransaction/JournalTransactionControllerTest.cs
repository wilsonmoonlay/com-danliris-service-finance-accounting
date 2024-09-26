using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.Controller.Utils;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.JournalTransaction;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.JournalTransaction
{
    public class JournalTransactionControllerTest : BaseControllerTest<JournalTransactionController, JournalTransactionModel, JournalTransactionViewModel, IJournalTransactionService>
    {
        [Fact]
        public void GetTransaction_ReturnOK()
        {
            var mocks = GetMocks();

            Dictionary<string, string> order = new Dictionary<string, string>();

            mocks.Service
                .Setup(f => f.ReadByDate(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<JournalTransactionModel>(new List<JournalTransactionModel>(), 1, order, new List<string>()));

            var response = GetController(mocks).GetTransaction(DateTimeOffset.UtcNow.AddDays(-7), DateTimeOffset.UtcNow, 1, 25, "{}", null, null, "{}");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void GetTransaction_ReturnInternalServerEror()
        {
            var mocks = GetMocks();

            mocks.Service
                .Setup(f => f.ReadByDate(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            var response = GetController(mocks).GetTransaction(DateTimeOffset.UtcNow.AddDays(-7), DateTimeOffset.UtcNow, 1, 25, "{}", null, null, "{}");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReport_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>())).Returns((new ReadResponse<JournalTransactionReportHeaderViewModel>(new List<JournalTransactionReportHeaderViewModel>(), 1, new Dictionary<string, string>(), new List<string>()), 0, 0));

            var response = GetController(mocks).GetReport();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void GetReport_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).GetReport();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Get_Unposted_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReadUnPostedTransactionsByPeriod(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new List<JournalTransactionModel>());

            var response = GetController(mocks).GetUnPosted(It.IsAny<string>(), It.IsAny<string>(), 0, 0);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Get_Unposted_Throws_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReadUnPostedTransactionsByPeriod(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(new Exception());

            var response = GetController(mocks).GetUnPosted(It.IsAny<string>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Get_ReferenceNo_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetAllReferenceNo(It.IsAny<string>(), It.IsAny<bool>())).Returns(new List<string>());

            var response = GetController(mocks).GetReferenceNo(It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Get_ReferenceNo_Throws_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetAllReferenceNo(It.IsAny<string>(), It.IsAny<bool>())).Throws(new Exception());

            var response = GetController(mocks).GetReferenceNo(It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Get_ReferenceType_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetAllReferenceType(It.IsAny<string>(), It.IsAny<bool>())).Returns(new List<string>());

            var response = GetController(mocks).GetReferenceType(It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Get_ReferenceType_Throws_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetAllReferenceType(It.IsAny<string>(), It.IsAny<bool>())).Throws(new Exception());

            var response = GetController(mocks).GetReferenceType(It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReportExcel_ReturnFile()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>())).Returns(new MemoryStream());

            var response = GetController(mocks).GetXls();
            Assert.NotNull(response);

            var response2 = GetController(mocks).GetXls(DateTimeOffset.UtcNow);
            Assert.NotNull(response2);

            var response3 = GetController(mocks).GetXls(null, DateTimeOffset.UtcNow);
            Assert.NotNull(response3);

            var response4 = GetController(mocks).GetXls(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
            Assert.NotNull(response4);

        }
        [Fact]
        public void GetReportExcel_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).GetXls();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ReverseJournalTransaction_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReverseJournalTransactionByReferenceNo(It.IsAny<string>())).Throws(new Exception());

            var response = await GetController(mocks).PostReverseJournalTransaction(It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task ReverseJournalTransaction_Succes()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReverseJournalTransactionByReferenceNo(It.IsAny<string>())).ReturnsAsync(1);

            var response = await GetController(mocks).PostReverseJournalTransaction(It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Success_Get_SubLedger_Reports()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetSubLedgerReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new SubLedgerReportViewModel());

            var response = await GetController(mocks).GetSubLedgerReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_ThrowException_Get_SubLedger_Reports()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetSubLedgerReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).GetSubLedgerReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Success_Get_SubLedger_Reports_Xls()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetSubLedgerReportXls(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new SubLedgerXlsFormat()
            {
                Filename = "aa",
                Result = new MemoryStream()
            });

            var response = await GetController(mocks).GetSubLedgerReportXls(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_ThrowException_Get_SubLedger_Reports_Xls()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetSubLedgerReportXls(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).GetSubLedgerReportXls(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Should_Success_Get_MonthOptions()
        {
            var mocks = GetMocks();

            var response = GetController(mocks).GetMonthOptions();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Succes_PostTransaction()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.PostTransactionAsync(It.IsAny<int>())).ReturnsAsync(1);

            var response = await GetController(mocks).PostingTransationById(It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Throw_Exception_PostTransaction()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.PostTransactionAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).PostingTransationById(It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Succes_PostTransactionById()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.PostTransactionAsync(It.IsAny<int>(), It.IsAny<JournalTransactionModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).PostingTransationById(It.IsAny<int>(), It.IsAny<JournalTransactionViewModel>());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Throw_Exception_PostTransactionById()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.PostTransactionAsync(It.IsAny<int>(), It.IsAny<JournalTransactionModel>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).PostingTransationById(It.IsAny<int>(), It.IsAny<JournalTransactionViewModel>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task Post_Many_WithoutException_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<List<JournalTransactionViewModel>>())).Verifiable();
            mocks.Service.Setup(s => s.CreateManyAsync(It.IsAny<List<JournalTransactionModel>>())).ReturnsAsync(1);

            var response = await GetController(mocks).PostMany(It.IsAny<List<JournalTransactionViewModel>>());
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        //[Fact]
        //public async Task Post_Many_ThrowServiceValidationExeption_ReturnBadRequest()
        //{
        //    var mocks = GetMocks();
        //    mocks.ValidateService.Setup(s => s.Validate(It.IsAny<List<JournalTransactionViewModel>>())).Throws(GetServiceValidationExeption());

        //    var response = await GetController(mocks).PostMany(new List<JournalTransactionViewModel>() { new JournalTransactionViewModel()});
        //    int statusCode = GetStatusCode(response);
        //    Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        //}

        [Fact]
        public async Task Post_Many_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<List<JournalTransactionViewModel>>())).Verifiable();
            mocks.Service.Setup(s => s.CreateManyAsync(It.IsAny<List<JournalTransactionModel>>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).PostMany(It.IsAny<List<JournalTransactionViewModel>>());
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetGeneralLedger_ReturnOk()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetGeneralLedgerReport(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>())).ReturnsAsync(new List<GeneralLedgerWrapperReportViewModel>());

            var response = await GetController(mocks).GetGeneralLedger(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task GetGeneralLedger_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetGeneralLedgerReport(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).GetGeneralLedger(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Success_Get_GeneralLedger_Reports_Xls()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetGeneralLedgerReportXls(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>())).ReturnsAsync(new MemoryStream());

            var response = await GetController(mocks).GetGeneralLedgerXls(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>());
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_ThrowException_Get_GeneralLedger_Reports_Xls()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetGeneralLedgerReportXls(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).GetGeneralLedgerXls(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
