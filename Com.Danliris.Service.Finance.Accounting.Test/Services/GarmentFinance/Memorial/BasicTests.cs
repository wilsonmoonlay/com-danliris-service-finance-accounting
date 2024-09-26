using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.Memorial
{
    public class BasicTests
    {
        private const string ENTITY = "GarmentFinanceMemorials";

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

        private GarmentFinanceMemorialDataUtil _dataUtil(GarmentFinanceMemorialService service, string testname)
        {
            return new GarmentFinanceMemorialDataUtil(service);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            GarmentFinanceMemorialService service = new GarmentFinanceMemorialService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialModel model = _dataUtil(service, GetCurrentMethod()).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            GarmentFinanceMemorialService service = new GarmentFinanceMemorialService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            GarmentFinanceMemorialService service = new GarmentFinanceMemorialService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentFinanceMemorialModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            GarmentFinanceMemorialService service = new GarmentFinanceMemorialService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            GarmentFinanceMemorialModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            GarmentFinanceMemorialService service = new GarmentFinanceMemorialService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            GarmentFinanceMemorialModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);
            //newModel.BGCheckNumber = "newBG";
            var Response1 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response1);

            GarmentFinanceMemorialModel model2 = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            //var newModel2 = await service.ReadByIdAsync(model.Id);
            GarmentFinanceMemorialModel newModel2 = new GarmentFinanceMemorialModel();
            newModel2.Id = model2.Id;
            newModel2.Items = new List<GarmentFinanceMemorialItemModel> { model2.Items.First() };
            newModel2.Items.First().Credit = 100;
            newModel2.Items.First().Debit = 100;
            var Response = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            GarmentFinanceMemorialItemModel newItem = new GarmentFinanceMemorialItemModel
            {
                COAId = 3,
                COACode = "no3",
                COAName = "Name",
                Debit = 3,
                Credit=0
            };

            GarmentFinanceMemorialItemModel newItem2 = new GarmentFinanceMemorialItemModel
            {
                COAId = 3,
                COACode = "no3",
                COAName = "Name",
                Credit = 3,
                Debit=0
            };

            newModel2.Items.Add(newItem);
            newModel2.Items.Add(newItem2);
            var Response3 = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            GarmentFinanceMemorialViewModel vm = new GarmentFinanceMemorialViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Null_Invoice_Data()
        {
            GarmentFinanceMemorialViewModel vm = new GarmentFinanceMemorialViewModel();
            vm.Items = new List<GarmentFinanceMemorialItemViewModel>
            {
                new GarmentFinanceMemorialItemViewModel()
                {
                    COA=null,
                    Debit=0,
                    Credit=1
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

    }
}

