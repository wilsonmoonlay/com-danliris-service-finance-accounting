using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.Memorial
{
    public class GarmentFinanceMemorialDataUtil
    {
        private readonly GarmentFinanceMemorialService Service;

        public GarmentFinanceMemorialDataUtil(GarmentFinanceMemorialService service)
        {
            Service = service;
        }

        public GarmentFinanceMemorialModel GetNewData()
        {
            return new GarmentFinanceMemorialModel
            {
                MemorialNo = "no",
                Remark = "remarks",
                Date = DateTimeOffset.Now,
                AccountingBookId = 1,
                AccountingBookCode = "code",
                AccountingBookType = "name",
                GarmentCurrencyCode = "number",
                GarmentCurrencyId = 1,
                GarmentCurrencyRate = 1,
                IsUsed=false,
                Items = new List<GarmentFinanceMemorialItemModel>
                {
                    new GarmentFinanceMemorialItemModel()
                    {
                        COAId = 1,
                        COACode = "code",
                        COAName = "name",
                        Credit = 0,
                        Debit = 1,

                    },
                    new GarmentFinanceMemorialItemModel()
                    {
                        COAId = 1,
                        COACode = "code",
                        COAName = "name",
                        Credit = 1,
                        Debit = 0,
                    },
                }
            };
        }

        public async Task<GarmentFinanceMemorialModel> GetTestData()
        {
            GarmentFinanceMemorialModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
