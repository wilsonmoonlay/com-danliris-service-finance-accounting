using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Test.Controller.Utils;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.PurchasingDispositionExpedition;
using Com.Moonlay.NetCore.Lib.Service;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionControllerTest
    {
        private PurchasingDispositionExpeditionViewModel ViewModel
        {
            get
            {
                return new PurchasingDispositionExpeditionViewModel
                {
                    supplier = new SupplierViewModel(),
                    incomeTaxVm = new IncomeTaxViewModel(),
                    currency = new CurrencyViewModel(),
                    category = new CategoryViewModel(),
                    items = new List<PurchasingDispositionExpeditionItemViewModel>
                    {
                        new PurchasingDispositionExpeditionItemViewModel()
                        {
                            product = new ProductViewModel(),
                            unit = new UnitViewModel(),
                        }
                    }
                };
            }
        }
        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IPurchasingDispositionExpeditionService>(), Mapper: new Mock<IMapper>());
        }

        protected PurchasingDispositionExpeditionController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                    new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            PurchasingDispositionExpeditionController controller = new PurchasingDispositionExpeditionController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
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

        private ServiceValidationExeption GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(this.ViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        private int GetStatusCodeGet((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) mocks)
        {
            PurchasingDispositionExpeditionController controller = GetController(mocks);
            IActionResult response = controller.Get();

            return GetStatusCode(response);
        }

        [Fact]
        public void Get_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new ReadResponse<PurchasingDispositionExpeditionModel>(new List<PurchasingDispositionExpeditionModel>(), 0, new Dictionary<string, string>(), new List<string>()));
            mocks.Mapper.Setup(f => f.Map<List<PurchasingDispositionExpeditionViewModel>>(It.IsAny<List<PurchasingDispositionExpeditionModel>>())).Returns(new List<PurchasingDispositionExpeditionViewModel>());

            var response = GetController(mocks).Get();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Get_ReadThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            int statusCode = GetStatusCodeGet(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        private async Task<int> GetStatusCodeGetById((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) mocks)
        {
            PurchasingDispositionExpeditionController controller = GetController(mocks);
            IActionResult response = await controller.GetById(1);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task GetById_NotNullModel_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(new PurchasingDispositionExpeditionModel());

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetById_NullModel_ReturnNotFound()
        {
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<PurchasingDispositionExpeditionViewModel>(It.IsAny<PurchasingDispositionExpeditionModel>())).Returns(ViewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync((PurchasingDispositionExpeditionModel)null);

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetById_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        private async Task<int> GetStatusCodePost((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) mocks)
        {
            PurchasingDispositionExpeditionController controller = GetController(mocks);
            IActionResult response = await controller.Post(new List<PurchasingDispositionExpeditionViewModel> { this.ViewModel});

            return GetStatusCode(response);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<PurchasingDispositionExpeditionViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<PurchasingDispositionExpeditionModel>())).ReturnsAsync(1);

            int statusCode = await GetStatusCodePost(mocks);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<PurchasingDispositionExpeditionViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<PurchasingDispositionExpeditionModel>())).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodePost(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task Post_ThrowException_ServiceValidationException()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<PurchasingDispositionExpeditionViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<PurchasingDispositionExpeditionModel>())).ThrowsAsync(GetServiceValidationExeption());

            int statusCode = await GetStatusCodePost(mocks);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        private async Task<int> GetStatusCodeDeleteByReferenceNo((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IPurchasingDispositionExpeditionService> Service, Mock<IMapper> Mapper) mocks)
        {
            PurchasingDispositionExpeditionController controller = GetController(mocks);
            IActionResult response = await controller.Delete(1);
            return GetStatusCode(response);
        }

        [Fact]
        public async Task DeleteByReferenceNo_WithoutException_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1);

            int statusCode = await GetStatusCodeDeleteByReferenceNo(mocks);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task DeleteByReferenceNo_ThrowException_ReturnInternalStatusError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodeDeleteByReferenceNo(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


		[Fact]
		public void GetBankExpenditureNoReturnOK()
		{
			var mocks = GetMocks();
			mocks.Service.Setup(f => f.ReadBankExpenditureNoteNo(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new ReadResponse<PurchasingDispositionExpeditionModel>(new List<PurchasingDispositionExpeditionModel>(), 0, new Dictionary<string, string>(), new List<string>()));
			mocks.Mapper.Setup(f => f.Map<List<PurchasingDispositionExpeditionViewModel>>(It.IsAny<List<PurchasingDispositionExpeditionModel>>())).Returns(new List<PurchasingDispositionExpeditionViewModel>());

			var response = GetController(mocks).GetLoaderBankExpenditureNoteNo();
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
		}

		[Fact]
		public void GetBankExpenditureNoReturnInternalservererror()
		{
			var mocks = GetMocks();
			mocks.Service.Setup(f => f.ReadBankExpenditureNoteNo(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

			mocks.Mapper.Setup(f => f.Map<List<PurchasingDispositionExpeditionViewModel>>(It.IsAny<List<PurchasingDispositionExpeditionModel>>())).Returns(new List<PurchasingDispositionExpeditionViewModel>());

			var response = GetController(mocks).GetLoaderBankExpenditureNoteNo();
			Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
		}
	}
}
