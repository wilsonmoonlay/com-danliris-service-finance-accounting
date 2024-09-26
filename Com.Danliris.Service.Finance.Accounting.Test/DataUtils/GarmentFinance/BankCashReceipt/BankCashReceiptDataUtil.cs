using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptDataUtil
    {
        private readonly BankCashReceiptService Service;

        public BankCashReceiptDataUtil(BankCashReceiptService service)
        {
            Service = service;
        }

        public BankCashReceiptModel GetNewData()
        {
            return new BankCashReceiptModel
            {
                ReceiptNo = "no",
                Remarks = "remarks",
                ReceiptDate = DateTimeOffset.Now,
                Amount = 1,
                BankAccountId = 1,
                BankAccountingCode = "code",
                BankAccountName = "name",
                BankAccountNumber = "number",
                BankCurrencyCode ="code",
                BankCurrencyId = 1,
                BankCurrencyRate = 1,
                BankAccountCOA = "accountCOA",
                BankName = "name",
                DebitCoaCode = "code",
                DebitCoaId = 1,
                DebitCoaName = "name",
                IncomeType = "type",
                NumberingCode = "code",
                CurrencyId = 1,
                CurrencyRate = 1,
                CurrencyCode = "IDR",
                BankCashReceiptTypeCoaCode="11",
                BankCashReceiptTypeCoaName="name",
                BankCashReceiptTypeCoaId=1,
                BankCashReceiptTypeId=1,
                BankCashReceiptTypeName="PENJUALAN LOKAL",
                BuyerCode="code",
                BuyerId=1,
                BuyerName="name",
                
                Items = new List<BankCashReceiptItemModel>
                {
                    new BankCashReceiptItemModel()
                    {
                        BankCashReceiptId = 1,
                        //AccAmountCoaCode = "code",
                        //AccAmountCoaId = 1,
                        //AccAmountCoaName = "name",
                        AccNumberCoaCode = "code",
                        AccNumberCoaId = 1,
                        AccNumberCoaName = "name",
                        AccSubCoaCode = "code",
                        AccSubCoaId = 1,
                        AccSubCoaName = "name",
                        //AccUnitCoaCode = "code",
                        //AccUnitCoaId = 1,
                        //AccUnitCoaName = "name",
                        //C1A = 1,
                        //C1B = 1,
                        //C2A = 1,
                        //C2B = 1,
                        //C2C = 1,
                        NoteNumber = "1",
                        Remarks = "remarks",
                        Amount = 1,
                        Summary = 5,
                        
                    },
                    new BankCashReceiptItemModel()
                    {
                        BankCashReceiptId = 2,
                        //AccAmountCoaCode = "code",
                        //AccAmountCoaId = 1,
                        //AccAmountCoaName = "name",
                        AccNumberCoaCode = "code",
                        AccNumberCoaId = 1,
                        AccNumberCoaName = "name",
                        AccSubCoaCode = "code",
                        AccSubCoaId = 1,
                        AccSubCoaName = "name",
                        //AccUnitCoaCode = "code",
                        //AccUnitCoaId = 1,
                        //AccUnitCoaName = "name",
                        //C1A = 1,
                        //C1B = 1,
                        //C2A = 1,
                        //C2B = 1,
                        //C2C = 1,
                        NoteNumber = "1",
                        Remarks = "remarks",
                        Amount = 1,
                        Summary = 5,

                    },
                }
            };
        }

        public async Task<BankCashReceiptModel> GetTestData_LOKAL()
        {
            BankCashReceiptModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public async Task<BankCashReceiptModel> GetTestData_EKSPOR()
        {
            BankCashReceiptModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public async Task<BankCashReceiptModel> GetTestData()
        {
            BankCashReceiptModel model = GetNewData();
            model.BankCashReceiptTypeName = "LAIN-LAIN";
            
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
