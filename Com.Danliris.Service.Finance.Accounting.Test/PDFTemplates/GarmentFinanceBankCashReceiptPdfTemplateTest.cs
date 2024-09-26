using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.PDFTemplates
{
    public class GarmentFinanceBankCashReceiptPdfTemplateTest
    {
        public BankCashReceiptViewModel viewModelIDR
        {
            get
            {
                return new BankCashReceiptViewModel()
                {
                    ReceiptNo = "receiptNo",
                    Amount = 1.00M,
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                    {
                        Id = 1,
                        Code = "IDR",
                        Description = "description",
                        Rate = 1,
                        Symbol = "symbol",
                    },
                    NumberingCode = "numberingCode",
                    ReceiptDate = DateTimeOffset.Now,
                    Remarks = "remarks",
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    IncomeType = "incomeType",
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel()
                    {
                        AccountCOA = "AccountCOA",
                        AccountName = "AccountName",
                        AccountNumber = "AccountNumber",
                        BankCode = "BankCode",
                        BankName = "BankName",
                        Code = "Code",
                        Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Id = 1,
                            Rate = 1,
                            Symbol = "Rp"
                        },
                        Id = 1
                    },
                    BankCashReceiptType = new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id = 1,
                        Name = "PENJUALAN"
                    },
                    Id = 1,
                    Items = new List<BankCashReceiptItemViewModel>()
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            Id = 1,
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            Summary = 1,
                        }
                    }
                };
            }
        }

        public BankCashReceiptViewModel viewModelDollar
        {
            get
            {
                return new BankCashReceiptViewModel()
                {
                    ReceiptNo = "receiptNo",
                    Amount = 1.00M,
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                    {
                        Id = 1,
                        Code = "USD",
                        Description = "description",
                        Rate = 1,
                        Symbol = "symbol",
                    },
                    NumberingCode = "numberingCode",
                    ReceiptDate = DateTimeOffset.Now,
                    Remarks = "remarks",
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    IncomeType = "incomeType",
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel()
                    {
                        AccountCOA = "AccountCOA",
                        AccountName = "AccountName",
                        AccountNumber = "AccountNumber",
                        BankCode = "BankCode",
                        BankName = "BankName",
                        Code = "Code",
                        Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                        {
                            Code = "USD",
                            Description = "Description",
                            Id = 1,
                            Rate = 1,
                            Symbol = "$"
                        },
                        Id = 1
                    },
                    Id = 1,
                    BankCashReceiptType = new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id = 1,
                        Name = "LOKAL"
                    },
                    Items = new List<BankCashReceiptItemViewModel>()
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            Id = 1,
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            Summary = 1,
                        }
                    }
                };
            }
        }

        public BankCashReceiptViewModel viewModelIDRNonZero
        {
            get
            {
                return new BankCashReceiptViewModel()
                {
                    ReceiptNo = "receiptNo",
                    Amount = 1.12M,
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                    {
                        Id = 1,
                        Code = "IDR",
                        Description = "description",
                        Rate = 1,
                        Symbol = "symbol",
                    },
                    NumberingCode = "numberingCode",
                    ReceiptDate = DateTimeOffset.Now,
                    Remarks = "remarks",
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    IncomeType = "incomeType",
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel()
                    {
                        AccountCOA = "AccountCOA",
                        AccountName = "AccountName",
                        AccountNumber = "AccountNumber",
                        BankCode = "BankCode",
                        BankName = "BankName",
                        Code = "Code",
                        Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Id = 1,
                            Rate = 1,
                            Symbol = "Rp"
                        },
                        Id = 1
                    },
                    Id = 1,
                    BankCashReceiptType= new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id=1,
                        Name="PENJUALAN LOKAL"
                    },
                    Buyer = new Lib.ViewModels.NewIntegrationViewModel.NewBuyerViewModel
                    {
                        Id = 1,
                        Name = "name"
                    },
                    Items = new List<BankCashReceiptItemViewModel>()
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            Id = 1,
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            Summary = 1,
                        }
                    }
                };
            }
        }

        public BankCashReceiptViewModel viewModelDollarNonZero
        {
            get
            {
                return new BankCashReceiptViewModel()
                {
                    ReceiptNo = "receiptNo",
                    Amount = 2.12M,
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                    {
                        Id = 1,
                        Code = "USD",
                        Description = "description",
                        Rate = 1,
                        Symbol = "symbol",
                    },
                    NumberingCode = "numberingCode",
                    ReceiptDate = DateTimeOffset.Now,
                    Remarks = "remarks",
                    DebitCoa = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                    {
                        Id = "1",
                        Code = "code",
                        Name = "name",
                    },
                    IncomeType = "incomeType",
                    BankCashReceiptType = new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
                    {
                        Id = 1,
                        Name = "JUAL EKSPOR"
                    },
                    Buyer = new Lib.ViewModels.NewIntegrationViewModel.NewBuyerViewModel
                    {
                        Id = 1,
                        Name = "name"
                    },
                    Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel()
                    {
                        AccountCOA = "AccountCOA",
                        AccountName = "AccountName",
                        AccountNumber = "AccountNumber",
                        BankCode = "BankCode",
                        BankName = "BankName",
                        Code = "Code",
                        Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                        {
                            Code = "USD",
                            Description = "Description",
                            Id = 1,
                            Rate = 1,
                            Symbol = "$"
                        },
                        Id = 1
                    },
                    Id = 1,
                    Items = new List<BankCashReceiptItemViewModel>()
                    {
                        new BankCashReceiptItemViewModel()
                        {
                            Id = 1,
                            //AccAmount = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            AccNumber = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            AccSub = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            {
                                Id = "1",
                                Code = "code",
                                Name = "name",
                            },
                            //AccUnit = new Lib.ViewModels.NewIntegrationViewModel.ChartOfAccountViewModel()
                            //{
                            //    Id = "1",
                            //    Code = "code",
                            //    Name = "name",
                            //},
                            Amount = 1,
                            //C1A = 1,
                            //C1B = 1,
                            //C2A = 1,
                            //C2B = 1,
                            //C2C = 1,
                            NoteNumber = "noteNumber",
                            Remarks = "remarks",
                            Summary = 1,
                        }
                    }
                };
            }
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithCurrencyIDR()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(viewModelIDR, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithCurrencyUSD()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(viewModelDollar, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithCurrencyIDRNonZero()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(viewModelIDRNonZero, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithCurrencyUSDNonZero()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(viewModelDollarNonZero, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Success_TerbilangKoma_MoreThan_4_DecimalPlace()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var terbilangKoma = PdfTemplate.TerbilangKoma(123.45678);
            Assert.NotNull(terbilangKoma);
        }

        [Fact]
        public void Should_Success_TerbilangKoma()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var angka = "123,45678";
            var terbilangKoma = PdfTemplate.TerbilangKoma(Convert.ToDouble(angka));
            Assert.NotNull(terbilangKoma);
        }

        [Fact]
        public void Should_Success_TerbilangKoma_LessThanEqual_4_DecimalPlace()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var terbilangKoma = PdfTemplate.TerbilangKoma(1004);
            Assert.NotNull(terbilangKoma);
        }

        [Fact]
        public void Should_Success_Terbilang_Ribuan()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var terbilang = PdfTemplate.Terbilang(11325, "IDR");
            Assert.NotNull(terbilang);
        }

        public void Should_Success_Terbilang_Ratusan()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var terbilang = PdfTemplate.Terbilang(123, "IDR");
            Assert.NotNull(terbilang);
        }

        public void Should_Success_Terbilang_Puluham()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var terbilang = PdfTemplate.Terbilang(56, "IDR");
            Assert.NotNull(terbilang);
        }

        [Fact]
        public void Should_Success_Terbilang_IsNegative()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var terbilang = PdfTemplate.Terbilang(-1, "IDR");
            Assert.NotNull(terbilang);
        }

        [Fact]
        public void Should_Success_Terbilang_IsNegative_Decimal()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var terbilang = PdfTemplate.Terbilang(-1.2, "USD");
            Assert.NotNull(terbilang);
        }

        [Fact]
        public void Should_Success_Terbilang_Decimal()
        {
            GarmentFinanceBankCashReceiptPdfTemplate PdfTemplate = new GarmentFinanceBankCashReceiptPdfTemplate();
            var angka = "123,456";
            var terbilang = PdfTemplate.Terbilang(Convert.ToDouble(angka), "USD");
            Assert.NotNull(terbilang);
        }
    }
}
