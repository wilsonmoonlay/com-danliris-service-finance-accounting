using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.Masters.COADataUtils;
using System;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.DailyBankTransaction
{
    public class DailyBankTransactionDataUtil
    {
        private readonly DailyBankTransactionService Service;
        private readonly COADataUtil _COADataUtil;

        public DailyBankTransactionDataUtil(DailyBankTransactionService service)
        {
            Service = service;
        }

        public DailyBankTransactionModel GetNewData()
        {
            DailyBankTransactionModel TestData = new DailyBankTransactionModel()
            {
                AccountBankAccountName = "AccountName",
                AccountBankAccountNumber = "AccountNumber",
                AccountBankCode = "BankCode",
                AccountBankCurrencyCode = "CurrencyCode",
                AccountBankCurrencyId = 1,
                AccountBankCurrencySymbol = "CurrencySymbol",
                AccountBankId = 1,
                AccountBankName = "BankName",
                AfterNominal = 0,
                BeforeNominal = 0,
                BuyerCode = "BuyerCode",
                BuyerId = 1,
                BuyerName = "BuyerName",
                Date = DateTimeOffset.UtcNow,
                Nominal = 1000,
                ReferenceNo = "",
                ReferenceType = "ReferenceType",
                Remark = "Remark",
                SourceType = "Operasional",
                Status = "IN",
                SupplierCode = "SupplierCode",
                SupplierName = "SupplierName",
                SupplierId = 1,
                DestinationBankAccountName = "AccountName",
                DestinationBankAccountNumber = "AccountNumber",
                DestinationBankCode = "BankCode",
                DestinationBankCurrencyCode = "CurrencyCode",
                DestinationBankCurrencyId = 1,
                DestinationBankCurrencySymbol = "CurrencySymbol",
                DestinationBankId = 1,
                DestinationBankName = "BankName",
                IsPosted = true ,
                AfterNominalValas=1,
                BeforeNominalValas=1,
                TransactionNominal=1,
                NominalValas=1,
                Receiver= "Receiver",
                

            };

            return TestData;
        }

        public DailyBankTransactionViewModel GetDataToValidate()
        {
            DailyBankTransactionViewModel TestData = new DailyBankTransactionViewModel()
            {
                Bank = new AccountBankViewModel()
                {
                    Id = 1,
                    AccountName = "AccountName",
                    AccountNumber = "AccountNumber",
                    BankCode = "BankCode",
                    BankName = "Name",
                    Code = "Code",
                    Currency = new CurrencyViewModel()
                    {
                        Id = 1,
                        Code = "Code",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Symbol"
                    }
                },
                AfterNominal = 0,
                BeforeNominal = 0,
                Buyer = new NewBuyerViewModel()
                {
                   Id = 1,
                  Code = "BuyerCode",
                  Name = "BuyerName"
                },
                Code = "Code",
                Date = DateTimeOffset.UtcNow,
                Nominal = 1000,
                ReferenceNo = "",
                ReferenceType = "ReferenceType",
                Remark = "Remark",
                SourceType = "Operasional",
                SourceFundingType = "Internal",
                Status = "IN",
                Supplier = new NewSupplierViewModel()
                {
                    _id = 1,
                    code = "SupplierCode",
                    name = "SupplierName"
                },
                OutputBank = new AccountBankViewModel()
                {
                    Id = 1,
                    AccountName = "AccountName",
                    AccountNumber = "AccountNumber",
                    BankCode = "BankCode",
                    BankName = "Name",
                    Code = "Code",
                    Currency = new CurrencyViewModel()
                    {
                        Id = 1,
                        Code = "Code",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Symbol"
                    }
                }
            };

            return TestData;
        }

        public async Task<DailyBankTransactionModel> GetTestDataIn()
        {
            DailyBankTransactionModel model = GetNewData();
            model.IsPosted = true;
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public async Task<DailyBankTransactionModel> GetTestDataOut()
        {
            DailyBankTransactionModel model = GetNewData();
            model.IsPosted = true;
            model.Status = "OUT";
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public async Task<DailyBankTransactionModel> GetTestDataNotPosted()
        {
            DailyBankTransactionModel model = GetNewData();
            model.IsPosted = false;
            model.Status = "OUT";
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public DailyBankTransactionModel GetNewData_InternalFunding()
        {
            DailyBankTransactionModel TestData = new DailyBankTransactionModel()
            {
                AccountBankAccountName = "AccountName",
                AccountBankAccountNumber = "AccountNumber",
                AccountBankCode = "BankCode",
                AccountBankCurrencyCode = "CurrencyCode",
                AccountBankCurrencyId = 1,
                AccountBankCurrencySymbol = "CurrencySymbol",
                AccountBankId = 1,
                AccountBankName = "BankName",
                AfterNominal = 0,
                BeforeNominal = 0,
                BuyerCode = "BuyerCode",
                BuyerId = 1,
                BuyerName = "BuyerName",
                Date = DateTimeOffset.UtcNow,
                Nominal = 1000,
                ReferenceNo = "",
                ReferenceType = "ReferenceType",
                Remark = "Remark",
                SourceType = "Operasional",
                Status = "IN",
                SupplierCode = "SupplierCode",
                SupplierName = "SupplierName",
                SupplierId = 1,
                DestinationBankAccountName = "AccountName",
                DestinationBankAccountNumber = "AccountNumber",
                DestinationBankCode = "BankCode",
                DestinationBankCurrencyCode = "CurrencyCode",
                DestinationBankCurrencyId = 1,
                DestinationBankCurrencySymbol = "CurrencySymbol",
                DestinationBankId = 1,
                DestinationBankName = "BankName",
                IsPosted = true,
                AfterNominalValas = 1,
                BeforeNominalValas = 1,
                TransactionNominal = 1,
                NominalValas = 1,
                Receiver = "Receiver",


            };

            return TestData;
        }
    }
}
