using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels
{
    public class VMValidationTest
    {
        [Fact]
        public void Should_Success_Validate_Garment_Invoice_Purchasing_Disposition1()
        {
            var vm = new GarmentInvoicePurchasingDispositionViewModel()
            {
                AccountBank = new AccountBankViewModel(),
                Supplier = new SupplierViewModel(),
                Items = new List<GarmentInvoicePurchasingDispositionItemViewModel>()
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Garment_Invoice_Purchasing_Disposition2()
        {
            var vm = new GarmentInvoicePurchasingDispositionViewModel()
            {
                AccountBank = new AccountBankViewModel()
                {
                    Id = 1,
                    Currency = new CurrencyViewModel()
                    {
                        Code = "IDR"
                    }
                },
                Supplier = new SupplierViewModel(),
                Items = new List<GarmentInvoicePurchasingDispositionItemViewModel>() { new GarmentInvoicePurchasingDispositionItemViewModel() },
                PaymentDate = DateTimeOffset.Now.AddMonths(1)
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Shoul_Success_Build_Garment_Invoice_Disposition_Posting()
        {
            var vm = new GarmentInvoicePurchasingDispositionPostingViewModel()
            {
                ListIds = new List<GarmentInvoicePurchasingDispositionPostingIdViewModel>()
            };

            Assert.NotNull(vm.ListIds);

        }

        [Fact]
        public void Shoul_Success_Build_Garment_Invoice_Disposition_Posting_Id()
        {
            var vm = new GarmentInvoicePurchasingDispositionPostingIdViewModel()
            {
                Id = 1
            };

            Assert.NotEqual(0, vm.Id);

        }

        [Fact]
        public void Should_Success_Validate_Garment_Invoice_Purchasing_Disposition_Item()
        {
            var vm = new GarmentInvoicePurchasingDispositionItemViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Shoul_Success_Build_Detail_Rincian()
        {
            var vm = new DetailRincian()
            {
                AccountingBookId = 1,
                Id = 1,
                AccountingBookType = "Test",
                GarmentCurrenciesCode = "Code",
                GarmentCurrenciesId = 1,
                GarmentCurrenciesRate = 1,
                IsPosted = false,
                Items = new List<DetailRincianItems>()
                {
                    new DetailRincianItems()
                    {
                        BillsNo = "BillsNo",
                        CurrencyCode = "Code",
                        GarmentDeliveryOrderId = 1,
                        GarmentDeliveryOrderNo = "DONo",
                        Id = 1,
                        InternalNoteNo = "INNo",
                        MemoAmount = 1,
                        MemoIdrAmount = 1,
                        PaymentBills = "Test",
                        PaymentRate = 1,
                        PurchasingRate = 1,
                        RemarksDetail ="Remarks",
                        SaldoAkhir = 1,
                        SupplierCode = "Code"
                    }
                },
                MemoDate = DateTimeOffset.Now,
                MemoId = 1,
                MemoNo = "MemoNo",
                Remarks = "Remarks"
            };

            Assert.NotNull(vm);
        }

        [Fact]
        public void Shoul_Success_Build_Disposition()
        {
            var vm = new DispositionDto()
            {
                DispositionId = 1,
                DispositionNo = "DispositionNo",
                MemoDetails = new List<MemoDetail>()
                {
                    new MemoDetail()
                    {
                        BillsNo = "BillsNo",
                        CurrencyCode = "Code",
                        GarmentDeliveryOrderId = 1,
                        GarmentDeliveryOrderNo = "DONo",
                        InternalNoteNo = "INNo",
                        MemoAmount  = 1,
                        MemoIdrAmount = 1,
                        PaymentBills = "Bills",
                        PaymentRate = 1,
                        PurchaseAmount = 1,
                        PurchasingRate = 1,
                        RemarksDetail ="Remarks",
                        SupplierCode = "SupplierCode",
                        SupplierName = "SupplierName"
                    }
                }
            };

            Assert.NotNull(vm);
        }

        [Fact]
        public void Shoul_Success_Build_EditDetailRincian()
        {
            var vm = new EditDetailRincian()
            {
                AccountingBookId = 1,
                AccountingBookType = "Type",
                GarmentCurrenciesCode = "Code",
                GarmentCurrenciesId = 1,
                GarmentCurrenciesRate = 1,
                IsPosted = false,
                Items = new List<EditDetailRincianItems>()
                {
                    new EditDetailRincianItems()
                    {
                        GarmentDeliveryOrderId = 1,
                        GarmentDeliveryOrderNo = "DONo",
                        Id = 1,
                        MemoAmount = 1,
                        MemoIdrAmount = 1,
                        PaymentRate = 1,
                        PurchasingRate = 1,
                        RemarksDetail = ""
                    }
                },
                MemoDate = DateTimeOffset.Now,
                MemoId = 1,
                MemoNo = "No",
                Remarks = "Remarks"
            };

            Assert.NotNull(vm);
        }

        [Fact]
        public void Shoul_Success_Build_ListMemoDetail()
        {
            var vm = new ListMemoDetail()
            {
                AccountingBookId = 1,
                AccountingBookType = "Type",
                GarmentCurrenciesCode = "Code",
                GarmentCurrenciesId = 1,
                Id = 1,
                GarmentCurrenciesRate = 1,
                IsPosted = false,
                MemoDate = DateTimeOffset.Now,
                MemoId = 1,
                MemoNo = "MemoNo",
                Remarks = "Remarks"
            };

            Assert.NotNull(vm);
        }

        [Fact]
        public void Shoul_Success_Build_MemoDetail()
        {
            var vm = new MemoDetail()
            {
                BillsNo = "BillsNo",
                CurrencyCode = "Code",
                GarmentDeliveryOrderId = 1,
                GarmentDeliveryOrderNo = "DONo",
                InternalNoteNo = "INNo",
                MemoAmount = 1,
                MemoIdrAmount = 1,
                PaymentBills = "Bills",
                PaymentRate = 1,
                PurchaseAmount = 1,
                PurchasingRate = 1,
                RemarksDetail = "Remarlks",
                SupplierCode = "Code",
                SupplierName = "Name"
            };

            Assert.NotNull(vm);
        }

        [Fact]
        public void Should_Success_Validate_Memo_Detail_Garment_Purchasing()
        {
            var vm = new MemoDetailGarmentPurchasingViewModel()
            {
                MemoDetailGarmentPurchasingDispositions = new List<MemoDetailGarmentPurchasingDispositionViewModel>()
                {
                    new MemoDetailGarmentPurchasingDispositionViewModel()
                    {
                        Disposition = new DispositionDto()
                    }
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }
    }
}
