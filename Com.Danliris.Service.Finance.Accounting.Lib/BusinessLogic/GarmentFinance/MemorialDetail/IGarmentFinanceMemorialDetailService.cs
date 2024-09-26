﻿using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.MemorialDetail
{
    public interface IGarmentFinanceMemorialDetailService
    {
        ReadResponse<GarmentFinanceMemorialDetailModel> Read(int page, int size, string order, List<string> select, string keyword, string filter = "{}");
        Task<int> CreateAsync(GarmentFinanceMemorialDetailModel model);
        Task<GarmentFinanceMemorialDetailModel> ReadByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(int id, GarmentFinanceMemorialDetailModel model);
    }
}
