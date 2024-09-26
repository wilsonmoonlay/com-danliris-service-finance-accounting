using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.MemorialDetailLocal
{
    public class BasicTests
    {
        private const string ENTITY = "GarmentFinanceMemorialDetailLocals";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        private FinanceDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
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

        private GarmentFinanceMemorialDetailLocalDataUtil _dataUtil(GarmentFinanceMemorialDetailLocalService service, string testname)
        {
            var memorialService = new GarmentFinanceMemorialService(GetServiceProvider().Object, service.DbContext);
            var memorialDataUtil = new GarmentFinanceMemorialDataUtil(memorialService);
            return new GarmentFinanceMemorialDetailLocalDataUtil(service, memorialDataUtil);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            GarmentFinanceMemorialDetailLocalService service = new GarmentFinanceMemorialDetailLocalService(GetServiceProvider().Object, _dbContext(GetCurrentAsyncMethod()));
            GarmentFinanceMemorialDetailLocalModel model = _dataUtil(service, GetCurrentAsyncMethod()).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            GarmentFinanceMemorialDetailLocalService service = new GarmentFinanceMemorialDetailLocalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            GarmentFinanceMemorialDetailLocalService service = new GarmentFinanceMemorialDetailLocalService(GetServiceProvider().Object, _dbContext(GetCurrentAsyncMethod()));
            GarmentFinanceMemorialDetailLocalModel model = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            GarmentFinanceMemorialDetailLocalService service = new GarmentFinanceMemorialDetailLocalService(GetServiceProvider().Object, _dbContext(GetCurrentAsyncMethod()));


            GarmentFinanceMemorialDetailLocalModel model = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            GarmentFinanceMemorialDetailLocalService service = new GarmentFinanceMemorialDetailLocalService(GetServiceProvider().Object, _dbContext(GetCurrentAsyncMethod()));


            GarmentFinanceMemorialDetailLocalModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);
            //newModel.BGCheckNumber = "newBG";
            var Response1 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response1);

            GarmentFinanceMemorialDetailLocalModel model2 = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            //var newModel2 = await service.ReadByIdAsync(model.Id);
            GarmentFinanceMemorialDetailLocalModel newModel2 = new GarmentFinanceMemorialDetailLocalModel();
            newModel2.Id = model2.Id;

            newModel2.Items = new List<GarmentFinanceMemorialDetailLocalItemModel> { model2.Items.First() };
            newModel2.OtherItems = new List<GarmentFinanceMemorialDetailLocalOtherItemModel> { model2.OtherItems.First() };
            var Response = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            GarmentFinanceMemorialDetailLocalItemModel newItem = new GarmentFinanceMemorialDetailLocalItemModel
            {
                LocalSalesNoteId = 3,
                LocalSalesNoteNo = "no3",
                BuyerName = "Name",
                BuyerCode = "code",
                BuyerId = 3,
                CurrencyId = 1,
                CurrencyCode = "code",
                CurrencyRate = 1,
                Amount = 1
            };


            newModel2.Items.Add(newItem);

            GarmentFinanceMemorialDetailLocalOtherItemModel newOtherItem = new GarmentFinanceMemorialDetailLocalOtherItemModel
            {
                ChartOfAccountId = 1,
                ChartOfAccountName = "Name",
                ChartOfAccountCode = "code",
                CurrencyId = 1,
                CurrencyCode = "code",
                CurrencyRate = 1,
                Amount = 1,
                TypeAmount = "DEBIT"
            };
            newModel2.OtherItems.Add(newOtherItem);
            var Response3 = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            // delete item and other item
            GarmentFinanceMemorialDetailLocalModel newModel3 = new GarmentFinanceMemorialDetailLocalModel();
            newModel3.Id = model2.Id;

            newModel3.Items = new List<GarmentFinanceMemorialDetailLocalItemModel>();
            newModel3.OtherItems = new List<GarmentFinanceMemorialDetailLocalOtherItemModel>();
            var Response4 = await service.UpdateAsync(model2.Id, newModel3);
            Assert.NotEqual(0, Response4);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            GarmentFinanceMemorialDetailLocalViewModel vm = new GarmentFinanceMemorialDetailLocalViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Null_Invoice_Data()
        {
            GarmentFinanceMemorialDetailLocalViewModel vm = new GarmentFinanceMemorialDetailLocalViewModel();
            vm.Items = new List<GarmentFinanceMemorialDetailLocalItemViewModel>
            {
                new GarmentFinanceMemorialDetailLocalItemViewModel()
                {
                    LocalSalesNoteId=0,
                    Amount = 0,
                }
            };
            vm.OtherItems = new List<GarmentFinanceMemorialDetailLocalOtherItemViewModel>
            {
                new GarmentFinanceMemorialDetailLocalOtherItemViewModel()
                {
                    Account=null,
                    Amount = 0,
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }
    }
}
