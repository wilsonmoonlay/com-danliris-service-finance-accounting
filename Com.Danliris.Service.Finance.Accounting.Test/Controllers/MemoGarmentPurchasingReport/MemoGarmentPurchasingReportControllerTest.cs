using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.MemoGarmentPurchasingReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.MemoGarmentPurchasingReport;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.MemoGarmentPurchasingReport
{
    public class MemoGarmentPurchasingReportControllerTest
    {
        private MemoGarmentPurchasingViewModel ViewModel
        {
            get { return new MemoGarmentPurchasingViewModel(); }
        }

        protected ServiceValidationException GetServiceValidationExeption()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(ViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
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

        private int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        private MemoGarmentPurchasingReportController GetController(IServiceProvider serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            var controller = (MemoGarmentPurchasingReportController)Activator.CreateInstance(typeof(MemoGarmentPurchasingReportController), serviceProvider);
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

        [Fact]
        public void Get_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoGarmentPurchasingReportService>();

            serviceMock
                .Setup(service => service.ReadReportDetailBased(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ReadResponse<MemoGarmentPurchasingDetailModel>(new List<MemoGarmentPurchasingDetailModel>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoGarmentPurchasingReportService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = controller.Get();
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoGarmentPurchasingReportService>();

            serviceMock
                .Setup(service => service.ReadReportDetailBased(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoGarmentPurchasingReportService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = controller.Get();
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetPdf_WithoutException_ReturnOk()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var model = new MemoGarmentPurchasingModel();
            model.MemoGarmentPurchasingDetails = new List<MemoGarmentPurchasingDetailModel>();
            var serviceMock = new Mock<IMemoGarmentPurchasingReportService>();
            serviceMock
                .Setup(service => service.GetReportPdfData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new ReadResponse<MemoGarmentPurchasingModel>(new List<MemoGarmentPurchasingModel>(), 1, new Dictionary<string, string>(), new List<string>()));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoGarmentPurchasingReportService))).Returns(serviceMock.Object);

            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetPdf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>());
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetPdf_WithoutException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoGarmentPurchasingReportService>();
            serviceMock
                .Setup(service => service.GetReportPdfData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoGarmentPurchasingReportService))).Returns(serviceMock.Object);

            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetPdf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetPdf_WithoutException_ReturnNotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoGarmentPurchasingReportService>();
            serviceMock
                .Setup(service => service.GetReportPdfData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoGarmentPurchasingReportService))).Returns(serviceMock.Object);

            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetPdf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetXls_WithoutException_ReturnOk()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var model = new MemoGarmentPurchasingModel();
            model.MemoGarmentPurchasingDetails = new List<MemoGarmentPurchasingDetailModel>();
            var serviceMock = new Mock<IMemoGarmentPurchasingReportService>();
            serviceMock
                .Setup(service => service.GenerateExcel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new MemoryStream());

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoGarmentPurchasingReportService))).Returns(serviceMock.Object);

            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetXls(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>());
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetXls_WithoutException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoGarmentPurchasingReportService>();
            serviceMock
                .Setup(service => service.GenerateExcel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Throws(new Exception());

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoGarmentPurchasingReportService))).Returns(serviceMock.Object);

            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetPdf(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
