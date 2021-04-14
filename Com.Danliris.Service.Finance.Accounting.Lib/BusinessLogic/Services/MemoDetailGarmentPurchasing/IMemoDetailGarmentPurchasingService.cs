using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoDetailGarmentPurchasing
{
    public interface IMemoDetailGarmentPurchasingService
    {
        DetailRincian GetDetailById(int memoId);
        Task<int> CreateAsync(MemoDetailGarmentPurchasingModel model);
        ReadResponse<ListMemoDetail> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        ReadResponse<ReportRincian> GetReport(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas);
        Task<int> UpdateAsync(int id, EditDetailRincian viewModel);
        Task<int> DeleteAsync(int id);
        ReadResponse<ReportPDF> GetPDF(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas);

    }
}
