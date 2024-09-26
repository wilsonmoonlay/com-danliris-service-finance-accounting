using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceiptDetailLocal
{
    public class GarmentFinanceBankCashReceiptDetailLocalDataUtil
    {
        private readonly GarmentFinanceBankCashReceiptDetailLocalService Service;
        private readonly BankCashReceiptDataUtil BankCashReceiptDataUtil;
        public GarmentFinanceBankCashReceiptDetailLocalDataUtil(GarmentFinanceBankCashReceiptDetailLocalService service, BankCashReceiptDataUtil bankCashReceiptDataUtil)
        {
            Service = service;
            BankCashReceiptDataUtil = bankCashReceiptDataUtil;
        }

        public GarmentFinanceBankCashReceiptDetailLocalModel GetNewData()
        {
            var bankCashReceipt = Task.Run(() => BankCashReceiptDataUtil.GetTestData()).Result;
            return new GarmentFinanceBankCashReceiptDetailLocalModel
            {
                BankCashReceiptId = bankCashReceipt.Id,
                BankCashReceiptDate = bankCashReceipt.ReceiptDate,
                BankCashReceiptNo = bankCashReceipt.ReceiptNo,
                Amount = 2,
                Items = new List<GarmentFinanceBankCashReceiptDetailLocalItemModel>
                {
                    new GarmentFinanceBankCashReceiptDetailLocalItemModel()
                    {
                        Amount = 2,
                        BankCashReceiptDetailLocalId = 1,
                        BuyerCode = "code",
                        BuyerId = 1,
                        BuyerName = "name",
                        CurrencyCode = "code",
                        CurrencyId = 1,
                        CurrencyRate = 1,
                        LocalSalesNoteId = 1,
                        LocalSalesNoteNo = "no",
                    }
                },
                OtherItems = new List<GarmentFinanceBankCashReceiptDetailLocalOtherItemModel>
                {
                    new GarmentFinanceBankCashReceiptDetailLocalOtherItemModel()
                    {
                        ChartOfAccountId = 1,
                        ChartOfAccountCode = "Code",
                        ChartOfAccountName = "Name",
                        BankCashReceiptDetailLocalId = 1,
                        Amount = 1,
                        CurrencyId = 1,
                        CurrencyCode = "code",
                        CurrencyRate = 1,
                        Remarks = "remarks",
                        TypeAmount = "KREDIT"

                    },
                    new GarmentFinanceBankCashReceiptDetailLocalOtherItemModel()
                    {
                        ChartOfAccountId = 1,
                        ChartOfAccountCode = "Code",
                        ChartOfAccountName = "Name",
                        BankCashReceiptDetailLocalId = 1,
                        Amount = 1,
                        CurrencyId = 1,
                        CurrencyCode = "code",
                        CurrencyRate = 1,
                        Remarks = "remarks",
                        TypeAmount = "DEBIT"

                    }
                }
            };
        }

        public async Task<GarmentFinanceBankCashReceiptDetailLocalModel> GetTestData()
        {
            GarmentFinanceBankCashReceiptDetailLocalModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
