using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.Controller.Utils;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.DailyBankTransaction;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.DailyBankTransaction
{
    public class DailyBankTransactionControllerTest : BaseControllerTest<DailyBankTransactionController, DailyBankTransactionModel, DailyBankTransactionViewModel, IDailyBankTransactionService>
    {
        private async Task<int> GetStatusCodeDeleteByReferenceNo((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IDailyBankTransactionService> Service, Mock<IMapper> Mapper) mocks)
        {
            DailyBankTransactionController controller = GetController(mocks);
            IActionResult response = await controller.DeleteByReferenceNo("1");
            return GetStatusCode(response);
        }

        [Fact]
        public async Task DeleteByReferenceNo_WithoutException_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteByReferenceNoAsync(It.IsAny<string>())).ReturnsAsync(1);

            int statusCode = await GetStatusCodeDeleteByReferenceNo(mocks);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task DeleteByReferenceNo_ThrowException_ReturnInternalStatusError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteByReferenceNoAsync(It.IsAny<string>())).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodeDeleteByReferenceNo(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetReport_Without_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new ReadResponse<DailyBankTransactionModel>(new List<DailyBankTransactionModel>(), 0, new Dictionary<string, string>(), new List<string>()));
            mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            int statusCode = GetStatusCodeGetReport(mocks);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetReportDailyBankBalance_Without_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetDailyBalanceReport(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(
                 new List<DailyBalanceReportViewModel>() { new DailyBalanceReportViewModel() { AccountNumber = "", Balance = 0, BankName = "", Credit = 0, CurrencyCode = "", Debit = 0 }

                    }
            );
            //mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            var controller = GetController(mocks);
            IActionResult response = controller.GetDailyBalanceAccountBankReport(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void GetReportDailyBankBalanceCurrency_Without_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetDailyBalanceCurrencyReport(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(
                 new List<DailyBalanceCurrencyReportViewModel>() { new DailyBalanceCurrencyReportViewModel() {  Balance = 0, Credit = 0, CurrencyCode = "", Debit = 0 }

                    }
            );
            //mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            var controller = GetController(mocks);
            IActionResult response = controller.GetDailyBalanceCurrencyReport(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Success_Posting()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.Posting(It.IsAny<List<int>>())).ReturnsAsync(1);
            //mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            var controller = GetController(mocks);
            IActionResult response = await controller.Posting(It.IsAny<List<int>>());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public void GetReportDailyBankBalanceExcel_Without_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelDailyBalance(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new MemoryStream());
            //mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            var controller = GetController(mocks);
            IActionResult response = controller.GetDailyBalanceReportXls(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>());
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportDailyBankBalanceExcel_With_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelDailyBalance(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());
            //mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            var controller = GetController(mocks);
            IActionResult response = controller.GetDailyBalanceReportXls(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReportXls_Without_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelDailyBalance(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new MemoryStream());
            //mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            var controller = GetController(mocks);
            IActionResult response = controller.GetReportXls(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportXls_With_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelDailyBalance(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());
            //mocks.Mapper.Setup(f => f.Map<List<DailyBankTransactionViewModel>>(It.IsAny<List<DailyBankTransactionModel>>())).Returns(ViewModels);

            var controller = GetController(mocks);
            IActionResult response = controller.GetReportXls(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }


        public override async Task Post_WithoutException_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<DailyBankTransactionViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<DailyBankTransactionModel>())).ReturnsAsync(1);
            mocks.Service.Setup(s => s.CreateInOutTransactionAsync(It.IsAny<DailyBankTransactionModel>())).ReturnsAsync(1);
            var vm = new DailyBankTransactionViewModel()
            {
                SourceType = "Pendanaan",
                Status = "IN"
            };
            DailyBankTransactionController controller = GetController(mocks);
            IActionResult response = await controller.Post(vm);

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);

            var vm2 = new DailyBankTransactionViewModel()
            {
                SourceType = "Pendanaan",
                Status = "OUT"
            };

            IActionResult response2 = await controller.Post(vm2);

            int statusCode2 = GetStatusCode(response2);
            Assert.Equal((int)HttpStatusCode.Created, statusCode2);
        }

        private int GetStatusCodeGetReport((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IDailyBankTransactionService> Service, Mock<IMapper> Mapper) mocks)
        {
            var controller = GetController(mocks);
            IActionResult response = controller.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

            return GetStatusCode(response);
        }

        [Fact]
        public void GetReport_Return_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service
                .Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            var response = GetController(mocks).GetReport(1, 1, 2018);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReportExcel_ReturnFile()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelDailyBalance(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new MemoryStream());

            var response = GetController(mocks).GetReportXls(1, 1, 2018, "Code");
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportExcel_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelDailyBalance(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new MemoryStream());

            var response = GetController(mocks).GetReportXls(1, 1, 2018, "Code");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReportExcelNull_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelDailyBalance(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new MemoryStream());

            var response = GetController(mocks).GetReportXls(1, 8, 2030, "Code");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        List<DailyBankTransactionModel> DailyBankTransactionModels
        {
            get
            {
                return new List<DailyBankTransactionModel>()
                {
                    new DailyBankTransactionModel()
                    {
                        AccountBankAccountName="AccountBankAccountName",
                        AccountBankAccountNumber="AccountBankAccountNumber",
                        AccountBankCode="AccountBankCode",
                        AccountBankCurrencyCode="AccountBankCurrencyCode",
                        AccountBankCurrencyId=1,
                        AccountBankCurrencySymbol="AccountBankCurrencySymbol",
                        AccountBankId=1,
                        AccountBankName="AccountBankName",
                        AfterNominal=1,
                        AfterNominalValas=1,
                        BeforeNominal=1,
                        BeforeNominalValas=1,
                        BuyerCode="BuyerCode",
                        BuyerId=1,
                        BuyerName="BuyerName",
                        Code="Code",
                        Date=DateTimeOffset.Now,
                        DestinationBankAccountName="DestinationBankAccountName",
                        DestinationBankAccountNumber="DestinationBankAccountNumber",
                        DestinationBankCode="DestinationBankCode",
                        DestinationBankCurrencyCode="DestinationBankCurrencyCode",
                        DestinationBankCurrencyId=1,
                        DestinationBankCurrencySymbol="DestinationBankCurrencySymbol",
                        DestinationBankId=1,
                        DestinationBankName="DestinationBankName",
                        IsPosted=true,
                        Nominal=1,
                        NominalValas=1,
                        Receiver="Receiver",
                        ReferenceNo="ReferenceNo",
                        ReferenceType="ReferenceType",
                        Remark="Remark",
                        SourceType="SourceType",
                        Status="Status",
                        SupplierCode="SupplierCode",
                        SupplierId=1,
                        SupplierName="SupplierName",
                        TransactionNominal=1
,                    }
                };
            }
        }

        [Fact]
        public void GetReportPdf_Return_File()
        {
            //Arrange
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GeneratePdf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(DailyBankTransactionModels);
            mocks.Service.Setup(f => f.GetBeforeBalance(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            mocks.Service.Setup(f => f.GetDataAccountBank(It.IsAny<int>())).Returns("any");

            //Act
            var response = GetController(mocks).GetReportPdf(1, 1, 2018);

            //Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportPdf_Return_()
        {
            //Arrange
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GeneratePdf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(DailyBankTransactionModels);
            mocks.Service.Setup(f => f.GetBeforeBalance(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            mocks.Service.Setup(f => f.GetDataAccountBank(It.IsAny<int>())).Throws(new Exception());

            //Act
            var response = GetController(mocks).GetReportPdf(1, 1, 2018);

            //Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReportAll_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(),It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DailyBankTransactionModel>(new List<DailyBankTransactionModel>(),1,new Dictionary<string, string>(),new List<string>()));

            var response = GetController(mocks).GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void GetReportAll_Return_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("test failed"));

            var response = GetController(mocks).GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
        [Fact]
        public void GetLoader_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetLoader(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DailyBankTransactionModel>(new List<DailyBankTransactionModel>(), 1, new Dictionary<string, string>(), new List<string>()));

            var response = GetController(mocks).GetLoader(It.IsAny<string>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void GetLoader_Return_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetLoader(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("test failed"));

            var response = GetController(mocks).GetLoader(It.IsAny<string>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
        [Fact]
        public void GetReportAllXlsIn_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DailyBankTransactionModel>(new List<DailyBankTransactionModel> { new DailyBankTransactionModel() { AccountBankAccountName="test"} }, 1, new Dictionary<string, string>(), new List<string>()));

            var response = GetController(mocks).GetReportAllXlsIn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            //Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportAllXlsIn_ifDateNull_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Nullable<DateTimeOffset>>(), It.IsAny<Nullable<DateTimeOffset>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DailyBankTransactionModel>(new List<DailyBankTransactionModel> { new DailyBankTransactionModel() { AccountBankAccountName = "test" } }, 1, new Dictionary<string, string>(), new List<string>()));

            var response = GetController(mocks).GetReportAllXlsIn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), null, null, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            //Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportAllXlsIn_Return_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("test failed"));

            var response = GetController(mocks).GetReportAllXlsIn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
        [Fact]
        public void GetReportAllXlsOut_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DailyBankTransactionModel>(new List<DailyBankTransactionModel> { new DailyBankTransactionModel() { AccountBankAccountName = "test" } }, 1, new Dictionary<string, string>(), new List<string>()));

            var response = GetController(mocks).GetReportAllXlsOut(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            //Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportAllXlsOut_ifDateNull_Return_Ok()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Nullable<DateTimeOffset>>(), It.IsAny<Nullable<DateTimeOffset>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DailyBankTransactionModel>(new List<DailyBankTransactionModel> { new DailyBankTransactionModel() { AccountBankAccountName = "test" } }, 1, new Dictionary<string, string>(), new List<string>()));

            var response = GetController(mocks).GetReportAllXlsOut(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), null, null, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            //Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportAllXlsOut_Return_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("test failed"));

            var response = GetController(mocks).GetReportAllXlsOut(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
