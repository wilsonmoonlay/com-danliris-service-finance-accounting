using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Master;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.Masters.COADataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.JournalTransaction
{
    public class JournalTransactionServiceTest
    {
        private const string ENTITY = "JournalTransaction";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

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

        private JournalTransactionDataUtil _dataUtil(JournalTransactionService service)
        {
            var coaService = new COAService(GetServiceProvider().Object, service._DbContext);
            var coaDataUtil = new COADataUtil(coaService);
            return new JournalTransactionDataUtil(service, coaDataUtil);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();
            var Response = service.Read(1, 25, "{}", null, data.DocumentNo, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_ByDate()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();
            DateTimeOffset date = DateTimeOffset.UtcNow;

            var Response = service.ReadByDate(date.AddDays(-1), date.AddDays(1), 0, 1, 25, "{}", null, data.DocumentNo, "{}");
            Assert.NotEmpty(Response.Data);
        }

        //[Fact]
        //public async Task Should_Error_Get_Data_ByDate()
        //{
        //    var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    var data = await _dataUtil(service).GetTestData();
        //    DateTimeOffset date = DateTimeOffset.UtcNow;

        //    var Response = service.ReadByDate(date, date.AddDays(-1), 0, 1, 25, "{}", null, data.DocumentNo, "{}");
        //    Assert.Throws<ServiceValidationException>(() => Response);
        //}

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_No_Error_Validate_Data()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var vm = _dataUtil(service).GetDataToValidate();

            Assert.True(vm.Validate(null).Count() == 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            var vm = new JournalTransactionViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_UpdateAsync_ExistData()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var existData = await service.ReadByIdAsync(model.Id);
            var Response = await service.UpdateAsync(existData.Id, existData);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_UpdateAsync_NewData()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var newData = _dataUtil(service).GetNewData();
            var Response = await service.UpdateAsync(model.Id, newData);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
           
            var Response = await service.DeleteAsync(model.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_Error_Validate_Data_NotEqual_Total()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var vm = _dataUtil(service).GetDataToValidate();
            vm.Items[1].Credit = 5000;
            vm.Date = DateTimeOffset.UtcNow.AddDays(2);

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Error_Validate_Data_Debit_And_Credit_Exist()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var vm = _dataUtil(service).GetDataToValidate();
            vm.Items[1].Debit = 5000;
            vm.Items[1].Credit = 1000;
            vm.Items[1].COA = null;

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Error_Validate_Data_Debit_And_Credit_EqualsZero()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var vm = _dataUtil(service).GetDataToValidate();
            vm.Items[1].Credit = 0;
            vm.Items[1].Credit = 0;

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Update_Data_NewItem()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            model.Description = "NewDescription";

            var newItemDebit = new JournalTransactionItemModel()
            {
                COA = new COAModel()
                {
                    Id = 1,
                    Code = ""
                },
                Debit = 100
            };
            model.Items.Add(newItemDebit);

            var newItemCredit = new JournalTransactionItemModel()
            {
                COA = new COAModel()
                {
                    Id = 1,
                    Code = ""
                },
                Credit = 100
            };
            model.Items.Add(newItemCredit);

            var Response = await service.UpdateAsync(model.Id, model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Generate_Excel()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var data = await _dataUtil(service).GetTestData();
            var reportResponse = service.GenerateExcel(data.Date.AddDays(-1), data.Date, 7);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public void Should_Success_Generate_Excel_Empty()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var reportResponse = service.GenerateExcel(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, 7);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Get_Report()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();

            var reportResponse = service.GetReport(1, 25, data.Date.AddDays(-1), data.Date, 7);
            Assert.NotNull(reportResponse.Item1);

            var reportResponse2 = service.GetReport(1, 25, data.Date.AddDays(-1), null, 7);
            Assert.NotNull(reportResponse2.Item1);

            var reportResponse3 = service.GetReport(1, 25, null, data.Date, 7);
            Assert.NotNull(reportResponse3.Item1);

            var reportResponse4 = service.GetReport(1, 25, null, null, 7);
            Assert.NotNull(reportResponse4.Item1);
        }

        [Fact]
        public async Task Should_Success_ReverseJournalTransaction_Data()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            var createdData = await service.CreateAsync(model);
            var response = await service.ReverseJournalTransactionByReferenceNo(model.ReferenceNo);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_CreateDuplicateReferenceNo_Data()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            var createdData = await service.CreateAsync(model);

            var newModel = _dataUtil(service).GetNewData();
            newModel.ReferenceNo = model.ReferenceNo;
            var response = await service.CreateAsync(newModel);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Skip_ReverseJournalTransaction_DataNotFound()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            //await Assert.ThrowsAsync<Exception>(() => service.ReverseJournalTransactionByReferenceNo("test"));
            var response = await service.ReverseJournalTransactionByReferenceNo("test");
            Assert.Equal(0, response);
        }

        [Fact]
        public async Task Should_Success_Generate_SubLedger_Excel()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var data = await _dataUtil(service).GetTestPostedData();
            var data2 = await _dataUtil(service).GetTestPostedGarmentIData();
            var data3 = await _dataUtil(service).GetTestPostedGarmentLData();
            var reportResponse = await service.GetSubLedgerReportXls(data.Items.ToList()[0].COAId, data.Date.Month, data.Date.Year, 1);

            Assert.NotNull(reportResponse);

            var reportResponse2 = await service.GetSubLedgerReportXls(null, data.Date.Month, data.Date.Year, 1);
            Assert.NotNull(reportResponse2);
        }

        [Fact]
        public async Task Should_Success_GetSubLedgerReportXls_When_EmptyData()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var result = await service.GetSubLedgerReportXls(null, DateTime.Now.Month, DateTime.Now.Year, 1);
            Assert.NotNull(result);
        }


        [Fact]
        public async Task Should_Success_Generate_SubLedger()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(Guid.NewGuid().ToString()));
            
            //_dbContext(GetCurrentMethod().
            var data = await _dataUtil(service).GetTestPostedData();
            var reportResponse = await service.GetSubLedgerReport(data.Items.ToList()[0].COAId, data.Date.Month, data.Date.Year, 1);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Generate_NoBankName_SubLedger()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var data = await _dataUtil(service).GetTestPostedManualData();
            var reportResponse = await service.GetSubLedgerReport(data.Items.ToList()[0].COAId, data.Date.Month, data.Date.Year, 1);
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Posting_Transaction()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var data = await _dataUtil(service).GetTestData();
            var reportResponse = await service.PostTransactionAsync(data.Id);
            Assert.NotEqual(0, reportResponse);
        }

        [Fact]
        public async Task Should_Success_Posting_Transaction_By_Model()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var data = await _dataUtil(service).GetTestData();
            var reportResponse = await service.PostTransactionAsync(data.Id, data);
            Assert.NotEqual(0, reportResponse);
        }

        [Fact]
        public async Task Should_Success_Create_NextMonth_Data()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var service = new JournalTransactionService(GetServiceProvider().Object, dbContext);
            var model = _dataUtil(service).GetNewData();
            var Response = await service.CreateAsync(model);

            var numberGenerator = dbContext.JournalTransactionNumbers.FirstOrDefault();
            numberGenerator.Month = model.CreatedUtc.Month - 1;
            dbContext.JournalTransactionNumbers.Update(numberGenerator);
            dbContext.SaveChanges();

            var newModel = _dataUtil(service).GetNewData();
            newModel.ReferenceNo = new Guid().ToString();
            var NewResponse = await service.CreateAsync(newModel);

            Assert.NotEqual(0, NewResponse);
        }

        [Fact]
        public async Task Should_Success_Create_Many()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var firstModel = _dataUtil(service).GetNewData();
            var secondModel = _dataUtil(service).GetNewData();
            var response = await service.CreateManyAsync(new List<JournalTransactionModel>() { firstModel, secondModel });

            //var newModel = _dataUtil(service).GetNewData();
            //newModel.ReferenceNo = model.ReferenceNo;
            //var response = await service.CreateAsync(newModel);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_Create_Data_Non_Exist_COA()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            var items = new List<JournalTransactionItemModel>()
            {
                new JournalTransactionItemModel()
                {
                    COA = new COAModel()
                    {
                        Code = "9999.9.99.99",
                    },
                    Debit = 1000
                },
                new JournalTransactionItemModel()
                {
                    COA = new COAModel()
                    {
                        Code = "9999.9.99.98",
                    },
                    Credit = 1000
                }
            };
            model.Items = items;
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Throw_Exception_Create_Non_Exist_COA_Invalid_Format()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            var items = new List<JournalTransactionItemModel>()
            {
                new JournalTransactionItemModel()
                {
                    COA = new COAModel()
                    {
                        Code = "9999.9.99.99",
                    },
                    Debit = 1000
                },
                new JournalTransactionItemModel()
                {
                    COA = new COAModel()
                    {
                        Code = "9999.9.9998",
                    },
                    Credit = 1000
                }
            };
            model.Items = items;
            //var Response = await service.CreateAsync(model);
            await Assert.ThrowsAsync<Exception>(() => service.CreateAsync(model));
        }

        [Fact]
        public async Task Should_Success_Read_Unposted_Journal()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            model.Status = "DRAFT";

            var createdData = await service.CreateAsync(model);

            var response = service.ReadUnPostedTransactionsByPeriod(DateTimeOffset.Now.Month, DateTimeOffset.Now.Year, model.ReferenceNo, model.Description, false);

            Assert.NotEmpty(response);
        }

        [Fact]
        public async Task Should_Success_Read_ReferenceNo()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            model.Status = "DRAFT";
            model.Description = "VB";
            var createdData = await service.CreateAsync(model);

            var response = service.GetAllReferenceNo(model.ReferenceNo, true);

            Assert.NotEmpty(response);
        }

        [Fact]
        public async Task Should_Success_Read_ReferenceType()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            model.Status = "DRAFT";
            model.Description = "VB";
            var createdData = await service.CreateAsync(model);

            var response = service.GetAllReferenceType(model.Description, true);

            Assert.NotEmpty(response);
        }

        [Fact]
        public async Task Should_Success_Get_General_Ledger()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewPostedData();
            await service.CreateAsync(model);

            var response = await service.GetGeneralLedgerReport(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1), 0);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Get_General_LedgerXls()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewPostedData();
            await service.CreateAsync(model);

            var response = await service.GetGeneralLedgerReportXls(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1), 0);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_GetGeneralLedgerReportXls_DataNoExist()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            
            var response = await service.GetGeneralLedgerReportXls(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1), 0);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_GetGeneralLedgerReportXls_When_DataNoExist()
        {
            var service = new JournalTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
          
            var response = await service.GetGeneralLedgerReportXls(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1), 0);

            Assert.NotNull(response);
        }
    }
}

