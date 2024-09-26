using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument.Helper;
using Com.Moonlay.Models;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.DailyBankTransaction
{
    public class DailyBankTransactionServiceTest
    {
        private const string ENTITY = "DailyBankTransactions";
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

        private DailyBankTransactionDataUtil _dataUtil(DailyBankTransactionService service)
        {
            return new DailyBankTransactionDataUtil(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new DailyBankTransactionIHttpService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            serviceProvider
                .Setup(x => x.GetService(typeof(IAutoJournalService)))
                .Returns(new AutoJournalServiceTestHelper());


            return serviceProvider;
        }

        [Fact]
        public async Task Should_Success_Get_Data_In()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestDataIn();
            var Response = service.Read(1, 25, "{}", null, data.Code, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_GetReportAll_In()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestDataIn();

            var Response = service.GetReportAll(data.Code, 0, string.Empty, data.Date, data.Date, 1, 25, "{}", null, data.Code, "{\"Status\":\"IN\"}");
            Assert.NotNull(Response.Data);

            var Response2 = service.GetReportAll(data.Code, 1, string.Empty, data.Date, data.Date, 0, 25, "{}", null, data.Code, "{\"Status\":\"IN\"}");
            Assert.NotNull(Response2.Data);
        }

        [Fact]
        public async Task Should_Success_GetLoader()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestDataIn();

            var Response = service.GetLoader(data.ReferenceNo, "{\"Status\":\"IN\"}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_GetReportAll_Out()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestDataOut();

            var Response = service.GetReportAll(data.Code, 0, string.Empty, data.Date, data.Date, 1, 25, "{}", null, data.Code, "{\"Status\":\"OUT\"}");
            Assert.NotNull(Response.Data);
        }

        [Fact]
        public async Task Should_Success_GetDocumentNo()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestDataOut();

            var Response = await service.GetDocumentNo("K", "test", "test");
            Assert.NotEmpty(Response);
        }

        //[Fact]
        //public async Task Should_Success_GenerateExcel()
        //{
        //    DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    var data = await _dataUtil(service).GetTestDataIn();
        //    var Response = service.GetExcel(data.AccountBankId, data.Date.Month, data.Date.Year, 1);
        //    Assert.NotNull(Response);

        //    //data.AccountBankId = 4;
        //    //var Response2 = service.GetExcel(data.AccountBankId, data.Date.Month, data.Date.Year, 1);
        //    //Assert.NotNull(Response2);
        //}

        //[Fact]
        //public void Should_Success_GenerateExcel_when_dataEmpty()
        //{
        //    DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

        //    var Response = service.GetExcel(0, 7, 1001, 0);
        //    Assert.NotNull(Response);
        //}

        //[Fact]
        //public async Task Should_Success_Get_Report()
        //{
        //    DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    var data = await _dataUtil(service).GetTestDataIn();
        //    var Response = service.GetReport(data.AccountBankId, data.Date.Month, data.Date.Year, 1);
        //    Assert.NotEmpty(Response.Data);
        //}

        [Fact]
        public async Task Should_Success_Get_Data_Out()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            await _dataUtil(service).GetTestDataOut();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = await _dataUtil(service).GetTestDataIn();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_No_Error_Validate_Data()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionViewModel vm = _dataUtil(service).GetDataToValidate();

            Assert.True(vm.Validate(null).Count() == 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_With_Invalid_Input_Data_In_Buyer_Null_Operasional()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel
            {
                Date = DateTime.Now.AddYears(1),
                Status = "IN",
                SourceType = "Operasional"
            };


            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_With_Invalid_Input_Data_In_Receiver_Null_Others()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel
            {
                Date = DateTime.Now.AddYears(1),
                Status = "IN",
                SourceType = "Lain - lain"
            };


            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_With_Invalid_Input_Data_In_Buyer_NotNull_NonOperasional()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel
            {
                Bank = new AccountBankViewModel()
                {
                    Id = 0
                },
                Buyer = new NewBuyerViewModel()
                {
                    Id = 0
                },
                Date = DateTime.Now.AddYears(1),
                Nominal = 0,
                Status = "IN",
                SourceType = "Investasi"
            };


            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_With_Invalid_Input_Data_Out_Supplier_Null_Operasional()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel
            {
                Date = DateTime.Now.AddYears(1),
                Status = "OUT",
                SourceType = "Operasional"
            };


            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Instatiate_New_Buyer()
        {
            var buyer = new NewBuyerViewModel()
            {
                Id = 1,
                Code = "Code",
                Name = "Name"
            };


            Assert.True(buyer != null);
        }

        [Fact]
        public void Should_Success_Instatiate_New_Supplier()
        {
            var supplier = new NewSupplierViewModel()
            {
                _id = 1,
                code = "Code",
                name = "Name",
                import = false
            };


            Assert.True(supplier != null);
        }

        [Fact]
        public void Should_Success_Instatiate_Buyer()
        {
            var supplier = new Lib.ViewModels.IntegrationViewModel.BuyerViewModel()
            {
                _id = "",
                code = "Code",
                name = "Name",
            };

            Assert.Equal("Code", supplier.code);
            Assert.Equal("Name", supplier.name);
            Assert.Equal("", supplier._id);
            Assert.True(supplier != null);
        }

        [Fact]
        public void Should_Success_Instatiate_Supplier()
        {
            var supplier = new Lib.ViewModels.IntegrationViewModel.SupplierViewModel()
            {
                _id = "",
                code = "Code",
                name = "Name"
            };


            Assert.True(supplier != null);
        }

        [Fact]
        public void Should_Success_Instatiate_AccountBank()
        {
            var supplier = new Lib.ViewModels.IntegrationViewModel.AccountBankViewModel()
            {
                _id = "",
                code = "Code",
                accountName = "accountName",
                bankName = "bankName",
                accountCurrencyId= "123",
                accountNumber= "123",
                bankCode = "bankCode",
                currency = new Lib.ViewModels.IntegrationViewModel.CurrencyViewModel()
                {
                    code = "",
                    description = "",
                    rate = 0,
                    symbol = "",
                    _id = ""
                }
            };

            Assert.Equal("Code", supplier.code);
            Assert.Equal("accountName", supplier.accountName);
            Assert.Equal("bankName", supplier.bankName);
            Assert.Equal("bankCode", supplier.bankCode);
            Assert.Equal("123", supplier.accountCurrencyId);
            Assert.Equal("123", supplier.accountNumber);
            Assert.NotNull(supplier.currency);
            Assert.True(supplier != null);
        }

        [Fact]
        public void Should_Success_Validate_With_Invalid_Input_Data_Out_Supplier_NotNull_NonOperasional()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel
            {
                Bank = new AccountBankViewModel()
                {
                    Id = 0
                },
                Date = DateTime.Now.AddYears(1),
                Status = "OUT",
                SourceType = "Investasi",
                Supplier = new NewSupplierViewModel()
                {
                    _id = 0
                }
            };


            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_With_Invalid_Input_Data_Out_Supplier_NotNull_Pendanaan()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel
            {
                Bank = new AccountBankViewModel()
                {
                    Id = 0
                },
                Date = DateTime.Now.AddYears(1),
                Status = "OUT",
                SourceType = "Pendanaan",
                Supplier = new NewSupplierViewModel()
                {
                    _id = 0
                },
                OutputBank = new AccountBankViewModel()
                {
                    Id = 0
                },
                TransactionNominal = 0
            };


            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_With_Invalid_Input_Data_Out_Supplier_NotNull_Pendanaan_Internal()
        {
            DailyBankTransactionViewModel vm = new DailyBankTransactionViewModel
            {
                Bank = new AccountBankViewModel()
                {
                    Id = 0
                },
                Date = DateTime.Now.AddYears(1),
                Status = "OUT",
                SourceType = "Pendanaan",
                SourceFundingType = "Internal",
                Supplier = new NewSupplierViewModel()
                {
                    _id = 0
                },
                OutputBank = new AccountBankViewModel()
                {
                    Id = 0
                },
                TransactionNominal = 0
            };


            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = await _dataUtil(service).GetTestDataNotPosted();
            var newModel = await service.ReadByIdAsync(model.Id);
            newModel.ReferenceNo = "";
            newModel.SourceType = "Pendanaan";
            var Response = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response);

            newModel.ReferenceNo = "";
            newModel.Status = "IN";
            var Response2 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response2);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = await _dataUtil(service).GetTestDataNotPosted();
            var newModel = await service.ReadByIdAsync(model.Id);
            newModel.FinancingSourceReferenceId = 1;

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Succes_When_Create_New_Data_With_Non_Exist_Next_Month_Or_Previous_Month_Balance()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();

            if (DateTime.Now.Month < 7)
            {
                model.Date = new DateTime(DateTime.Now.Year - 1, 8, 1);
            }
            else
            {
                model.Date = model.Date.AddMonths(-6);
            }

            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Create_December()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Date = new DateTime(2018, 1, 1);
            var modelResponse = await service.CreateAsync(model);
            DailyBankTransactionModel previousMonthModel = _dataUtil(service).GetNewData();
            previousMonthModel.Date = new DateTime(2017, 12, 1);

            var Response = await service.CreateAsync(previousMonthModel);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Delete_By_ReferenceNo()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Date = new DateTime(2018, 1, 1);
            model.Status = "IN";
            var modelResponse = await service.CreateAsync(model);

            var Response = await service.DeleteByReferenceNoAsync(model.ReferenceNo);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Delete_By_ReferenceNo_NextMonth_Exist()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Date = new DateTime(2018, 1, 1);
            model.Status = "OUT";
            model.ReferenceNo = model.Date.ToString();
            var modelResponse = await service.CreateAsync(model);

            DailyBankTransactionModel modelNextMonth = _dataUtil(service).GetNewData();
            modelNextMonth.Date = new DateTime(2018, 2, 1);
            var modelNextMonthResponse = await service.CreateAsync(modelNextMonth);

            var Response = await service.DeleteByReferenceNoAsync(model.ReferenceNo);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_CreateInOut_Data()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Status = "OUT";
            model.SourceType = "Pendanaan";
            var Response = await service.CreateInOutTransactionAsync(model);
            Assert.NotEqual(0, Response);
            var vm = _dataUtil(service).GetDataToValidate();
            vm.Status = "OUT";
            vm.SourceType = "Pendanaan";
            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Fail_CreateInOut_Data()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Status = null;
            model.IsPosted = true;
            model.SourceType = "Pendanaan";
            //var Response = await service.CreateInOutTransactionAsync(model);
            await Assert.ThrowsAnyAsync<Exception>(() => service.CreateInOutTransactionAsync(model));
        }

        [Fact]
        public async Task Should_Success_ReportDailyBalance_Data()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Status = "OUT";
            model.SourceType = "Pendanaan";
            await service.CreateInOutTransactionAsync(model);

            var data = service.GetDailyBalanceReport(model.AccountBankId, DateTime.Now.AddDays(-7), DateTime.Now, "G");
            Assert.NotNull(data);
        }

        [Fact]
        public async Task Should_Success_ReportDailyBalance_Excel()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Status = "OUT";
            model.SourceType = "Pendanaan";
            await service.CreateInOutTransactionAsync(model);

            var data = service.GenerateExcelDailyBalance(model.AccountBankId, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(7), "G", 1);
            Assert.NotNull(data);
        }

        [Fact]
        public void Should_Success_ReportDailyBalance_Excel_When_DataNoExist()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var result = service.GenerateExcelDailyBalance(1, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(7), "G", 1);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Success_CreateInOut_Data_Empty_Remark()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Status = "OUT";
            model.SourceType = "Pendanaan";
            model.Remark = "";
            var Response = await service.CreateInOutTransactionAsync(model);
            Assert.NotEqual(0, Response);
            var vm = _dataUtil(service).GetDataToValidate();
            vm.Status = "OUT";
            vm.SourceType = "Pendanaan";
            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Posting_Transaction()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            await service.CreateInOutTransactionAsync(model);
            var response = await service.Posting(new List<int>() { model.Id });
            Assert.True(response > 0);
        }

        [Fact]
        public async Task Should_Success_CreateInOut_Data_Not_Empty_Remark()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            DailyBankTransactionModel model = _dataUtil(service).GetNewData();
            model.Status = "OUT";
            model.SourceType = "Pendanaan";
            model.Remark = "any remark";
            var Response = await service.CreateInOutTransactionAsync(model);
            Assert.NotEqual(0, Response);
            var vm = _dataUtil(service).GetDataToValidate();
            vm.Status = "OUT";
            vm.SourceType = "Pendanaan";
            Assert.True(vm.Validate(null).Count() > 0);
        }

        //[Fact]
        //public async Task Should_Success_GeneratePdf()
        //{
        //    DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
        //    var data = await _dataUtil(service).GetTestDataIn();
        //    var response = service.GeneratePdf(data.AccountBankId, data.Date.Month, data.Date.Year, 1);
        //    Assert.NotEmpty(response);
        //}

        [Fact]
        public async Task Should_Success_GetBeforeBalance()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestDataIn();
            var response = service.GetBeforeBalance(data.AccountBankId, data.Date.Month, data.Date.Year, 7);

            //Assert.NotEqual(0,response);
        }

        [Fact]
        public async Task Should_Success_GetDataAccountBank()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestDataIn();
            var response = service.GetDataAccountBank(data.AccountBankId);
            Assert.NotEmpty( response);
        }

        [Fact]
        public async Task Should_Success_IsPosted_Transaction()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var service = new DailyBankTransactionService(GetServiceProvider().Object, dbContext);
            var model = _dataUtil(service).GetNewData();

            var monthlyBalance = new List<BankTransactionMonthlyBalanceModel>()
            {
                new BankTransactionMonthlyBalanceModel()
                {
                    Month = 10,
                    Year = 2019,
                    AccountBankId = model.AccountBankId
                },
                new BankTransactionMonthlyBalanceModel()
                {
                    Month = 8,
                    Year = 2019,
                    AccountBankId = model.AccountBankId
                },
                new BankTransactionMonthlyBalanceModel()
                {
                    Month = 12,
                    Year = 2019,
                    AccountBankId = model.AccountBankId
                }
            };

            foreach (var datum in monthlyBalance)
            {
                EntityExtension.FlagForCreate(datum, "Test", "Test");
            }

            dbContext.BankTransactionMonthlyBalances.AddRange(monthlyBalance);
            dbContext.SaveChanges();

            model.IsPosted = true;
            var response = await service.CreateAsync(model);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_IsPosted_Transaction_SameYear()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var service = new DailyBankTransactionService(GetServiceProvider().Object, dbContext);
            var model = _dataUtil(service).GetNewData();

            var monthlyBalance = new List<BankTransactionMonthlyBalanceModel>()
            {
                new BankTransactionMonthlyBalanceModel()
                {
                    Month = 10,
                    Year = 2019,
                    AccountBankId = model.AccountBankId
                },
                new BankTransactionMonthlyBalanceModel()
                {
                    Month = 8,
                    Year = 2020,
                    AccountBankId = model.AccountBankId
                },
                new BankTransactionMonthlyBalanceModel()
                {
                    Month = 12,
                    Year = 2020,
                    AccountBankId = model.AccountBankId
                }
            };

            foreach (var datum in monthlyBalance)
            {
                EntityExtension.FlagForCreate(datum, "Test", "Test");
            }

            dbContext.BankTransactionMonthlyBalances.AddRange(monthlyBalance);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task Should_Success_Posting_Pendanaan_Internal_Transaction()
        {
            DailyBankTransactionService service = new DailyBankTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            model.SourceType = "Pendanaan";
            model.SourceFundingType = "Internal";
            //model.BankCharges = 100;
            
            await service.CreateInOutTransactionAsync(model);
            var response = await service.Posting(new List<int>() { model.Id });
            Assert.True(response > 0);
        }
    }
}
