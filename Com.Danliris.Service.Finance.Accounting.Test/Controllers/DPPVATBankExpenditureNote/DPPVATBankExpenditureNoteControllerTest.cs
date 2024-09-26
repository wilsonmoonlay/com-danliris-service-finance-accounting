using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteControllerTest
    {
        protected DPPVATBankExpenditureNoteController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            DPPVATBankExpenditureNoteController controller = new DPPVATBankExpenditureNoteController(serviceProvider.Object);
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

        Mock<IServiceProvider> GetServiceProvider()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
              .Setup(s => s.GetService(typeof(IIdentityService)))
              .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });

            var validateService = new Mock<IValidateService>();
            serviceProvider
              .Setup(s => s.GetService(typeof(IValidateService)))
              .Returns(validateService.Object);
            return serviceProvider;
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        protected ServiceValidationException GetServiceValidationException(dynamic dto)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }


        [Fact]
        public async Task PostCashflowType_Succes_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Create(It.IsAny<FormDto>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Post(form);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task PostCashflowType_Throws_ValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            FormDto form = new FormDto();
            service
                .Setup(s => s.Create(It.IsAny<FormDto>()))
                .ThrowsAsync(GetServiceValidationException(form));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).Post(form);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }



        [Fact]
        public async Task PostCashflowType_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Create(It.IsAny<FormDto>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Post(form);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public void Get_Succes_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DPPVATBankExpenditureNoteIndexDto>(new List<DPPVATBankExpenditureNoteIndexDto>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).Get("", 1, 10, "{}");

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public void Get_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).Get("", 1, 10, "{}");

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task Put_Succes_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();
            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel();
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>();
            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new DPPVATBankExpenditureNoteDto(model, items, details));


            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Put(1, form);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }


        [Fact]
        public async Task Put_Throws_ValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel();
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>();
            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new DPPVATBankExpenditureNoteDto(model, items, details));

            FormDto form = new FormDto();
            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .ThrowsAsync(GetServiceValidationException(form));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).Put(1, form);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(() => null);

            FormDto form = new FormDto();
            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).Put(1, form);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task Put_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel();
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>();
            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new DPPVATBankExpenditureNoteDto(model, items, details));

            FormDto form = new FormDto();
            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).Put(1, form);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task Posting_Succes_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Posting(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Posting(new List<int>());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Posting_Throws_ValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Posting(It.IsAny<List<int>>()))
                .Throws(GetServiceValidationException(new List<int>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Posting(new List<int>());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Posting_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Posting(It.IsAny<List<int>>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Posting(new List<int>());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Delete_Succes_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel();
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>();
            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new DPPVATBankExpenditureNoteDto(model, items, details));


            service
                .Setup(s => s.Delete(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Delete(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Delete_Succes_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel();
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>();
            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(() => null);


            service
                .Setup(s => s.Delete(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Delete(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }


        [Fact]
        public async Task Delete_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel();
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>();
            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new DPPVATBankExpenditureNoteDto(model, items, details));


            service
                .Setup(s => s.Delete(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = await GetController(serviceProviderMock).Delete(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public void GetById_Succes_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel();
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>();
            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new DPPVATBankExpenditureNoteDto(model, items, details));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).Get(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public void GetById_Succes_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).Get(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public void GetById_Succes_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Returns(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).Get(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }


        [Fact]
        public void GetReport_Succes_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();

            service
                .Setup(s => s.ExpenditureReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<ReportDto>());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).GetReport(1, 1, 1, 1, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddDays(1));

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        //[Fact]
        //public void GetReport_Succes_Return_InternalServerError()
        //{
        //    //Setup
        //    Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
        //    var service = new Mock<IDPPVATBankExpenditureNoteService>();

        //    service
        //        .Setup(s => s.ExpenditureReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
        //        .Throws(new Exception());

        //    serviceProviderMock
        //       .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
        //       .Returns(service.Object);

        //    //Act
        //    FormDto form = new FormDto();
        //    IActionResult response = GetController(serviceProviderMock).GetReport(1, 1, 1, 1, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddDays(1));

        //    //Assert
        //    int statusCode = this.GetStatusCode(response);
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void GeneratePdf_Succes_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();
            // , List< DPPVATBankExpenditureNoteItemModel > items, List<DPPVATBankExpenditureNoteDetailModel> details

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel("documentNo", 1, "bankAcountNumber", "bankName", "bankAccountingCode", 1, "IDR", 1, 1, "supplierName", false, "bgCheckNo", 1, DateTimeOffset.Now, "IDR", 1, 1);
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>()
            {

            };

            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>()
            {

            };
            service
                .Setup(s => s.Read( It.IsAny<int>()))
                .Returns(new DPPVATBankExpenditureNoteDto(model, items, details));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).GeneratePdf(1);

            //Assert
            //Assert
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
           
        }
        [Fact]
        public void GeneratePdf_Succes_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IDPPVATBankExpenditureNoteService>();
            // , List< DPPVATBankExpenditureNoteItemModel > items, List<DPPVATBankExpenditureNoteDetailModel> details

            DPPVATBankExpenditureNoteModel model = new DPPVATBankExpenditureNoteModel("documentNo", 1, "bankAcountNumber", "bankName", "bankAccountingCode", 1, "IDR", 1, 1, "supplierName", false, "bgCheckNo", 1, DateTimeOffset.Now, "IDR", 1, 1);
            List<DPPVATBankExpenditureNoteItemModel> items = new List<DPPVATBankExpenditureNoteItemModel>()
            {

            };

            List<DPPVATBankExpenditureNoteDetailModel> details = new List<DPPVATBankExpenditureNoteDetailModel>()
            {

            };
            service
                .Setup(s => s.Read(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IDPPVATBankExpenditureNoteService)))
               .Returns(service.Object);

            //Act
            FormDto form = new FormDto();
            IActionResult response = GetController(serviceProviderMock).GeneratePdf(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }



}
