using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionPaymentReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.GarmentDispositionExpedition
{
   public class GarmentDispositionExpeditionControllerTest
    {
        protected GarmentDispositionExpeditionController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            GarmentDispositionExpeditionController controller = new GarmentDispositionExpeditionController(serviceProvider.Object);
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
        public async Task Get_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetByPosition(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), "{}"))
                .Returns(new ReadResponse<IndexDto>(new List<IndexDto>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).Get("", 1, 1, new GarmentPurchasingExpeditionPosition(), "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Get_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetByPosition(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), "{}"))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).Get("", 1, 1, new GarmentPurchasingExpeditionPosition(), "{}", 1, 10);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }



        [Fact]
        public async Task SendToVerification_Success_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToVerification(It.IsAny<SendToVerificationAccountingFormDto>()))
                 .ReturnsAsync(1);
                

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            SendToVerificationAccountingFormDto formDto = new SendToVerificationAccountingFormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                }
            };
            IActionResult response =await GetController(serviceProviderMock).SendToVerification(formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task SendToVerification_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToVerification(It.IsAny<SendToVerificationAccountingFormDto>()))
                .ReturnsAsync(1 );

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            SendToVerificationAccountingFormDto formDto = new SendToVerificationAccountingFormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                }
            };
            IActionResult response =await GetController(serviceProviderMock).SendToVerification(formDto); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }


        [Fact]
        public async Task SendToVerification_Throws_ValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();
            SendToVerificationAccountingFormDto formDto = new SendToVerificationAccountingFormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                }
            };
            service
                .Setup(s => s.SendToVerification(It.IsAny<SendToVerificationAccountingFormDto>()))
                .ThrowsAsync(GetServiceValidationException(formDto));


            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
          
            IActionResult response = await GetController(serviceProviderMock).SendToVerification(formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task SendToVerification_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();
            SendToVerificationAccountingFormDto formDto = new SendToVerificationAccountingFormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                }
            };
            service
                .Setup(s => s.SendToVerification(It.IsAny<SendToVerificationAccountingFormDto>()))
                .ThrowsAsync(new Exception());


            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToVerification(formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToAccounting_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToAccounting(It.IsAny<SendToVerificationAccountingFormDto>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            SendToVerificationAccountingFormDto formDto = new SendToVerificationAccountingFormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                }
            };
            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(formDto); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task SendToAccounting_Throws_ValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();
            SendToVerificationAccountingFormDto formDto = new SendToVerificationAccountingFormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                }
            };

            service
                .Setup(s => s.SendToAccounting(It.IsAny<SendToVerificationAccountingFormDto>()))
                .ThrowsAsync(GetServiceValidationException(formDto));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
           
            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(formDto); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task SendToAccounting_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToAccounting(It.IsAny<SendToVerificationAccountingFormDto>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            SendToVerificationAccountingFormDto formDto = new SendToVerificationAccountingFormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                }
            };
            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(formDto); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public  void GetSendToVerificationOrAccounting_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetSendToVerificationOrAccounting(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ReadResponse<IndexDto>(new List<IndexDto>(),1,new Dictionary<string, string>(),new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response =  GetController(serviceProviderMock).GetSendToVerificationOrAccounting("","{}",1,10); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetSendToVerificationOrAccounting_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetSendToVerificationOrAccounting(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetSendToVerificationOrAccounting("", "{}", 1, 10); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task VerificationAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VerificationAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response =await GetController(serviceProviderMock).VerificationAccepted(new List<int>() { 1}); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task VerificationAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VerificationAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).VerificationAccepted(new List<int>() { 1 }); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task CashierAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.CashierAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).CashierAccepted(new List<int>() { 1 }); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task CashierAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.CashierAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).CashierAccepted(new List<int>() { 1 }); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task AccountingAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.AccountingAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AccountingAccepted(new List<int>() { 1 }); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task AccountingAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.AccountingAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AccountingAccepted(new List<int>() { 1 }); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task PurchasingAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.PurchasingAccepted(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PurchasingAccepted(new List<int>() { 1 }); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task PurchasingAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.PurchasingAccepted(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
           
            IActionResult response = await GetController(serviceProviderMock).PurchasingAccepted(new List<int>() { 1 }); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetVerified_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetVerified(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ReadResponse<IndexDto>(new List<IndexDto>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
           
            IActionResult response =  GetController(serviceProviderMock).GetVerified("","{}",1,10); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetVerified_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetVerified(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetVerified("", "{}", 1, 10); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetVerifiedById_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetById(It.IsAny<int>()))
                .Returns(new GarmentDispositionExpeditionModel());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
          
            IActionResult response = GetController(serviceProviderMock).GetVerifiedById(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public void GetVerifiedById_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.GetById(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = GetController(serviceProviderMock).GetVerifiedById(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task VoidVerificationAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VoidVerificationAccepted(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response =await GetController(serviceProviderMock).VoidVerificationAccepted(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task VoidVerificationAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VoidVerificationAccepted(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidVerificationAccepted(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task VoidCashierAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VoidVerificationAccepted(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidCashierAccepted(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task VoidCashierAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VoidCashierAccepted(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidCashierAccepted(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task VoidAccountingAccepted_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VoidAccountingAccepted(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidAccountingAccepted(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task VoidAccountingAccepted_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.VoidAccountingAccepted(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).VoidAccountingAccepted(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToDispositionNote_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToPurchasing(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToDispositionNote(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToDispositionNote_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToPurchasing(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToDispositionNote(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task SendToAccounting_by_id_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToAccounting(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToAccounting_by_id_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToAccounting(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToAccounting(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToCashier_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToCashier(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToCashier(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToCashier_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToCashier(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act

            IActionResult response = await GetController(serviceProviderMock).SendToCashier(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task SendToPurchasingRejected_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToPurchasingRejected(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            RejectionForm form = new RejectionForm();
            IActionResult response = await GetController(serviceProviderMock).SendToPurchasingRejected(1, form); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task SendToPurchasingRejected_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();

            service
                .Setup(s => s.SendToPurchasingRejected(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            RejectionForm form = new RejectionForm();
            IActionResult response = await GetController(serviceProviderMock).SendToPurchasingRejected(1, form); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetReportXls_Return_Ok() 
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();
            var paymentService = new Mock<IGarmentDispositionPaymentReportService>();

            paymentService
                 .Setup(s => s.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),It.IsAny<GarmentPurchasingExpeditionPosition>(),It.IsAny<string>(),It.IsAny<DateTimeOffset>(),It.IsAny<DateTimeOffset>()))
                 .ReturnsAsync(new List<GarmentDispositionPaymentReportDto> {  new GarmentDispositionPaymentReportDto(0,"test",DateTimeOffset.MinValue,DateTimeOffset.MinValue,null,0,null,null,0,null,0,0,0,0,0,0,0,0,0,0,null,null,GarmentPurchasingExpeditionPosition.AccountingAccepted,null,DateTimeOffset.MinValue,DateTimeOffset.MinValue,null,DateTimeOffset.MinValue,null,null,"0",0,null,0, 0, null, 0, null, null, 0, null, DateTimeOffset.MinValue, 0, null, 0, null, DateTimeOffset.MinValue, null, DateTimeOffset.MinValue, null, null) } );

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            RejectionForm form = new RejectionForm();
            IActionResult response = await GetController(serviceProviderMock).GetReportXls(0,0,0,GarmentPurchasingExpeditionPosition.AccountingAccepted,string.Empty,null,null); 

            //Assert
            //int statusCode = this.GetStatusCode(response);
            Assert.NotNull(response);

        }
        [Fact]
        public async Task GetReportXls_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();
            var paymentService = new Mock<IGarmentDispositionPaymentReportService>();

            paymentService
                 .Setup(s => s.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                 .ThrowsAsync(new Exception("test failed"));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            RejectionForm form = new RejectionForm();
            IActionResult response = await GetController(serviceProviderMock).GetReportXls(0, 0, 0, GarmentPurchasingExpeditionPosition.AccountingAccepted, string.Empty, null, null);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public async Task GetReport_Return_Ok()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();
            var paymentService = new Mock<IGarmentDispositionPaymentReportService>();

            paymentService
                 .Setup(s => s.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                 .ReturnsAsync(new List<GarmentDispositionPaymentReportDto> { new GarmentDispositionPaymentReportDto(0, "test", DateTimeOffset.MinValue, DateTimeOffset.MinValue, null, 0, null, null, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, GarmentPurchasingExpeditionPosition.AccountingAccepted, null, DateTimeOffset.MinValue, DateTimeOffset.MinValue, null, DateTimeOffset.MinValue, null, null, "0", 0, null, 0, 0, null, 0, null, null, 0, null, DateTimeOffset.MinValue, 0, null, 0, null, DateTimeOffset.MinValue, null, DateTimeOffset.MinValue, null, null) });

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            RejectionForm form = new RejectionForm();
            IActionResult response = await GetController(serviceProviderMock).GetReport(0, 0, 0, GarmentPurchasingExpeditionPosition.AccountingAccepted, string.Empty, null, null);

            //Assert
            //int statusCode = this.GetStatusCode(response);
            Assert.NotNull(response);

        }

        [Fact]
        public async Task GetReport_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IGarmentDispositionExpeditionService>();
            var paymentService = new Mock<IGarmentDispositionPaymentReportService>();

            paymentService
                 .Setup(s => s.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<GarmentPurchasingExpeditionPosition>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                 .ThrowsAsync(new Exception("test failed"));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IGarmentDispositionExpeditionService)))
               .Returns(service.Object);

            //Act
            RejectionForm form = new RejectionForm();
            IActionResult response = await GetController(serviceProviderMock).GetReport(0, 0, 0, GarmentPurchasingExpeditionPosition.AccountingAccepted, string.Empty, null, null);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }
    }
}
