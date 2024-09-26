using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition.Report;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentInvoicePurchasingDisposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Security.Claims;

using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.GarmentInvoicePurchasingDisposition
{
	public class GarmentInvoicePurchasingDispositionControllerTest
	{
		protected GarmentInvoicePurchasingDispositionController GetController((Moq.Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentInvoicePurchasingDispositionService> Service, Mock<IMapper> Mapper) mocks)
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);

			GarmentInvoicePurchasingDispositionController controller = new GarmentInvoicePurchasingDispositionController(mocks.IdentityService.Object, mocks.ValidateService.Object,mocks.Mapper.Object, mocks.Service.Object);
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

		public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentInvoicePurchasingDispositionService> Service, Mock<IMapper> Mapper) GetMocks()
		{
			return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IGarmentInvoicePurchasingDispositionService>(), Mapper: new Mock<IMapper>());
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
		public void GetMonitoring_Success()
		{
			var mock = GetMocks();
			mock.Service
			   .Setup(s => s.GetMonitoring(It.IsAny<string>(), It.IsAny<string>(), DateTimeOffset.Now, DateTimeOffset.Now, It.IsAny<int>()))
			   .ReturnsAsync(new List<MonitoringDispositionPayment>());
			//Act
			IActionResult response = GetController(mock).GetMonitoring(It.IsAny<string>(), It.IsAny<string>(), DateTimeOffset.Now, DateTimeOffset.Now);

			//Assert
			Assert.NotNull(response);
		}

		 
		[Fact]
		public void Should_Success_GetXls()
		{
			var mocks = GetMocks();

			mocks.Service.Setup(f => f.DownloadReportXls(It.IsAny<string>(), It.IsAny<string>(), DateTimeOffset.Now, DateTimeOffset.Now))
			   .ReturnsAsync(new MemoryStream());
			var response = GetController(mocks).GetReportAllXls(It.IsAny<string>(), It.IsAny<string>(), DateTimeOffset.Now, DateTimeOffset.Now);
			Assert.NotNull(response);

		}
		[Fact]
		public void GetLoader()
		{
			var mock = GetMocks();
			 
			mock.Service
			   .Setup(s => s.GetLoader(null,"{}"))
			   .Returns(new ReadResponse<GarmentInvoicePurchasingDispositionNoVM>(new List<GarmentInvoicePurchasingDispositionNoVM>(), 1, new Dictionary<string, string>(), new List<string>()));
			//Act
			var response =   GetController(mock).GetLoader(null, "{}");
			Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
		}

	}
}
