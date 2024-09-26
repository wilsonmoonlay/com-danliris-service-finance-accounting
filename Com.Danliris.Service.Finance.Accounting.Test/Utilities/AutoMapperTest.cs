using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.DownPayment;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.Master;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.RealizationVBNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.RealizationVBWithPO;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.VBNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.VBWithPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePaymentViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using System.Collections.Generic;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Utilities
{
    public class AutoMapperTest
    {
        public AutoMapperTest()
        {
        }

        [Fact]
        public void Should_Success_Map_DownPayment()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<DownPaymentProfile>()).CreateMapper();
            var model = new DownPaymentModel();
            var vm = mapper.Map<DownPaymentViewModel>(model);
            Assert.NotNull(vm);
        }

        [Fact]
        public void Should_Success_Map_RealizationVb()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<RealizationVbNonPOProfile>()).CreateMapper();
            var model = new RealizationVbModel();
            var vm = mapper.Map<RealizationVbNonPOViewModel>(model);
            Assert.NotNull(vm);
        }

        [Fact]
        public void Should_Success_Map_RealizationVBWithPO()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<RealizationVBWithPOProfile>()).CreateMapper();
            var model = new RealizationVbModel();
            var vm = mapper.Map<RealizationVbWithPOViewModel>(model);
            Assert.NotNull(vm);
        }

        [Fact]
        public void Should_Success_Map_VbRequest()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<VBNonPORequestProfile>()).CreateMapper();
            var model = new VbRequestModel();
            var vm = mapper.Map<VbNonPORequestViewModel>(model);
            Assert.NotNull(vm);
        }

        [Fact]
        public void Should_Success_Map_DailyBankTransaction()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<DailyBankTransactionProfile>()).CreateMapper();
            var model = new DailyBankTransactionModel();
            var vm = mapper.Map<DailyBankTransactionViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_VBWithPORequest()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<VBWithPORequestProfile>()).CreateMapper();
            var model = new VbRequestModel();
            var vm = mapper.Map<VbWithPORequestViewModel>(model);
            Assert.NotNull(vm);
        }


        [Fact]
        public void Should_Success_Map_JournalTransaction()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<JournalTransactionProfile>()).CreateMapper();
            var model = new JournalTransactionModel()
            {
                Items = new List<JournalTransactionItemModel>()
                {
                    new JournalTransactionItemModel()
                }
            };
            var vm = mapper.Map<JournalTransactionViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_LockTransaction()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<LockTransactionProfile>()).CreateMapper();
            var model = new LockTransactionModel();
            var vm = mapper.Map<LockTransactionViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_COA()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<COAProfile>()).CreateMapper();
            var model = new COAModel();
            var vm = mapper.Map<COAViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_Payment_Disposition()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<PaymentDispositionNoteProfile>()).CreateMapper();
            var model = new PaymentDispositionNoteModel();
            var vm = mapper.Map<PaymentDispositionNoteViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_Purchasing_Disposition()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<PurchasingDispositionExpeditionProfile>()).CreateMapper();
            var model = new PurchasingDispositionExpeditionModel();
            var vm = mapper.Map<PurchasingDispositionExpeditionViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_Garment_Invoice_Payment()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<GarmentInvoicePaymentProfile>()).CreateMapper();
            var model = new GarmentInvoicePaymentModel();
            var vm = mapper.Map<GarmentInvoicePaymentViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_Garment_Finance_BankCashReceipt()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<BankCashReceiptProfile>()).CreateMapper();
            var model = new BankCashReceiptModel();
            var vm = mapper.Map<BankCashReceiptViewModel>(model);
            Assert.True(true);
        }

        [Fact]
        public void Should_Success_Map_Garment_Finance_BankCashReceiptDetail()
        {
            var mapper = new MapperConfiguration(configuration => configuration.AddProfile<BankCashReceiptDetailProfile>()).CreateMapper();
            var model = new BankCashReceiptDetailModel();
            var vm = mapper.Map<BankCashReceiptDetailViewModel>(model);
            Assert.True(true);
        }
    }
}
