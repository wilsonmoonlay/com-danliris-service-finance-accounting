using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.MemorialDetail
{
    public class GarmentFinanceMemorialDetailDataUtil
    {
        private readonly GarmentFinanceMemorialDetailService Service;
        private readonly GarmentFinanceMemorialDataUtil MemorialDataUtil;

        public GarmentFinanceMemorialDetailDataUtil(GarmentFinanceMemorialDetailService service, GarmentFinanceMemorialDataUtil memorialDataUtil)
        {
            Service = service;
            MemorialDataUtil = memorialDataUtil;
        }

        public GarmentFinanceMemorialDetailModel GetNewData()
        {
            var memorial = Task.Run(() => MemorialDataUtil.GetTestData()).Result;
            return new GarmentFinanceMemorialDetailModel
            {
                MemorialNo = memorial.MemorialNo,
                MemorialId = memorial.Id,
                MemorialDate = DateTimeOffset.Now,
                Amount = 2,
                DebitCoaId = 1,
                DebitCoaCode = "code",
                DebitCoaName = "name",
                InvoiceCoaId = 1,
                InvoiceCoaCode = "code",
                InvoiceCoaName = "name",
                Items = new List<GarmentFinanceMemorialDetailItemModel>
                {
                    new GarmentFinanceMemorialDetailItemModel()
                    {
                        BuyerId = 1,
                        BuyerCode = "code",
                        BuyerName = "name",
                        Amount = 1,
                        CurrencyId = 1,
                        CurrencyCode="code",
                        CurrencyRate=1,
                        InvoiceId=1,
                        InvoiceNo="no",

                    },
                },
                OtherItems = new List<GarmentFinanceMemorialDetailOtherItemModel>
                {
                    new GarmentFinanceMemorialDetailOtherItemModel()
                    {
                        Amount = 1,
                        ChartOfAccountId = 1,
                        ChartOfAccountCode = "code",
                        ChartOfAccountName = "name",
                        CurrencyId = 1,
                        CurrencyCode = "code",
                        CurrencyRate = 1,
                        TypeAmount = "KREDIT",
                        Remarks = "remarks"

                    },
                    new GarmentFinanceMemorialDetailOtherItemModel()
                    {
                        Amount = 1,
                        ChartOfAccountId = 1,
                        ChartOfAccountCode = "code",
                        ChartOfAccountName = "name",
                        CurrencyId = 1,
                        CurrencyCode = "code",
                        CurrencyRate = 1,
                        TypeAmount = "DEBIT",
                        Remarks = "remarks"

                    },
                },
                RupiahItems = new List<GarmentFinanceMemorialDetailRupiahItemModel>
                {
                    new GarmentFinanceMemorialDetailRupiahItemModel()
                    {
                        ChartOfAccountId = 1,
                        ChartOfAccountCode = "code",
                        ChartOfAccountName = "name",
                        Debit = 1,
                        Credit = 1,
                    }
                }
            };
        }

        public async Task<GarmentFinanceMemorialDetailModel> GetTestData()
        {
            GarmentFinanceMemorialDetailModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
