using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentFinance.MemorialDetailLocal;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.GarmentFinance.MemorialDetailLocal
{
    public class GarmentFinanceMemorialDetailLocalControllerTest
    {
        private GarmentFinanceMemorialDetailLocalViewModel viewModel
        {
            get
            {
                return new GarmentFinanceMemorialDetailLocalViewModel
                {
                    MemorialNo = "no",
                    MemorialId = 1,
                    MemorialDate = DateTimeOffset.Now,
                    Amount = 1,
                    InvoiceCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    Items = new List<GarmentFinanceMemorialDetailLocalItemViewModel>
                    {
                        new GarmentFinanceMemorialDetailLocalItemViewModel()
                        {
                            Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                            {
                                Id = 1,
                                Code = "code",
                                Rate = 1
                            },
                            Buyer= new Lib.ViewModels.NewIntegrationViewModel.BuyerViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name="name"
                            },
                            Amount=1,
                            LocalSalesNoteId = 1,
                            LocalSalesNoteNo = "no"
                        },
                    },
                    OtherItems = new List<GarmentFinanceMemorialDetailLocalOtherItemViewModel>
                    {
                        new GarmentFinanceMemorialDetailLocalOtherItemViewModel()
                        {
                            Remarks = "remarks",
                            TypeAmount = "DEBIT",
                            Account = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            Amount = 1,
                            Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                            {
                                Id = 1,
                                Code = "code",
                                Rate = 1
                            }
                        }
                    }
                };
            }
        }

        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentFinanceMemorialDetailLocalService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IGarmentFinanceMemorialDetailLocalService>(), Mapper: new Mock<IMapper>());
        }

        protected GarmentFinanceMemorialDetailLocalController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentFinanceMemorialDetailLocalService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                    new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentFinanceMemorialDetailLocalController controller = new GarmentFinanceMemorialDetailLocalController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
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

        private ServiceValidationException GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(this.viewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        private int GetStatusCodeGet((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentFinanceMemorialDetailLocalService> Service, Mock<IMapper> Mapper) mocks)
        {
            GarmentFinanceMemorialDetailLocalController controller = GetController(mocks);
            IActionResult response = controller.Get();

            return GetStatusCode(response);
        }

        [Fact]
        public void Get_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service
                .Setup(f => f.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<GarmentFinanceMemorialDetailLocalModel>(new List<GarmentFinanceMemorialDetailLocalModel>() { new GarmentFinanceMemorialDetailLocalModel() }, 0, new Dictionary<string, string>(), new List<string>()));
            mocks.Mapper
                .Setup(f => f.Map<List<GarmentFinanceMemorialDetailLocalViewModel>>(It.IsAny<List<GarmentFinanceMemorialDetailLocalModel>>()))
                .Returns(new List<GarmentFinanceMemorialDetailLocalViewModel>());

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

        private async Task<int> GetStatusCodePost((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentFinanceMemorialDetailLocalService> Service, Mock<IMapper> Mapper) mocks)
        {
            GarmentFinanceMemorialDetailLocalController controller = GetController(mocks);
            IActionResult response = await controller.Post(viewModel);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<GarmentFinanceMemorialDetailLocalViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<GarmentFinanceMemorialDetailLocalModel>())).ReturnsAsync(1);

            int statusCode = await GetStatusCodePost(mocks);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<GarmentFinanceMemorialDetailLocalViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<GarmentFinanceMemorialDetailLocalModel>())).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodePost(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task Post_Throws_Validation_Exception()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentFinanceMemorialDetailLocalViewModel>())).Throws(GetServiceValidationExeption());
            var mockMapper = new Mock<IMapper>();

            var mockFacade = new Mock<IGarmentFinanceMemorialDetailLocalService>();
            var mockIdentity = new Mock<IIdentityService>();
            var ViewModel = this.viewModel;
            ViewModel.MemorialDate = DateTimeOffset.MinValue;
            var controller = GetController((mockIdentity, validateMock, mockFacade, mockMapper));
            var response = await controller.Post(ViewModel);
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        private async Task<int> GetStatusCodeGetById((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentFinanceMemorialDetailLocalService> Service, Mock<IMapper> Mapper) mocks)
        {
            GarmentFinanceMemorialDetailLocalController controller = GetController(mocks);
            IActionResult response = await controller.GetById(1);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task GetById_NotNullModel_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(new GarmentFinanceMemorialDetailLocalModel());

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetById_NullModel_ReturnNotFound()
        {
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<GarmentFinanceMemorialDetailLocalViewModel>(It.IsAny<GarmentFinanceMemorialDetailLocalModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync((GarmentFinanceMemorialDetailLocalModel)null);

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

        [Fact]
        public async Task Delete_Success()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1);

            var response = await GetController(mocks).Delete(1);
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task Delete_Throws_Internal_Error()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).Throws(new Exception());

            var response = await GetController(mocks).Delete(1);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task Delete_Throws_Internal_Error_Aggregate()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).Throws(new AggregateException());

            var response = await GetController(mocks).Delete(1);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        private async Task<int> GetStatusCodePut((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentFinanceMemorialDetailLocalService> Service, Mock<IMapper> Mapper) mocks)
        {
            GarmentFinanceMemorialDetailLocalController controller = GetController(mocks);
            IActionResult response = await controller.Put(viewModel.Id, viewModel);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task Put_WithoutException_ReturnUpdated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<GarmentFinanceMemorialDetailLocalViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<GarmentFinanceMemorialDetailLocalModel>())).ReturnsAsync(1);

            int statusCode = await GetStatusCodePut(mocks);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Update_Throws_Validation_Exception()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentFinanceMemorialDetailLocalViewModel>())).Throws(GetServiceValidationExeption());
            var mockMapper = new Mock<IMapper>();

            var mockFacade = new Mock<IGarmentFinanceMemorialDetailLocalService>();
            var mockIdentity = new Mock<IIdentityService>();
            var response = await GetController((mockIdentity, validateMock, mockFacade, mockMapper)).Put(It.IsAny<int>(), viewModel);
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }


        [Fact]
        public async Task Update_Throws_Internal_Error()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.UpdateAsync(It.IsAny<int>(), It.IsAny<GarmentFinanceMemorialDetailLocalModel>())).Throws(new Exception());

            var response = await GetController(mocks).Put(1, It.IsAny<GarmentFinanceMemorialDetailLocalViewModel>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task Update_Throws_Internal_Error_Aggregate()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.UpdateAsync(It.IsAny<int>(), It.IsAny<GarmentFinanceMemorialDetailLocalModel>())).Throws(new AggregateException());

            var response = await GetController(mocks).Put(1, It.IsAny<GarmentFinanceMemorialDetailLocalViewModel>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
