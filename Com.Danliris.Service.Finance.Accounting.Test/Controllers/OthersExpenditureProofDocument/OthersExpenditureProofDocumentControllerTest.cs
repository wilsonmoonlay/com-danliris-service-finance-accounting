using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.OthersExpenditureProofDocument;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentControllerTest
    {
        private OthersExpenditureProofDocumentCreateUpdateViewModel _validCreateUpdateViewModel = new OthersExpenditureProofDocumentCreateUpdateViewModel()
        {
            AccountBankCode = "BMU",
            AccountBankId = 1,
            Date = DateTimeOffset.Now,
            Items = new List<OthersExpenditureProofDocumentCreateUpdateItemViewModel>()
            {
                new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
                {
                    COAId = 1,
                    Debit = 10,
                    Remark = "Remark"
                }
            },
            CekBgNo = "CekBgNo",
            Remark = "Remark",
            Type = "Type"
        };

        private OthersExpenditureProofDocumentCreateUpdateViewModel _invalidCreateUpdateViewModel = new OthersExpenditureProofDocumentCreateUpdateViewModel();

        private ServiceValidationException GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(_invalidCreateUpdateViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        private OthersExpenditureProofDocumentController GetController(IIdentityService identityService, IValidateService validateService, IOthersExpenditureProofDocumentService service)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            var controller = (OthersExpenditureProofDocumentController)Activator.CreateInstance(typeof(OthersExpenditureProofDocumentController), identityService, validateService, service);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        private int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        OthersExpenditureProofDocumentPDFViewModel othersExpenditureProofDocumentPDFViewModel
        {
            get
            {
                return new OthersExpenditureProofDocumentPDFViewModel()
                {
                    AccountBankId = 1,
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel()
                    {
                        AccountCOA = "AccountCOA",
                        AccountName = "AccountName",
                        AccountNumber = "AccountNumber",
                        BankCode = "BankCode",
                        BankName = "BankName",
                        Code = "Code",
                        Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Id = 1,
                            Rate = 1,
                            Symbol = "Rp"
                        },
                        Id = 1
                    },
                    CekBgNo = "CekBgNo",
                    Date = DateTimeOffset.Now,
                    DocumentNo = "DocumentNo",
                    Id = 1,
                    Items = new List<OthersExpenditureProofDocumentItemPDFViewModel>()
                    {

                    },
                    Remark = "Remark",
                    Type = "Type"
                };
            }
        }


        [Fact]
        public async Task GetById_WithoutException_ReturnOK()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetSingleByIdAsync(It.IsAny<int>())).ReturnsAsync(new OthersExpenditureProofDocumentViewModel());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetById(It.IsAny<int>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnNotFound()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetSingleByIdAsync(It.IsAny<int>())).ReturnsAsync((OthersExpenditureProofDocumentViewModel)null);

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetById(It.IsAny<int>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetById_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetSingleByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetById(It.IsAny<int>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetPDFById_WithoutException_ReturnOK()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetPDFByIdAsync(It.IsAny<int>())).ReturnsAsync(othersExpenditureProofDocumentPDFViewModel);

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetPDFById(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetPDFById_NotFound_ReturnNotFound()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetPDFByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetPDFById(It.IsAny<int>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetPDFById_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetPDFByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetPDFById(It.IsAny<int>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Get_WithoutException_ReturnOK()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetPagedListAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new OthersExpenditureProofPagedListViewModel());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Get_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetPagedListAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnCreated()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Verifiable();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.CreateAsync(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).ReturnsAsync(1);

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Post(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_WithValidationException_ReturnBadRequest()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Throws(GetServiceValidationExeption());
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Post(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Verifiable();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.CreateAsync(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).ThrowsAsync(new Exception());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Post(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Put_WithoutException_ReturnNoContent()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Verifiable();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).ReturnsAsync(1);

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Put(It.IsAny<int>(), It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Throws(GetServiceValidationExeption());
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Put(It.IsAny<int>(), It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Verifiable();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).ThrowsAsync(new Exception());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Put(It.IsAny<int>(), It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Delete_WithoutException_ReturnNoContent()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1);

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Delete(It.IsAny<int>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Delete_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Delete(It.IsAny<int>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Posting_Without_Exception()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Verifiable();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.Posting(It.IsAny<List<int>>())).ReturnsAsync("1");

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Posting(It.IsAny<List<int>>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Posting_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<OthersExpenditureProofDocumentCreateUpdateViewModel>())).Verifiable();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.Posting(It.IsAny<List<int>>())).ThrowsAsync(new Exception());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.Posting(It.IsAny<List<int>>());

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetLoader_WithoutException_ReturnOK()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetLoaderAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new OthersExpenditureProofPagedListViewModel());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetLoader("test", "test");

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetLoader_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetLoaderAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("test failed"));

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetLoader("test", "test");

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetReport_WithoutException_ReturnOK()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetReportList(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Lib.BusinessLogic.Services.OthersExpenditureProofDocument.OthersExpenditureProofDocumentReportListViewModel());

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetReport(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, "test", "test", 1, 23, "{}", "test");

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetReport_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetReportList(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("test failed"));

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetReport(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, "test", "test", 1, 23, "{}", "test");

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetReportXls_WithoutException_ReturnOK()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetReportList(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Lib.BusinessLogic.Services.OthersExpenditureProofDocument.OthersExpenditureProofDocumentReportListViewModel
                {
                    Count = 1,
                    Page = 1,
                    Size = 1,
                    Total = 1,
                    Data = new List<Lib.BusinessLogic.Services.OthersExpenditureProofDocument.OthersExpenditureProofDocumentReportViewModel> {
                    new Lib.BusinessLogic.Services.OthersExpenditureProofDocument.OthersExpenditureProofDocumentReportViewModel
                    {
                        Total=10,
                        Date = DateTimeOffset.Now
                    }
                    }
                });

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetReportXls(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, "test", "test", 1, 23, "{}", "test");

            //int statusCode = GetStatusCode(response);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetReportXls_WithException_ReturnInternalServerError()
        {
            var identityServiceMock = new Mock<IIdentityService>();
            var validateServiceMock = new Mock<IValidateService>();
            var serviceMock = new Mock<IOthersExpenditureProofDocumentService>();
            serviceMock.Setup(service => service.GetReportList(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("test failed"));

            var controller = GetController(identityServiceMock.Object, validateServiceMock.Object, serviceMock.Object);
            var response = await controller.GetReportXls(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, "test", "test", 1, 23, "{}", "test");

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}