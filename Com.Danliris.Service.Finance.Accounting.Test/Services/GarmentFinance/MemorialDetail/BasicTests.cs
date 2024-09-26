using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.MemorialDetail;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.MemorialDetail
{
    public class BasicTests
    {
        private const string ENTITY = "GarmentFinanceMemorialDetails";



        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
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

        private GarmentFinanceMemorialDetailDataUtil _dataUtil(GarmentFinanceMemorialDetailService service, string testname)
        {
            var memorialService = new GarmentFinanceMemorialService(GetServiceProvider().Object, service.DbContext);
            var memorialDataUtil = new GarmentFinanceMemorialDataUtil(memorialService);
            return new GarmentFinanceMemorialDetailDataUtil(service,memorialDataUtil);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            GarmentFinanceMemorialDetailService service = new GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialDetailModel model = _dataUtil(service, GetCurrentMethod()).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            GarmentFinanceMemorialDetailService service = new GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            GarmentFinanceMemorialDetailService service = new GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialDetailModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            GarmentFinanceMemorialDetailService service = new GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            GarmentFinanceMemorialDetailModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            GarmentFinanceMemorialDetailService service = new GarmentFinanceMemorialDetailService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            GarmentFinanceMemorialDetailModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);
            //newModel.BGCheckNumber = "newBG";
            var Response1 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response1);

            GarmentFinanceMemorialDetailModel model2 = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            //var newModel2 = await service.ReadByIdAsync(model.Id);
            GarmentFinanceMemorialDetailModel newModel2 = new GarmentFinanceMemorialDetailModel();
            newModel2.Id = model2.Id;

            newModel2.Items = new List<GarmentFinanceMemorialDetailItemModel> { model2.Items.First() };
            newModel2.OtherItems = new List<GarmentFinanceMemorialDetailOtherItemModel> { model2.OtherItems.First() };
            newModel2.RupiahItems = new List<GarmentFinanceMemorialDetailRupiahItemModel> { model2.RupiahItems.First() };

            var Response = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            GarmentFinanceMemorialDetailItemModel newItem = new GarmentFinanceMemorialDetailItemModel
            {
                InvoiceId = 3,
                InvoiceNo = "no3",
                BuyerName = "Name",
                BuyerCode = "code",
                BuyerId = 3,
                CurrencyId = 1,
                CurrencyCode="code",
                CurrencyRate=1,
                Amount = 1
            };


            newModel2.Items.Add(newItem);

            GarmentFinanceMemorialDetailOtherItemModel newOtherItem = new GarmentFinanceMemorialDetailOtherItemModel
            {
                ChartOfAccountId = 1,
                ChartOfAccountName = "Name",
                ChartOfAccountCode = "code",
                CurrencyId = 1,
                CurrencyCode = "code",
                CurrencyRate = 1,
                Amount = 1,
                TypeAmount = "KREDIT"
            };
            newModel2.OtherItems.Add(newOtherItem);

            GarmentFinanceMemorialDetailRupiahItemModel newRupiahItem = new GarmentFinanceMemorialDetailRupiahItemModel
            {
                ChartOfAccountId = 1,
                ChartOfAccountName = "Name",
                ChartOfAccountCode = "code",
                Credit = 1,
                Debit = 1,
            };
            newModel2.RupiahItems.Add(newRupiahItem);

            var Response3 = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            GarmentFinanceMemorialDetailViewModel vm = new GarmentFinanceMemorialDetailViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Null_Invoice_Data()
        {
            GarmentFinanceMemorialDetailViewModel vm = new GarmentFinanceMemorialDetailViewModel();
            vm.TotalAmount = 100;
            vm.Items = new List<GarmentFinanceMemorialDetailItemViewModel>
            {
                new GarmentFinanceMemorialDetailItemViewModel()
                {
                    InvoiceId=0
                }
            };
            vm.OtherItems = new List<GarmentFinanceMemorialDetailOtherItemViewModel>
            {
                new GarmentFinanceMemorialDetailOtherItemViewModel()
                {
                    Account=null,
                    Amount = 0,
                }
            };
            vm.RupiahItems = new List<GarmentFinanceMemorialDetailRupiahItemViewModel>
            {
                new GarmentFinanceMemorialDetailRupiahItemViewModel()
                {
                    Account=null,
                    Credit = 0,
                    Debit = 0,
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Amount_Different()
        {
            GarmentFinanceMemorialDetailViewModel vm = new GarmentFinanceMemorialDetailViewModel();
            vm.Amount = 3;
            vm.Items = new List<GarmentFinanceMemorialDetailItemViewModel>
            {
                new GarmentFinanceMemorialDetailItemViewModel()
                {
                    Amount = 1,
                }
            };
            vm.OtherItems = new List<GarmentFinanceMemorialDetailOtherItemViewModel>
            {
                new GarmentFinanceMemorialDetailOtherItemViewModel()
                {
                    TypeAmount = "KREDIT",
                    Amount= 1,
                },
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

    }
}
