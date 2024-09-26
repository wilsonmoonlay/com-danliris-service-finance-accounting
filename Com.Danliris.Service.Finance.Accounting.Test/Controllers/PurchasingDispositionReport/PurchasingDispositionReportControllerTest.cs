using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.PurchasingDispositionReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.PurchasingDispositionReport
{
    public class PurchasingDispositionReportControllerTest
    {
        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IPurchasingDispositionExpeditionService>(), Mapper: new Mock<IMapper>());
        }

        protected PurchasingDispositionReportController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            PurchasingDispositionReportController controller = new PurchasingDispositionReportController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
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
        public async Task GetReport_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new ReadResponse<PurchasingDispositionReportViewModel>(new List<PurchasingDispositionReportViewModel>(), 1, new Dictionary<string, string>(), new List<string>()));

            var response = await GetController(mocks).GetReportAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, It.IsAny<int>(), It.IsAny<int>(), "{}", "{}");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task GetReport_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).GetReportAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),It.IsAny<int>(), It.IsAny<int>(),"{}","{}");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
 
        [Fact]
        public async Task GetReportExcel_ReturnFile()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new MemoryStream());

            var response = await GetController(mocks).GetXlsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), "{}", It.IsAny<DateTime>(), It.IsAny<DateTime>());
            Assert.NotNull(response);

            //var response2 = await GetController(mocks).GetXlsAsync("{}", DateTime.UtcNow);
            //Assert.NotNull(response2);
            //var response3 = await GetController(mocks).GetXlsAsync("{}", null, DateTime.UtcNow);
            //Assert.NotNull(response3);
            //var response4 = await GetController(mocks).GetXlsAsync("{}", DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
            //Assert.NotNull(response4);
        }
        [Fact]
        public async Task GetReportExcel_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await GetController(mocks).GetXlsAsync( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),"{}", It.IsAny<DateTime>(), It.IsAny<DateTime>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
