using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.MemorialDetailLocal
{
    public class GarmentFinanceMemorialDetailLocalDataUtil
    {
        private readonly GarmentFinanceMemorialDetailLocalService Service;
        private readonly GarmentFinanceMemorialDataUtil MemorialDataUtil;

        public GarmentFinanceMemorialDetailLocalDataUtil(GarmentFinanceMemorialDetailLocalService service, GarmentFinanceMemorialDataUtil memorialDataUtil)
        {
            Service = service;
            MemorialDataUtil = memorialDataUtil;
        }

        public GarmentFinanceMemorialDetailLocalModel GetNewData()
        {
            var memorial = Task.Run(() => MemorialDataUtil.GetTestData()).Result;
            return new GarmentFinanceMemorialDetailLocalModel
            {
                MemorialNo = memorial.MemorialNo,
                MemorialId = memorial.Id,
                MemorialDate = DateTimeOffset.Now,
                Amount = 1,
                DebitCoaId = 1,
                DebitCoaCode = "code",
                DebitCoaName = "name",
                InvoiceCoaId = 1,
                InvoiceCoaCode = "code",
                InvoiceCoaName = "name",
                Items = new List<GarmentFinanceMemorialDetailLocalItemModel>
                {
                    new GarmentFinanceMemorialDetailLocalItemModel()
                    {
                        BuyerId = 1,
                        BuyerCode = "code",
                        BuyerName = "name",
                        Amount = 1,
                        CurrencyId = 1,
                        CurrencyCode="code",
                        CurrencyRate=1,
                        LocalSalesNoteId=1,
                        LocalSalesNoteNo="no",
                    },
                },
                OtherItems = new List<GarmentFinanceMemorialDetailLocalOtherItemModel>
                {
                    new GarmentFinanceMemorialDetailLocalOtherItemModel()
                    {
                        Amount = 1,
                        ChartOfAccountId = 1,
                        ChartOfAccountCode = "code",
                        ChartOfAccountName = "name",
                        CurrencyId = 1,
                        CurrencyCode = "code",
                        CurrencyRate = 1,
                        Remarks = "remarks",
                        TypeAmount = "KREDIT",
                    }
                }
            };
        }

        public async Task<GarmentFinanceMemorialDetailLocalModel> GetTestData()
        {
            GarmentFinanceMemorialDetailLocalModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
