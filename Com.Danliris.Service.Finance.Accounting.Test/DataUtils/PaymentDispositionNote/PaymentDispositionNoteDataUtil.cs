using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.PaymentDispositionNote
{
    public class PaymentDispositionNoteDataUtil
    {
        private readonly PaymentDispositionNoteService Service;
        private readonly PurchasingDispositionExpeditionDataUtil ExpeditionData;

        public PaymentDispositionNoteDataUtil(PaymentDispositionNoteService service, PurchasingDispositionExpeditionDataUtil expeditionData)
        {
            Service = service;
            ExpeditionData = expeditionData;
        }

        public PaymentDispositionNoteModel GetNewData()
        {
            var datas = Task.Run(() => ExpeditionData.GetTestData()).Result;
            long nowTicks = DateTimeOffset.Now.Ticks;
            string nowTicksA = $"{nowTicks}a";
            string nowTicksB = $"{nowTicks}b";
            return new PaymentDispositionNoteModel()
            {
                SupplierImport = true,
                SupplierCode = nowTicksA,
                SupplierName = nowTicksA,
                SupplierId = 1,
                BankCurrencyCode = nowTicksA,
                BankCurrencyId = 1,
                BankCurrencyRate = 1,
                BankAccountName = nowTicksA,
                BankAccountNumber = nowTicksA,
                BankCode = nowTicksA,
                BankId = 1,
                BankName = nowTicksA,
                BankAccountCOA = nowTicksA,
                TransactionType = "Any",
                BGCheckNumber = nowTicksA,
                Amount = 1000,
                PaymentDate = DateTimeOffset.Now,

                Items = new List<PaymentDispositionNoteItemModel>
                {
                    new PaymentDispositionNoteItemModel
                    {
                        PurchasingDispositionExpeditionId=datas.Id,
                        CategoryCode=datas.CategoryId,
                        CategoryId=1,
                        CategoryName=datas.CategoryName,
                        DispositionDate=DateTimeOffset.Now,
                        DispositionId=1,
                        DispositionNo=datas.DispositionNo,
                        DivisionCode=datas.DivisionCode,
                        DivisionId=1,
                        DivisionName=datas.DivisionName,
                        DPP=datas.DPP,
                        VatValue=datas.DPP,
                        IncomeTaxValue=datas.IncomeTaxValue,
                        ProformaNo=datas.ProformaNo,
                        TotalPaid=datas.TotalPaid,
                        Details= new List<PaymentDispositionNoteDetailModel>
                        {
                            new PaymentDispositionNoteDetailModel
                            {
                                ProductCode=datas.Items.First().ProductCode,
                                Price=datas.Items.First().Price,
                                ProductId=1,
                                ProductName=datas.Items.First().ProductName,
                                UnitCode=datas.Items.First().UnitCode,
                                UnitId=1,
                                UnitName=datas.Items.First().UnitName,
                                UomId=1,
                                UomUnit=datas.Items.First().UomUnit,
                                Quantity=datas.Items.First().Quantity,
                                PurchasingDispositionDetailId=datas.Items.First().PurchasingDispositionDetailId,
                                PurchasingDispositionExpeditionItemId=datas.Items.First().Id,
                                EPOId=datas.Items.First().EPOId
                            }
                        }

                    },
                    new PaymentDispositionNoteItemModel
                    {
                        PurchasingDispositionExpeditionId=datas.Id,
                        CategoryCode=datas.CategoryId,
                        CategoryId=1,
                        CategoryName=datas.CategoryName,
                        DispositionDate=DateTimeOffset.Now,
                        DispositionId=1,
                        DispositionNo=datas.DispositionNo,
                        DivisionCode=datas.DivisionCode,
                        DivisionId=1,
                        DivisionName=datas.DivisionName,
                        DPP=datas.DPP,
                        VatValue=datas.DPP,
                        IncomeTaxValue=datas.IncomeTaxValue,
                        ProformaNo=datas.ProformaNo,
                        TotalPaid=datas.TotalPaid,
                        Details= new List<PaymentDispositionNoteDetailModel>
                        {
                            new PaymentDispositionNoteDetailModel
                            {
                                ProductCode=datas.Items.First().ProductCode,
                                Price=datas.Items.First().Price,
                                ProductId=1,
                                ProductName=datas.Items.First().ProductName,
                                UnitCode=datas.Items.First().UnitCode,
                                UnitId=1,
                                UnitName=datas.Items.First().UnitName,
                                UomId=1,
                                UomUnit=datas.Items.First().UomUnit,
                                Quantity=datas.Items.First().Quantity,
                                PurchasingDispositionDetailId=datas.Items.First().PurchasingDispositionDetailId,
                                PurchasingDispositionExpeditionItemId=datas.Items.First().Id,
                                EPOId=datas.Items.First().EPOId
                            }
                        }

                    }
                }
            };
        }

        public PaymentDispositionNoteViewModel GetNewDataViewModel()
        {
            var datas = Task.Run(() => ExpeditionData.GetTestData()).Result;
            long nowTicks = DateTimeOffset.Now.Ticks;
            string nowTicksA = $"{nowTicks}a";
            string nowTicksB = $"{nowTicks}b";
            return new PaymentDispositionNoteViewModel()
            {
                PaymentDispositionNo = "PaymentDispositionNo",
                Supplier = new Lib.ViewModels.NewIntegrationViewModel.SupplierViewModel
                {
                    Code = "Code",
                    Name = "Name"
                },
                AccountBank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel
                {
                    BankName = "BankName",
                    AccountNumber = "AccountNumber",
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                    {
                        Code = "Code"
                    }
                },
                CurrencyCode = "CurrencyCode",
                CurrencyRate = 1,
                CurrencyId = 1,
                BankAccountCOA = nowTicksA,
                TransactionType = "Any",
                BGCheckNumber = nowTicksA,
                Amount = 1000,
                PaymentDate = DateTimeOffset.Now,

                Items = new List<PaymentDispositionNoteItemViewModel>
                {
                    new PaymentDispositionNoteItemViewModel
                    {
                        purchasingDispositionExpeditionId = 1,
                        SupplierPayment = 1000,
                        AmountPaid = 1000,
                        dispositionNo = "dispositionNo",
                        category = new Lib.ViewModels.IntegrationViewModel.CategoryViewModel
                        {
                            name = "name"
                        },
                        division = new Lib.ViewModels.IntegrationViewModel.DivisionViewModel
                        {
                            name = "name"
                        },
                        Details= new List<PaymentDispositionNoteDetailViewModel>
                        {
                            new PaymentDispositionNoteDetailViewModel
                            {
                                price = 1000,
                                unit = new Lib.ViewModels.IntegrationViewModel.UnitViewModel
                                {
                                    code = "code"
                                }
                            }
                        }

                    },
                    new PaymentDispositionNoteItemViewModel
                    {
                        purchasingDispositionExpeditionId = 1,
                        SupplierPayment = 1000,
                        AmountPaid = 1000,
                        dispositionNo = "dispositionNo",
                        category = new Lib.ViewModels.IntegrationViewModel.CategoryViewModel
                        {
                            name = "name"
                        },
                        division = new Lib.ViewModels.IntegrationViewModel.DivisionViewModel
                        {
                            name = "name"
                        },
                        Details= new List<PaymentDispositionNoteDetailViewModel>
                        {
                            new PaymentDispositionNoteDetailViewModel
                            {
                                price = 1000,
                                unit = new Lib.ViewModels.IntegrationViewModel.UnitViewModel
                                {
                                    code = "code"
                                }
                            },
                            new PaymentDispositionNoteDetailViewModel
                            {
                                price = 1000,
                                unit = new Lib.ViewModels.IntegrationViewModel.UnitViewModel
                                {
                                    code = "code2"
                                }
                            }
                        }

                    }
                }
            };
        }

        public PaymentDispositionNoteViewModel GetNewDataViewModelIDRNONIDR()
        {
            var datas = Task.Run(() => ExpeditionData.GetTestData()).Result;
            long nowTicks = DateTimeOffset.Now.Ticks;
            string nowTicksA = $"{nowTicks}a";
            string nowTicksB = $"{nowTicks}b";
            return new PaymentDispositionNoteViewModel()
            {
                PaymentDispositionNo = "PaymentDispositionNo",
                Supplier = new Lib.ViewModels.NewIntegrationViewModel.SupplierViewModel
                {
                    Code = "Code",
                    Name = "Name"
                },
                AccountBank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel
                {
                    BankName = "BankName",
                    AccountNumber = "AccountNumber",
                    Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                    {
                        Code = "IDR"
                    }
                },
                CurrencyCode = "CurrencyCode",
                CurrencyRate = 1,
                CurrencyId = 1,
                BankAccountCOA = nowTicksA,
                TransactionType = "Any",
                BGCheckNumber = nowTicksA,
                Amount = 1000,
                PaymentDate = DateTimeOffset.Now,

                Items = new List<PaymentDispositionNoteItemViewModel>
                {
                    new PaymentDispositionNoteItemViewModel
                    {
                        purchasingDispositionExpeditionId = 1,
                        SupplierPayment = 1000,
                        AmountPaid = 1000,
                        dispositionNo = "dispositionNo",
                        category = new Lib.ViewModels.IntegrationViewModel.CategoryViewModel
                        {
                            name = "name"
                        },
                        division = new Lib.ViewModels.IntegrationViewModel.DivisionViewModel
                        {
                            name = "name"
                        },
                        Details= new List<PaymentDispositionNoteDetailViewModel>
                        {
                            new PaymentDispositionNoteDetailViewModel
                            {
                                price = 1000,
                                unit = new Lib.ViewModels.IntegrationViewModel.UnitViewModel
                                {
                                    code = "code"
                                }
                            }
                        }

                    },
                    new PaymentDispositionNoteItemViewModel
                    {
                        purchasingDispositionExpeditionId = 1,
                        SupplierPayment = 1000,
                        AmountPaid = 1000,
                        dispositionNo = "dispositionNo",
                        category = new Lib.ViewModels.IntegrationViewModel.CategoryViewModel
                        {
                            name = "name"
                        },
                        division = new Lib.ViewModels.IntegrationViewModel.DivisionViewModel
                        {
                            name = "name"
                        },
                        Details= new List<PaymentDispositionNoteDetailViewModel>
                        {
                            new PaymentDispositionNoteDetailViewModel
                            {
                                price = 1000,
                                unit = new Lib.ViewModels.IntegrationViewModel.UnitViewModel
                                {
                                    code = "code"
                                }
                            },
                            new PaymentDispositionNoteDetailViewModel
                            {
                                price = 1000,
                                unit = new Lib.ViewModels.IntegrationViewModel.UnitViewModel
                                {
                                    code = "code2"
                                }
                            }
                        }

                    }
                }
            };
        }

        public async Task<PaymentDispositionNotePostDto> GetNewPostDto()
        {
            PaymentDispositionNoteModel model = GetNewData();
            await Service.CreateAsync(model);
            var read = await Service.ReadByIdAsync((int)model.Id);
            return new PaymentDispositionNotePostDto
                {
                    ListIds = new List<PaymentDispositionNotePostIdDto>
                    {
                        new PaymentDispositionNotePostIdDto{ Id = read.Id }
                    }
                };            
        }

        public async Task<PaymentDispositionNoteModel> GetTestData()
        {
            PaymentDispositionNoteModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        public async Task<PaymentDispositionNoteViewModel> GetPDFData()
        {
            PaymentDispositionNoteModel model = GetNewData();
            PaymentDispositionNoteViewModel viewModel = GetNewDataViewModel();
            await Service.CreateAsync(model);
            return viewModel;
        }

        public async Task<PaymentDispositionNoteViewModel> GetPDFDataIDRNONIDR()
        {
            PaymentDispositionNoteModel model = GetNewData();
            PaymentDispositionNoteViewModel viewModel = GetNewDataViewModelIDRNONIDR();
            await Service.CreateAsync(model);
            return viewModel;
        }
    }
}
