using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition.Reports;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingExpedition;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.GarmentPurchasingExpedition
{
    public class GarmentPurchasingExpeditionControllerTest
    {
        protected GarmentPurchasingExpeditionController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
             user.Setup(u => u.Claims).Returns(claims);

            GarmentPurchasingExpeditionController controller = new GarmentPurchasingExpeditionController(serviceProvider.Object);
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
        public async Task SendToVerification_Succes_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToVerification(It.IsAny<SendToVerificationAccountingForm>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).SendToVerification(new SendToVerificationAccountingForm());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task SendToVerification_Throws_ValidationException()
        {
            //Setup
            var dto = new SendToVerificationAccountingForm();
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToVerification(It.IsAny<SendToVerificationAccountingForm>()))
                .ThrowsAsync(GetServiceValidationException(dto));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).SendToVerification(dto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task SendToVerification_Return_InternalServerError()
        {
            //Setup
            var dto = new SendToVerificationAccountingForm();
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToVerification(It.IsAny<SendToVerificationAccountingForm>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).SendToVerification(dto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToAccounting_Succes_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToAccounting(It.IsAny<SendToVerificationAccountingForm>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(new SendToVerificationAccountingForm());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task SendToAccounting_Throws_ValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();
            var dto = new SendToVerificationAccountingForm();
            service
                .Setup(s => s.SendToAccounting(It.IsAny<SendToVerificationAccountingForm>()))
                .ThrowsAsync(GetServiceValidationException(dto));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(dto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task SendToAccounting_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();
            var dto = new SendToVerificationAccountingForm();
            service
                .Setup(s => s.SendToAccounting(It.IsAny<SendToVerificationAccountingForm>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(dto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetSendToVerificationOrAccounting_Succes_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetSendToVerificationOrAccounting(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ReadResponse<IndexDto>(new List<IndexDto>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetSendToVerificationOrAccounting("", "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetSendToVerificationOrAccounting_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetSendToVerificationOrAccounting(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetSendToVerificationOrAccounting("", "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Get_Succes_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetByPosition(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<int>(), It.IsAny<int>(),It.IsAny<string>()))
                .Returns(new ReadResponse<IndexDto>(new List<IndexDto>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get("", 1, 1, GarmentPurchasingExpeditionPosition.SendToVerification, "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_Succes_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetByPosition(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get("", 1, 1, GarmentPurchasingExpeditionPosition.SendToVerification, "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task VerificationAccepted_Succes_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VerificationAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).VerificationAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }


        [Fact]
        public async Task VerificationAccepted_Succes_ReturInternal_ServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VerificationAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).VerificationAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task CashierAccepted_Succes_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.CashierAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).CashierAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task CashierAccepted_Succes_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.CashierAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).CashierAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task AccountingAccepted_Succes_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.AccountingAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).AccountingAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task AccountingAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.AccountingAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).AccountingAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task PurchasingAccepted_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.PurchasingAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).PurchasingAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task PurchasingAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.PurchasingAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act
            var ids = new List<int>() { 1, 2 };
            IActionResult response = await GetController(serviceProviderMock).PurchasingAccepted(ids);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task VoidVerificationAccepted_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VoidVerificationAccepted(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidVerificationAccepted(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task VoidVerificationAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VoidVerificationAccepted(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidVerificationAccepted(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task VoidCashierAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VoidCashierAccepted(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidCashierAccepted(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task VoidCashierAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VoidCashierAccepted(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidCashierAccepted(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task VoidAccountingAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VoidAccountingAccepted(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidAccountingAccepted(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task VoidAccountingAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.VoidAccountingAccepted(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidAccountingAccepted(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToInternalNote_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToPurchasing(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToInternalNote(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToInternalNote_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToPurchasing(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToInternalNote(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public void GetVerified_Return_Ok()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetVerified(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ReadResponse<IndexDto>(new List<IndexDto>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).GetVerified("", "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetVerified_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetVerified(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).GetVerified("", "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetVerifiedById_Return_Ok()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetById(It.IsAny<int>()))
                .Returns(new IndexDto(1, DateTimeOffset.Now, "", DateTimeOffset.Now, DateTimeOffset.Now, "", 1, "IDR"));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).GetVerifiedById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetVerifiedById_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.GetById(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).GetVerifiedById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToAccountingById_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToAccounting(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response =await GetController(serviceProviderMock).SendToAccounting(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToAccountingById_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToAccounting(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task SendToCashier_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToCashier(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToCashier(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToCashier_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToCashier(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToCashier(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToPurchasingRejected_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
                .Setup(s => s.SendToPurchasingRejected(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToPurchasingRejected(1,new RejectionForm());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToPurchasingRejected_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionService>();

            service
               .Setup(s => s.SendToPurchasingRejected(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToPurchasingRejected(1,new RejectionForm());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetReport_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionReportService>();

            service
                .Setup(s => s.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<DateTimeOffset>(),It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<GarmentPurchasingExpeditionReportViewModel>() { new GarmentPurchasingExpeditionReportViewModel()});

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionReportService)))
               .Returns(service.Object);

            //Act

            IActionResult response =  GetController(serviceProviderMock).GetReport(1,1, GarmentPurchasingExpeditionPosition.VerificationAccepted,DateTimeOffset.Now,DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        //[Fact]
        //public void GetReport_Return_InternalServerError()
        //{
        //    //Setup
        //    Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
        //    var service = new Mock<IGarmentPurchasingExpeditionReportService>();

        //    service
        //        .Setup(s => s.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
        //        .Throws(new Exception());

        //    serviceProviderMock
        //       .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionReportService)))
        //       .Returns(service.Object);

        //    //Act

        //    IActionResult response = GetController(serviceProviderMock).GetReport(1, 1, GarmentPurchasingExpeditionPosition.VerificationAccepted, DateTimeOffset.Now, DateTimeOffset.Now);

        //    //Assert
        //    int statusCode = this.GetStatusCode(response);
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void GetReportXls_Return_File()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionReportService>();

            service
                .Setup(s => s.GenerateExcel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new System.IO.MemoryStream());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionReportService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).GetReportXls(1, 1, GarmentPurchasingExpeditionPosition.VerificationAccepted, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }

        [Fact]
        public void GetReportXls_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentPurchasingExpeditionReportService>();

            service
                .Setup(s => s.GenerateExcel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentPurchasingExpeditionReportService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).GetReportXls(1, 1, GarmentPurchasingExpeditionPosition.VerificationAccepted, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }


 

}
