﻿using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using System;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.DailyBankTransaction
{
    public class DailyBankTransactionDataUtil
    {
        private readonly DailyBankTransactionService Service;

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
                ReferenceNo = "ReferenceNo",
                ReferenceType = "ReferenceType",
                Remark = "Remark",
                SourceType = "Operasional",
                Status = "IN",
                SupplierCode = "SupplierCode",
                SupplierName = "SupplierName",
                SupplierId = 1
            };

            return TestData;
        }

        public DailyBankTransactionViewModel GetDataToValidate()
        {
            DailyBankTransactionViewModel TestData = new DailyBankTransactionViewModel()
            {
                Bank = new NewAccountBankViewModel()
                {
                    Id = 1,
                    AccountName = "AccountName",
                    AccountNumber = "AccountNumber",
                    BankCode = "BankCode",
                    BankName = "Name",
                    Code = "Code",
                    Currency = new NewCurrencyViewModel()
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
                ReferenceNo = "ReferenceNo",
                ReferenceType = "ReferenceType",
                Remark = "Remark",
                SourceType = "Operasional",
                Status = "IN",
                Supplier = new NewSupplierViewModel()
                {
                    Id = 1,
                    Code = "SupplierCode",
                    Name = "SupplierName"
                }
            };

            return TestData;
        }

        public async Task<DailyBankTransactionModel> GetTestDataIn()
        {
            DailyBankTransactionModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public async Task<DailyBankTransactionModel> GetTestDataOut()
        {
            DailyBankTransactionModel model = GetNewData();
            model.Status = "OUT";
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
