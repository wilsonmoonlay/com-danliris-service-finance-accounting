using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.BankCashReceiptDetailLocal
{
    public class GarmentFinanceBankCashReceiptDetailLocalServiceTest
    {
        private string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        private FinanceDbContext GetDbContext(string testName)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider);

            return new FinanceDbContext(optionsBuilder.Options);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        private GarmentFinanceBankCashReceiptDetailLocalDataUtil _dataUtil(GarmentFinanceBankCashReceiptDetailLocalService service, string testname)
        {
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var bankCashReceiptService = new BankCashReceiptService(serviceProviderMock.Object);
            var bankCashReceiptDataUtil = new BankCashReceiptDataUtil(bankCashReceiptService);
            return new GarmentFinanceBankCashReceiptDetailLocalDataUtil(service, bankCashReceiptDataUtil);
        }

        private BankCashReceiptDataUtil _dataUtilReceipt(BankCashReceiptService service, string testname)
        {
            return new BankCashReceiptDataUtil(service);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {

            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            var serviceReceipt = new BankCashReceiptService(serviceProviderMock.Object);

            var dto = _dataUtilReceipt(serviceReceipt, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var ResponseReceipt = await serviceReceipt.ReadByIdAsync(dto.Id);

            var model = _dataUtil(service, GetCurrentAsyncMethod()).GetNewData();

            //Act
            var Response = await service.CreateAsync(model);

            //Assert
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Create_Data_Null_Receipt()
        {

            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            var serviceReceipt = new BankCashReceiptService(serviceProviderMock.Object);

            var model = _dataUtil(service, GetCurrentAsyncMethod()).GetNewData();
            var dto = _dataUtilReceipt(serviceReceipt, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var ResponseReceipt = await serviceReceipt.ReadByIdAsync(2);

            //Act
            var Response = await service.CreateAsync(model);

            //Assert
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var Response = service.Read(1, 25, "{}", null, null, "{}");

            //Assert
            Assert.NotNull(Response);
            Assert.NotEqual(0, Response.Count);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var Response = service.ReadByIdAsync(dto.Id);

            //Assert
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);
            var serviceReceipt = new BankCashReceiptService(serviceProviderMock.Object);

            var dto = _dataUtilReceipt(serviceReceipt, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var ResponseReceipt = await serviceReceipt.ReadByIdAsync(dto.Id);

            var model = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var newModel = await service.ReadByIdAsync(model.Id);
            var Response = await service.DeleteAsync(newModel.Id);

            //Assert
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data_Null_Receipt()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var model = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var newModel = await service.ReadByIdAsync(model.Id);
            var Response = await service.DeleteAsync(newModel.Id);

            //Assert
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Shou0ld_Success_Update_Data()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentFinanceBankCashReceiptDetailLocalService(serviceProviderMock.Object);

            var model = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            var model2 = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();

            GarmentFinanceBankCashReceiptDetailLocalModel newModel2 = new GarmentFinanceBankCashReceiptDetailLocalModel();
            newModel2.Id = model2.Id;
            //Act
            var newModel = await service.ReadByIdAsync(model.Id);
            var Response1 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response1);

            newModel2.Items = new List<GarmentFinanceBankCashReceiptDetailLocalItemModel> { model2.Items.First() };
            newModel2.OtherItems = new List<GarmentFinanceBankCashReceiptDetailLocalOtherItemModel> { model2.OtherItems.First() };
            var Response = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            GarmentFinanceBankCashReceiptDetailLocalItemModel newItem = new GarmentFinanceBankCashReceiptDetailLocalItemModel
            {
                BankCashReceiptDetailLocalId = 1,
                Amount = 2,
            };

            GarmentFinanceBankCashReceiptDetailLocalOtherItemModel newOtherItem = new GarmentFinanceBankCashReceiptDetailLocalOtherItemModel
            {
                BankCashReceiptDetailLocalId = 1,
                Amount = 2,
                TypeAmount = "DEBIT"
            };

            newModel2.Items.Add(newItem);
            newModel2.OtherItems.Add(newOtherItem);
            var Response3 = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            // delete item and other item
            GarmentFinanceBankCashReceiptDetailLocalModel newModel3 = new GarmentFinanceBankCashReceiptDetailLocalModel();
            newModel3.Id = model2.Id;

            newModel3.Items = new List<GarmentFinanceBankCashReceiptDetailLocalItemModel>();
            newModel3.OtherItems = new List<GarmentFinanceBankCashReceiptDetailLocalOtherItemModel>();
            var Response4 = await service.UpdateAsync(model2.Id, newModel3);
            Assert.NotEqual(0, Response4);

        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            GarmentFinanceBankCashReceiptDetailLocalViewModel vm = new GarmentFinanceBankCashReceiptDetailLocalViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Null_Items_Data()
        {
            GarmentFinanceBankCashReceiptDetailLocalViewModel vm = new GarmentFinanceBankCashReceiptDetailLocalViewModel();
            vm.Amount = 0;
            vm.Items = new List<GarmentFinanceBankCashReceiptDetailLocalItemViewModel>
            {
                new GarmentFinanceBankCashReceiptDetailLocalItemViewModel()
                {
                    LocalSalesNoteId =0,
                    Id=0,
                    Buyer = new Lib.ViewModels.NewIntegrationViewModel.BuyerViewModel()
                    {
                        Id = "1",
                        Code = null,
                    },
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                    {
                        Id = 0,
                        Code = null
                    }
                }
            };
            vm.OtherItems = new List<GarmentFinanceBankCashReceiptDetailLocalOtherItemViewModel>
            {
                new GarmentFinanceBankCashReceiptDetailLocalOtherItemViewModel()
                {
                    Id=0,
                    TypeAmount = "KREDIT",
                    Amount= 0,
                },
                new GarmentFinanceBankCashReceiptDetailLocalOtherItemViewModel()
                {
                    Id=0,
                    TypeAmount = "DEBIT",
                    Amount= 0,
                },
                new GarmentFinanceBankCashReceiptDetailLocalOtherItemViewModel()
                {
                    Id=0,
                    TypeAmount = null,
                    Amount= 0,
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Amount_Different()
        {
            GarmentFinanceBankCashReceiptDetailLocalViewModel vm = new GarmentFinanceBankCashReceiptDetailLocalViewModel();
            vm.Amount = 3;
            vm.Items = new List<GarmentFinanceBankCashReceiptDetailLocalItemViewModel>
            {
                new GarmentFinanceBankCashReceiptDetailLocalItemViewModel()
                {
                    Amount = 1,
                }
            };
            vm.OtherItems = new List<GarmentFinanceBankCashReceiptDetailLocalOtherItemViewModel>
            {
                new GarmentFinanceBankCashReceiptDetailLocalOtherItemViewModel()
                {
                    TypeAmount = "KREDIT",
                    Amount= 1,
                },
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }
    }
}
