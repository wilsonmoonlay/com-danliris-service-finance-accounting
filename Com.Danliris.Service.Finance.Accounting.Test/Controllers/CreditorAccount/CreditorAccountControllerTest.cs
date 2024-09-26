using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.CreditorAccount;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.CreditorAccount
{
    public class CreditorAccountControllerTest
    {
        protected CreditorAccountViewModel ViewModel
        {
            get { return new CreditorAccountViewModel(); }
        }

        public ServiceValidationException GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(ViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<ICreditorAccountService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<ICreditorAccountService>(), Mapper: new Mock<IMapper>());
        }

        protected CreditorAccountController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<ICreditorAccountService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            CreditorAccountController controller = new CreditorAccountController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public void GetReport_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns((new ReadResponse<DebtCardDto>(new List<DebtCardDto>(), 1, new Dictionary<string, string>(), new List<string>()), 1));

            var response = GetController(mocks).GetReport("code", 1, 2018);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void GetReport_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).GetReport("code", 1, 2018);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReportExcel_ReturnFile()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new MemoryStream());

            var response = GetController(mocks).GetXls("code", 1, 2018);
            Assert.NotNull(response);
        }

        [Fact]
        public void GetReportExcel_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).GetXls("code", 1, 2018);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void GetReportExcelNull_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).GetXls("code", 8, 2030);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task GetByUnitReceiptNote_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByUnitReceiptNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new CreditorAccountUnitReceiptNotePostedViewModel());
            
            var response = await GetController(mocks).UnitReceiptNoteGet("1", "1", "1");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task GetByUnitReceiptNote_ReturnNotFound()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByUnitReceiptNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(CreditorAccountUnitReceiptNotePostedViewModel));

            var response = await GetController(mocks).UnitReceiptNoteGet("1", "1", "1");
            Assert.Equal((int)HttpStatusCode.NotFound, GetStatusCode(response));
        }

        [Fact]
        public async Task GetByUnitReceiptNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByUnitReceiptNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).UnitReceiptNoteGet("1", "1", "1");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task PostByUnitReceiptNote_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).UnitReceiptNotePost(new List<CreditorAccountUnitReceiptNotePostedViewModel>());
            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response));
        }

        [Fact]
        public async Task PostByUnitReceiptNote_ThrowsServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<List<CreditorAccountUnitReceiptNotePostedViewModel>>())).Throws(GetServiceValidationExeption());

            var response = await GetController(mocks).UnitReceiptNotePost(new List<CreditorAccountUnitReceiptNotePostedViewModel>());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        //[Fact]
        //public async Task PostByUnitReceiptNote_ThrowException()
        //{
        //    var mocks = GetMocks();
        //    mocks.ValidateService.Setup(s => s.Validate(It.IsAny<List<CreditorAccountUnitReceiptNotePostedViewModel>>())).Verifiable();
        //    //mocks.Service.Setup(s => s.CreateFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Throws(new Exception());

        //    var response = await GetController(mocks).UnitReceiptNotePost(new List<CreditorAccountUnitReceiptNotePostedViewModel>());
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        //}

        [Fact]
        public async Task PutByUnitReceiptNote_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Verifiable();
            
            mocks.Service.Setup(f => f.UpdateFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).UnitReceiptNotePut(new CreditorAccountUnitReceiptNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByUnitReceiptNote_ThrowNotFound()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Throws(new NotFoundException());

            var response = await GetController(mocks).UnitReceiptNotePut(new CreditorAccountUnitReceiptNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByUnitReceiptNote_ThrowServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Throws(GetServiceValidationExeption());
            
            var response = await GetController(mocks).UnitReceiptNotePut(new CreditorAccountUnitReceiptNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByUnitReceiptNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Throws(new Exception());

            var response = await GetController(mocks).UnitReceiptNotePut(new CreditorAccountUnitReceiptNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task PutDeleteByUnitReceiptNote_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.DeleteFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).UnitReceiptNotePutDelete(new CreditorAccountUnitReceiptNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task PutDeleteByUnitReceiptNote_ThrowNotFound()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.DeleteFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Throws(new NotFoundException());

            var response = await GetController(mocks).UnitReceiptNotePutDelete(new CreditorAccountUnitReceiptNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutDeleteByUnitReceiptNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.DeleteFromUnitReceiptNoteAsync(It.IsAny<CreditorAccountUnitReceiptNotePostedViewModel>())).Throws(new Exception());

            var response = await GetController(mocks).UnitReceiptNotePutDelete(new CreditorAccountUnitReceiptNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }


        [Fact]
        public async Task PutByUnitPaymentOrder_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitPaymentOrderPostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromUnitPaymentOrderAsync(It.IsAny<CreditorAccountUnitPaymentOrderPostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).UnitPaymentOrderPut(new CreditorAccountUnitPaymentOrderPostedViewModel());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByUnitPaymentOrder_ThrowNotFound()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitPaymentOrderPostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromUnitPaymentOrderAsync(It.IsAny<CreditorAccountUnitPaymentOrderPostedViewModel>())).Throws(new NotFoundException());

            var response = await GetController(mocks).UnitPaymentOrderPut(new CreditorAccountUnitPaymentOrderPostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByUnitPaymentOrder_ThrowServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitPaymentOrderPostedViewModel>())).Throws(GetServiceValidationExeption());

            var response = await GetController(mocks).UnitPaymentOrderPut(new CreditorAccountUnitPaymentOrderPostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByUnitPaymentOrder_ThrowException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountUnitPaymentOrderPostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromUnitPaymentOrderAsync(It.IsAny<CreditorAccountUnitPaymentOrderPostedViewModel>())).Throws(new Exception());

            var response = await GetController(mocks).UnitPaymentOrderPut(new CreditorAccountUnitPaymentOrderPostedViewModel());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task DeleteByUnitReceiptNote_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteFromUnitReceiptNoteAsync(It.IsAny<int>())).ReturnsAsync(1);

            var response = await GetController(mocks).UnitReceiptNoteDelete(1);
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task DeleteByUnitReceiptNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteFromUnitReceiptNoteAsync(It.IsAny<int>())).Throws(new Exception());

            var response = await GetController(mocks).UnitReceiptNoteDelete(1);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }


        [Fact]
        public async Task GetByBankExpenditureNote_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByBankExpenditureNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new CreditorAccountBankExpenditureNotePostedViewModel());

            var response = await GetController(mocks).BankExpenditureNoteGet("1", "1", "1");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task GetByBankExpenditureNote_ReturnNotFound()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByBankExpenditureNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(CreditorAccountBankExpenditureNotePostedViewModel));

            var response = await GetController(mocks).BankExpenditureNoteGet("1", "1", "1");
            Assert.Equal((int)HttpStatusCode.NotFound, GetStatusCode(response));
        }

        [Fact]
        public async Task GetByBankExpenditureNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByBankExpenditureNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).BankExpenditureNoteGet("1", "1", "1");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task PostByBankExpenditureNote_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).BankExpenditureNotePost(new CreditorAccountBankExpenditureNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response));
        }

        [Fact]
        public async Task PostByBankExpenditureNote_ThrowsServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(GetServiceValidationExeption());

            var response = await GetController(mocks).BankExpenditureNotePost(new CreditorAccountBankExpenditureNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PostByBankExpenditureNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(new Exception());

            var response = await GetController(mocks).BankExpenditureNotePost(new CreditorAccountBankExpenditureNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task PostByBankExpenditureNoteList_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).BankExpenditureNoteListPost(new List<CreditorAccountBankExpenditureNotePostedViewModel>() { new CreditorAccountBankExpenditureNotePostedViewModel() });
            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response));
        }

        [Fact]
        public async Task PostByBankExpenditureNoteList_ThrowsServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(GetServiceValidationExeption());

            var response = await GetController(mocks).BankExpenditureNoteListPost(new List<CreditorAccountBankExpenditureNotePostedViewModel>() { new CreditorAccountBankExpenditureNotePostedViewModel() });
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        //[Fact]
        //public async Task PostByBankExpenditureNoteList_ThrowException()
        //{
        //    var mocks = GetMocks();
        //    mocks.ValidateService.Setup(s => s.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();
        //    mocks.Service.Setup(s => s.CreateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(new Exception());

        //    var response = await GetController(mocks).BankExpenditureNoteListPost(new List<CreditorAccountBankExpenditureNotePostedViewModel>() { new CreditorAccountBankExpenditureNotePostedViewModel() });
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        //}

        [Fact]
        public async Task PutByBankExpenditureNote_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).BankExpenditureNotePut(new CreditorAccountBankExpenditureNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByBankExpenditureNote_ThrowNotFound()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(new NotFoundException());

            var response = await GetController(mocks).BankExpenditureNotePut(new CreditorAccountBankExpenditureNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByBankExpenditureNote_ThrowServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(GetServiceValidationExeption());

            var response = await GetController(mocks).BankExpenditureNotePut(new CreditorAccountBankExpenditureNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByBankExpenditureNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(new Exception());

            var response = await GetController(mocks).BankExpenditureNotePut(new CreditorAccountBankExpenditureNotePostedViewModel());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByBankExpenditureNoteList_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).BankExpenditureNoteListPut(new List<CreditorAccountBankExpenditureNotePostedViewModel>() { new CreditorAccountBankExpenditureNotePostedViewModel() });
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByBankExpenditureNoteList_ThrowNotFound()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(new NotFoundException());

            var response = await GetController(mocks).BankExpenditureNoteListPut(new List<CreditorAccountBankExpenditureNotePostedViewModel>() { new CreditorAccountBankExpenditureNotePostedViewModel() });
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByBankExpenditureNoteList_ThrowServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(GetServiceValidationExeption());

            var response = await GetController(mocks).BankExpenditureNoteListPut(new List<CreditorAccountBankExpenditureNotePostedViewModel>() { new CreditorAccountBankExpenditureNotePostedViewModel() });
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public async Task PutByBankExpenditureNoteList_ThrowException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.UpdateFromBankExpenditureNoteAsync(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Throws(new Exception());

            var response = await GetController(mocks).BankExpenditureNoteListPut(new List<CreditorAccountBankExpenditureNotePostedViewModel>() { new CreditorAccountBankExpenditureNotePostedViewModel()});
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task DeleteByBankExpenditureNote_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteFromBankExpenditureNoteAsync(It.IsAny<int>())).ReturnsAsync(1);

            var response = await GetController(mocks).BankExpenditureNoteDelete(1);
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task DeleteByBankExpenditureNote_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteFromBankExpenditureNoteAsync(It.IsAny<int>())).Throws(new Exception());

            var response = await GetController(mocks).BankExpenditureNoteDelete(1);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task DeleteByBankExpenditureListNote_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteFromBankExpenditureNoteListAsync(It.IsAny<string>())).ReturnsAsync(1);

            var response = await GetController(mocks).BankExpenditureNoteDeleteList("code");
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task DeleteByBankExpenditureNoteList_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteFromBankExpenditureNoteListAsync(It.IsAny<string>())).Throws(new Exception());

            var response = await GetController(mocks).BankExpenditureNoteDeleteList("code");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task CreateFromCorrection_NoContent()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.CreateFromUnitPaymentCorrection(It.IsAny<CreditorAccountUnitPaymentCorrectionPostedViewModel>())).ReturnsAsync(1);

            var response = await GetController(mocks).UnitPaymentCorrectionPost(It.IsAny<CreditorAccountUnitPaymentCorrectionPostedViewModel>());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task CreateFromCorrection_ThrowsException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.CreateFromUnitPaymentCorrection(It.IsAny<CreditorAccountUnitPaymentCorrectionPostedViewModel>())).Throws(new Exception());

            var response = await GetController(mocks).UnitPaymentCorrectionPost(It.IsAny<CreditorAccountUnitPaymentCorrectionPostedViewModel>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        List<CreditorAccountViewModel> CreditorAccountViewModels
        {
            get
            {
                return new List<CreditorAccountViewModel>()
                {
                    new CreditorAccountViewModel()
                    {
                        BankExpenditureNoteNo="BankExpenditureNoteNo",
                        Currency="Currency",
                        CurrencyRate=1,
                        Date=DateTimeOffset.Now,
                        DPP=1,
                        DPPCurrency=1,
                        FinalBalance=1,
                        InvoiceNo="InvoiceNo",
                        MemoNo="MemoNo",
                        Mutation=1,
                        PaymentDuration="PaymentDuration",
                        PPN=1,
                        Products="Products",
                        Total=1,
                        UnitReceiptNoteNo="UnitReceiptNoteNo"
                    }
                };
            }
        }

        [Fact]
        public void GetPdf_Return_Success()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.GeneratePdf(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(CreditorAccountViewModels);
            mocks.Service.Setup(f => f.GetFinalBalance(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(0);

            var response =  GetController(mocks).GetPdf("",1,2020);
            Assert.NotNull(response);
        }

        [Fact]
        public void GetPdf_Return_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<CreditorAccountBankExpenditureNotePostedViewModel>())).Verifiable();

            mocks.Service.Setup(f => f.GeneratePdf(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            mocks.Service.Setup(f => f.GetFinalBalance(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).GetPdf("", 1, 2020);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
