using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentFinance.BankCashReceipt;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptControllerTest
    {
        private BankCashReceiptViewModel viewModel
        {
            get
            {
                return new BankCashReceiptViewModel
                {
                    ReceiptNo = "no",
                    Amount = 12345.00M,
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel
                    {
                        Id = 1,
                        AccountName = "accountName",
                        AccountCOA = "accountCOA",
                        AccountNumber = "accountNumber",
                        BankCode = "bankCode",
                        BankName = "bankName",
                        Code = "code",
                        Currency = null
                    },
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                    {
                        Id = 1,
                        Code = "IDR",
                        Rate = 1,
                        Description = "description",
                        Symbol = "symbol",
                    },
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    IncomeType = "incomeType",
                    ReceiptDate = DateTimeOffset.Now,
                    NumberingCode = "numberingCode",
                    Remarks = "remarks",
                    BankCashReceiptType= new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id=1,
                        Name="PENJUALAN EKSPOR",
                    },
                    Buyer= new Lib.ViewModels.NewIntegrationViewModel.NewBuyerViewModel
                    {
                        Id=1,
                        Name="name"
                    },
                    Items = new List<BankCashReceiptItemViewModel>
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            Summary = 1,
                        }
                    }
                };
            }
        }

        private BankCashReceiptViewModel viewModelWithDecimal
        {
            get
            {
                return new BankCashReceiptViewModel
                {
                    ReceiptNo = "no",
                    Amount = 12345.12M,
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel
                    {
                        Id = 1,
                        AccountName = "accountName",
                        AccountCOA = "accountCOA",
                        AccountNumber = "accountNumber",
                        BankCode = "bankCode",
                        BankName = "bankName",
                        Code = "code",
                        Currency = null
                    },
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                    {
                        Id = 1,
                        Code = "IDR",
                        Rate = 1,
                        Description = "description",
                        Symbol = "symbol",
                    },
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    IncomeType = "incomeType",
                    ReceiptDate = DateTimeOffset.Now,
                    NumberingCode = "numberingCode",
                    Remarks = "remarks",
                    BankCashReceiptType= new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id=1,
                        Name="name"
                    },
                    Items = new List<BankCashReceiptItemViewModel>
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            Summary = 1,
                        }
                    }
                };
            }
        }

        private BankCashReceiptViewModel viewModelDollarZeroDecimal
        {
            get
            {
                return new BankCashReceiptViewModel
                {
                    ReceiptNo = "no",
                    Amount = 120000.00M,
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel
                    {
                        Id = 1,
                        AccountName = "accountName",
                        AccountCOA = "accountCOA",
                        AccountNumber = "accountNumber",
                        BankCode = "bankCode",
                        BankName = "bankName",
                        Code = "code",
                        Currency = null
                    },
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                    {
                        Id = 1,
                        Code = "USD",
                        Rate = 1,
                        Description = "description",
                        Symbol = "symbol",
                    },
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    BankCashReceiptType= new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id=2,
                        Name="PENJUALAN LOKAL"
                    },
                    Buyer= new Lib.ViewModels.NewIntegrationViewModel.NewBuyerViewModel
                    {
                        Id=1,
                        Name="nama"
                    },
                    IncomeType = "incomeType",
                    ReceiptDate = DateTimeOffset.Now,
                    NumberingCode = "numberingCode",
                    Remarks = "remarks",
                    Items = new List<BankCashReceiptItemViewModel>
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            Summary = 1,
                        }
                    }
                };
            }
        }

        private BankCashReceiptViewModel viewModelDollarWithDecimal
        {
            get
            {
                return new BankCashReceiptViewModel
                {
                    ReceiptNo = "no",
                    Amount = 100.12M,
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel
                    {
                        Id = 1,
                        AccountName = "accountName",
                        AccountCOA = "accountCOA",
                        AccountNumber = "accountNumber",
                        BankCode = "bankCode",
                        BankName = "bankName",
                        Code = "code",
                        Currency = null
                    },
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                    {
                        Id = 1,
                        Code = "USD",
                        Rate = 1,
                        Description = "description",
                        Symbol = "symbol",
                    },
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    IncomeType = "incomeType",
                    ReceiptDate = DateTimeOffset.Now,
                    NumberingCode = "numberingCode",
                    Remarks = "remarks",
                    BankCashReceiptType= new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id=1,
                        Name="LAIN LAIN"
                    },
                    Items = new List<BankCashReceiptItemViewModel>
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            Summary = 1,
                        }
                    }
                };
            }
        }

        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IBankCashReceiptService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IBankCashReceiptService>(), Mapper: new Mock<IMapper>());
        }

        protected BankCashReceiptController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IBankCashReceiptService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                    new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            BankCashReceiptController controller = new BankCashReceiptController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
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

        private int GetStatusCodeGet((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IBankCashReceiptService> Service, Mock<IMapper> Mapper) mocks)
        {
            BankCashReceiptController controller = GetController(mocks);
            IActionResult response = controller.Get();

            return GetStatusCode(response);
        }

        [Fact]
        public void Get_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service
                .Setup(f => f.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<BankCashReceiptModel>(new List<BankCashReceiptModel>() { new BankCashReceiptModel() }, 0, new Dictionary<string, string>(), new List<string>()));
            mocks.Mapper
                .Setup(f => f.Map<List<BankCashReceiptViewModel>>(It.IsAny<List<BankCashReceiptModel>>()))
                .Returns(new List<BankCashReceiptViewModel>());

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

        private async Task<int> GetStatusCodePost((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IBankCashReceiptService> Service, Mock<IMapper> Mapper) mocks)
        {
            BankCashReceiptController controller = GetController(mocks);
            IActionResult response = await controller.Post(viewModel);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<BankCashReceiptViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<BankCashReceiptModel>())).ReturnsAsync(1);

            int statusCode = await GetStatusCodePost(mocks);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<BankCashReceiptViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<BankCashReceiptModel>())).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodePost(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Post_Throws_Validation_Exception()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<BankCashReceiptViewModel>())).Throws(GetServiceValidationExeption());
            var mockMapper = new Mock<IMapper>();

            var mockFacade = new Mock<IBankCashReceiptService>();
            var mockIdentity = new Mock<IIdentityService>();
            var ViewModel = this.viewModel;
            ViewModel.ReceiptDate = DateTimeOffset.MinValue;
            var response = GetController((mockIdentity, validateMock, mockFacade, mockMapper)).Post(ViewModel).Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        private async Task<int> GetStatusCodeGetById((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IBankCashReceiptService> Service, Mock<IMapper> Mapper) mocks)
        {
            BankCashReceiptController controller = GetController(mocks);
            IActionResult response = await controller.GetById(1);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task GetById_NotNullModel_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(new BankCashReceiptModel());

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetById_NullModel_ReturnNotFound()
        {
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync((BankCashReceiptModel)null);

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetById_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Delete_Success()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1);

            var response = GetController(mocks).Delete(1).Result;
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public void Delete_Throws_Internal_Error()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).Delete(1).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Delete_Throws_Internal_Error_Aggregate()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteAsync(It.IsAny<int>())).Throws(new AggregateException());

            var response = GetController(mocks).Delete(1).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        private async Task<int> GetStatusCodePut((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IBankCashReceiptService> Service, Mock<IMapper> Mapper) mocks)
        {
            BankCashReceiptController controller = GetController(mocks);
            IActionResult response = await controller.Put(viewModel.Id, viewModel);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task Put_WithoutException_ReturnUpdated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<BankCashReceiptViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<BankCashReceiptModel>())).ReturnsAsync(1);

            int statusCode = await GetStatusCodePut(mocks);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public void Update_Throws_Validation_Exception()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<BankCashReceiptViewModel>())).Throws(GetServiceValidationExeption());
            var mockMapper = new Mock<IMapper>();

            var mockFacade = new Mock<IBankCashReceiptService>();
            var mockIdentity = new Mock<IIdentityService>();
            var response = GetController((mockIdentity, validateMock, mockFacade, mockMapper)).Put(It.IsAny<int>(), viewModel).Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }


        [Fact]
        public void Update_Throws_Internal_Error()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.UpdateAsync(It.IsAny<int>(), It.IsAny<BankCashReceiptModel>())).Throws(new Exception());

            var response = GetController(mocks).Put(1, It.IsAny<BankCashReceiptViewModel>()).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Update_Throws_Internal_Error_Aggregate()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.UpdateAsync(It.IsAny<int>(), It.IsAny<BankCashReceiptModel>())).Throws(new AggregateException());

            var response = GetController(mocks).Put(1, It.IsAny<BankCashReceiptViewModel>()).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async void GeneratePdf_Success_IDR_Return_OK()
        {
            //Setup
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(new BankCashReceiptModel());
            BankCashReceiptController controller = GetController(mocks);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            var response = await controller.GetById(It.IsAny<int>());

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }

        [Fact]
        public async void GeneratePdf_Success_IDR_WithDecimal_Return_OK()
        {
            //Setup
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModelWithDecimal);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(new BankCashReceiptModel());
            BankCashReceiptController controller = GetController(mocks);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            var response = await controller.GetById(It.IsAny<int>());

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }

        [Fact]
        public async void GeneratePdf_Success_USD_WithDecimal_Return_OK()
        {
            //Setup
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModelDollarWithDecimal);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(new BankCashReceiptModel());
            BankCashReceiptController controller = GetController(mocks);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            var response = await controller.GetById(It.IsAny<int>());

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }

        [Fact]
        public async void GeneratePdf_Success_USD_ZeroDecimal_Return_OK()
        {
            //Setup
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModelDollarZeroDecimal);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(new BankCashReceiptModel());
            BankCashReceiptController controller = GetController(mocks);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            var response = await controller.GetById(It.IsAny<int>());

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }

        [Fact]
        public async void GeneratePdf_Success_Return_NotFound()
        {
            //Setup
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync((BankCashReceiptModel)null);
            BankCashReceiptController controller = GetController(mocks);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            IActionResult response = await controller.GetById(1);
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async void GeneratePdf_Success_Return_Internal_Server_Error()
        {
            //Setup
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<BankCashReceiptViewModel>(It.IsAny<BankCashReceiptModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception());
            BankCashReceiptController controller = GetController(mocks);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            IActionResult response = await controller.GetById(1);
            int statusCode = GetStatusCode(response);
            //Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }
    }

   
}
