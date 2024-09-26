using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument.Helper
{
    public class AutoJournalServiceTestHelper : IAutoJournalService
    {
        public async Task<int> AutoJournalFromOthersExpenditureProof(OthersExpenditureProofDocumentCreateUpdateViewModel viewModel, string documentNo)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalReverseFromOthersExpenditureProof(string documentNo)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalInklaring(List<int> vbRequestIds)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalVBNonPOClearence(List<int> vbRealizationIds)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalVBNonPOClearence(List<int> vbRealizationIds, AccountBankViewModel bank)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalInklaring(List<int> vbRequestIds, AccountBankViewModel bank)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalFromOthersExpenditureProof(OthersExpenditureProofDocumentModel model, List<OthersExpenditureProofDocumentItemModel> items)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public string DocumentNoGenerator(AccountBankViewModel bank)
        {
            return string.Empty;
        }

        public async Task<int> AutoJournalVBNonPOClearence(List<int> vbRealizationIds, AccountBankViewModel bank, string referenceNo)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalFromDailyBankTransaction(DailyBankTransactionModel model, AccountBank accountBank, AccountBank accountBankDestination)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }

        public async Task<int> AutoJournalFromDisposition(PaymentDispositionNoteModel model, string Username, string UserAgent)
        {
            await Task.Delay(1000);
            return await Task.FromResult(1);
        }
    }
}
