using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums;
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
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.PurchasingMemoTextile
{
    public class ControllerTest
    {
        protected PurchasingMemoTextileController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            var controller = new PurchasingMemoTextileController(serviceProvider.Object);
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

        Mock<IServiceProvider> GetServiceProviderMock()
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

        protected ServiceValidationException GetServiceValidationException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(new Lib.BusinessLogic.PurchasingMemoTextile.FormDto(), serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }


        [Fact]
        public void Post_Succes_Return_Created()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Create(It.IsAny<Lib.BusinessLogic.PurchasingMemoTextile.FormDto>())).Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).Post(It.IsAny<Lib.BusinessLogic.PurchasingMemoTextile.FormDto>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public void Post_Succes_Return_ValidationException()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Create(It.IsAny<Lib.BusinessLogic.PurchasingMemoTextile.FormDto>()))
                .Throws(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).Post(It.IsAny<Lib.BusinessLogic.PurchasingMemoTextile.FormDto>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void Post_Succes_Return_Exception()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Create(It.IsAny<Lib.BusinessLogic.PurchasingMemoTextile.FormDto>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).Post(It.IsAny<Lib.BusinessLogic.PurchasingMemoTextile.FormDto>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Get_Succes_Return_Ok()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new ReadResponse<Lib.BusinessLogic.PurchasingMemoTextile.IndexDto>(new List<Lib.BusinessLogic.PurchasingMemoTextile.IndexDto>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).Get();

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_Succes_Return_Exception()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).Get();

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Get_ById_Succes_Return_Ok()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new PurchasingTextileDto(1, new AccountingBookDto(1, "", ""), new MemoDetailDto(1, "", DateTimeOffset.Now, new CurrencyDto(1, "", 1)), "", new List<Lib.BusinessLogic.PurchasingMemoTextile.FormItemDto>(), ""));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).GetById(It.IsAny<int>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_ById_Succes_Return_NotFound()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<int>()))
                .Returns((PurchasingTextileDto)null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).GetById(It.IsAny<int>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void Get_ById_Succes_Return_Exception()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).GetById(It.IsAny<int>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetPDF_ById_Succes_Return_Ok()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<int>()))
                .Returns(new PurchasingTextileDto(1, new AccountingBookDto(1, "", ""), new MemoDetailDto(1, "", DateTimeOffset.Now, new CurrencyDto(1, "", 1)), "", new List<Lib.BusinessLogic.PurchasingMemoTextile.FormItemDto>() { new Lib.BusinessLogic.PurchasingMemoTextile.FormItemDto(new ChartOfAccountDto(1, "", ""), 1, 1) }, ""));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).GetPDFById(It.IsAny<int>());

            //Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void GetPDF_ById_Succes_Return_NotFound()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<int>()))
                .Returns((PurchasingTextileDto)null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).GetPDFById(It.IsAny<int>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void GetPDF_ById_Succes_Return_Exception()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var service = new Mock<IPurchasingMemoTextileService>();

            service.Setup(s => s.Read(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IPurchasingMemoTextileService)))
               .Returns(service.Object);

            //Act
            var response = GetController(serviceProviderMock).GetPDFById(It.IsAny<int>());

            //Assert
            var statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
