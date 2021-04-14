using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoDetailGarmentPurchasing
{
    public class MemoDetailGarmentPurchasingService : IMemoDetailGarmentPurchasingService
    {
        private const string UserAgent = "finance-service";
        public FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        protected DbSet<MemoDetailGarmentPurchasingModel> DbSet;
        protected DbSet<MemoDetailGarmentPurchasingDetailModel> DbSetDetail;

        public MemoDetailGarmentPurchasingService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;

            DbSet = _dbContext.Set<MemoDetailGarmentPurchasingModel>();
            DbSetDetail = _dbContext.Set<MemoDetailGarmentPurchasingDetailModel>();
        }

        public void CreateModel(MemoDetailGarmentPurchasingModel model)
        {
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
           

            foreach (var item in model.MemoDetailGarmentPurchasingDetail)
            {
                EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
            }
            DbSet.Add(model);
        }

        public async Task<int> CreateAsync(MemoDetailGarmentPurchasingModel model)
        {
            try
            {
                CreateModel(model);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReadResponse<ListMemoDetail> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            try
            {
                var query = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();

                var searchAttributes = new List<string>
                {
                    "MemoNo", "AccountingBookType", "GarmentCurrenciesCode"
                };

                query = QueryHelper<MemoDetailGarmentPurchasingModel>.Search(query, searchAttributes, keyword);

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                query = QueryHelper<MemoDetailGarmentPurchasingModel>.Filter(query, filterDictionary);

                var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                query = QueryHelper<MemoDetailGarmentPurchasingModel>.Order(query, orderDictionary);

                var pageable = new Pageable<MemoDetailGarmentPurchasingModel>(query, page - 1, size);

                var data = pageable.Data.Select(entity => new ListMemoDetail
                {
                    Id = entity.Id,
                    MemoId = entity.MemoId,
                    MemoNo = entity.MemoNo,
                    MemoDate = entity.MemoDate,
                    AccountingBookId = entity.AccountingBookId,
                    AccountingBookType = entity.AccountingBookType,
                    GarmentCurrenciesId = entity.GarmentCurrenciesId,
                    GarmentCurrenciesCode = entity.GarmentCurrenciesCode,
                    GarmentCurrenciesRate = entity.GarmentCurrenciesRate,
                    Remarks = entity.Remarks,
                    IsPosted = entity.IsPosted,
                }).ToList();
                //var data = pageable.Data.ToList();

        int totalData = pageable.TotalCount;
                return new ReadResponse<ListMemoDetail>(data, totalData, orderDictionary, new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReadResponse<ReportRincian> GetReport(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas)
        {
            try
            {
                var memoDetailsGarments = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();
                var memoDetailsGarmentsDetails = _dbContext.MemoDetailGarmentPurchasingDetails.AsQueryable();
                var garmentDebts = _dbContext.GarmentDebtBalances.AsQueryable();

                var searchAttributes = new List<string>
                {
                    "AccountingBookType"
                };

                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Search(memoDetailsGarments, searchAttributes, keyword);

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Filter(memoDetailsGarments, filterDictionary);

                var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Order(memoDetailsGarments, orderDictionary);

                var pageable = new Pageable<MemoDetailGarmentPurchasingModel>(memoDetailsGarments, page - 1, size);

                var memoDetails = pageable.Data.Where(s => s.MemoDate.Date.Year == date.Date.Year & s.MemoDate.Date.Month == date.Date.Month);

                var memoDetailDetails = from memoDetail in memoDetails
                                        join memoDetailsGarmentsDetail in memoDetailsGarmentsDetails
                           on memoDetail.Id equals memoDetailsGarmentsDetail.MemoDetailId
                           select new {
                               memoDetail.Id,
                               memoDetail.MemoId,
                               memoDetail.MemoNo,
                               memoDetail.MemoDate,
                               memoDetail.AccountingBookType,
                               memoDetailsGarmentsDetail.MemoDetailId,
                               memoDetailsGarmentsDetail.GarmentDeliveryOrderNo,
                               memoDetailsGarmentsDetail.GarmentDeliveryOrderId,
                               memoDetailsGarmentsDetail.RemarksDetail,
                               memoDetailsGarmentsDetail.PaymentRate,
                               memoDetailsGarmentsDetail.PurchasingRate,
                               memoDetailsGarmentsDetail.MemoAmount,
                               memoDetailsGarmentsDetail.MemoIdrAmount,
                               memoDetailsGarmentsDetail.SupplierCode,
                               memoDetailsGarmentsDetail.SupplierName
                           };

                var reports = from memoDetailDetail in memoDetailDetails
                              join garmentDebt in garmentDebts
                              on memoDetailDetail.GarmentDeliveryOrderId equals garmentDebt.GarmentDeliveryOrderId
                              select new ReportRincian
                              {
                                  Id = memoDetailDetail.Id,
                                  MemoId = memoDetailDetail.MemoId,
                                  MemoNo = memoDetailDetail.MemoNo,
                                  MemoDate = memoDetailDetail.MemoDate,
                                  InternalNoteNo = garmentDebt.InternalNoteNo,
                                  BillsNo = garmentDebt.BillsNo,
                                  PaymentBills = garmentDebt.PaymentBills,
                                  GarmentDeliveryOrderNo = memoDetailDetail.GarmentDeliveryOrderNo,
                                  RemarksDetail = memoDetailDetail.RemarksDetail,
                                  CurrencyCode = garmentDebt.CurrencyCode,
                                  MemoAmount = memoDetailDetail.MemoAmount,
                                  MemoIdrAmount = memoDetailDetail.MemoIdrAmount,
                                  AccountingBookType = memoDetailDetail.AccountingBookType,
                                  PaymentRate = memoDetailDetail.PaymentRate,
                                  PurchasingRate = memoDetailDetail.PurchasingRate,
                                  SupplierCode = memoDetailDetail.SupplierCode,
                                  SupplierName = memoDetailDetail.SupplierName
                              };

                if(valas > -1)
                {
                    if(valas == 0)
                    {
                        reports = reports.Where(s => s.CurrencyCode == "IDR");
                    }
                    else
                    {
                        reports = reports.Where(s => s.CurrencyCode != "IDR");
                    }
                }

                int totalData = pageable.TotalCount;
                return new ReadResponse<ReportRincian>(reports.ToList(), totalData, orderDictionary, new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DetailRincian GetDetailById(int Id)
        {
            var memoGarments = _dbContext.MemoGarmentPurchasings.AsQueryable();
            var memoDetailsGarments = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();
            var memoDetailsGarmentsDetails = _dbContext.MemoDetailGarmentPurchasingDetails.AsQueryable();
            var garmentDebts = _dbContext.GarmentDebtBalances.AsQueryable();

            var memoDetail = memoDetailsGarments.FirstOrDefault(s => s.Id == Id);
            var listDataDetails = memoDetailsGarmentsDetails.Where(m => m.MemoDetailId == Id).Select(m => new
            {
                m.Id,
                m.MemoDetailId,
                m.GarmentDeliveryOrderNo,
                m.GarmentDeliveryOrderId,
                m.RemarksDetail,
                m.PaymentRate,
                m.PurchasingRate,
                m.MemoAmount,
                m.MemoIdrAmount
            });

            var listData = from listDataDetail in listDataDetails
                           join garmentDebt in garmentDebts on listDataDetail.GarmentDeliveryOrderId equals garmentDebt.GarmentDeliveryOrderId
                           select new DetailRincianItems {
                               Id = listDataDetail.Id,
                               GarmentDeliveryOrderId = listDataDetail.GarmentDeliveryOrderId,
                               GarmentDeliveryOrderNo =  listDataDetail.GarmentDeliveryOrderNo,
                               InternalNoteNo = garmentDebt.InternalNoteNo,
                               BillsNo = garmentDebt.BillsNo,
                               PaymentBills = garmentDebt.PaymentBills,
                               SupplierCode = garmentDebt.SupplierCode,
                               RemarksDetail = listDataDetail.RemarksDetail,
                               CurrencyCode = garmentDebt.CurrencyCode,
                               PaymentRate = listDataDetail.PaymentRate,
                               PurchasingRate = listDataDetail.PurchasingRate,
                               SaldoAkhir = garmentDebt.DPPAmount + garmentDebt.VATAmount - garmentDebt.IncomeTaxAmount,
                               MemoAmount = listDataDetail.MemoAmount,
                               MemoIdrAmount = listDataDetail.MemoIdrAmount,
                           };

            var queryJoin = memoDetailsGarments.Where(m => m.Id == Id).Select(memoDetailsGarment => new DetailRincian
            {
                Id = memoDetailsGarment.Id,
                MemoId = memoDetailsGarment.MemoId,
                MemoDate = memoDetailsGarment.MemoDate,
                MemoNo = memoDetailsGarment.MemoNo,
                AccountingBookId = memoDetailsGarment.AccountingBookId,
                AccountingBookType = memoDetailsGarment.AccountingBookType,
                GarmentCurrenciesId = memoDetailsGarment.GarmentCurrenciesId,
                GarmentCurrenciesCode = memoDetailsGarment.GarmentCurrenciesCode,
                GarmentCurrenciesRate = memoDetailsGarment.GarmentCurrenciesRate,
                Remarks = memoDetailsGarment.Remarks,
                Items = listData.ToList()
            });

            return queryJoin.FirstOrDefault(s => s.MemoId == memoDetail.MemoId);
        }

        public async Task<int> UpdateAsync(int id, EditDetailRincian viewModel)
        {
            UpdateModel(id, viewModel);
            return await _dbContext.SaveChangesAsync();
        }

        public void UpdateModel(int id, EditDetailRincian viewModel)
        {
            MemoDetailGarmentPurchasingModel existDb = DbSet
                        .Include(d => d.MemoDetailGarmentPurchasingDetail)
                        .Single(memoDetail => memoDetail.Id == id && !memoDetail.IsDeleted);


            existDb.MemoId = viewModel.MemoId;
            existDb.Remarks = viewModel.Remarks;
            existDb.MemoNo = viewModel.MemoNo;
            existDb.MemoDate = viewModel.MemoDate;
            existDb.AccountingBookId = viewModel.AccountingBookId;
            existDb.AccountingBookType = viewModel.AccountingBookType;
            existDb.GarmentCurrenciesId = viewModel.GarmentCurrenciesId;
            existDb.GarmentCurrenciesCode = viewModel.GarmentCurrenciesCode;
            existDb.GarmentCurrenciesRate = viewModel.GarmentCurrenciesRate;
            existDb.IsPosted = viewModel.IsPosted;

            EntityExtension.FlagForUpdate(existDb, _identityService.Username, UserAgent);
            DbSet.Update(existDb);

            foreach (var existRow in existDb.MemoDetailGarmentPurchasingDetail)
            {
                EditDetailRincianItems itemModel = viewModel.Items.FirstOrDefault(p => p.Id.Equals(existRow.Id));
                if (itemModel == null)
                {
                    EntityExtension.FlagForDelete(existRow, _identityService.Username, UserAgent);
                }
                else
                {
                    existRow.GarmentDeliveryOrderId = itemModel.GarmentDeliveryOrderId;
                    existRow.GarmentDeliveryOrderNo = itemModel.GarmentDeliveryOrderNo;
                    existRow.RemarksDetail = itemModel.RemarksDetail;
                    existRow.PaymentRate = itemModel.PaymentRate;
                    existRow.PurchasingRate = itemModel.PurchasingRate;
                    existRow.MemoAmount = itemModel.MemoAmount;
                    existRow.MemoIdrAmount = itemModel.MemoIdrAmount;

                    EntityExtension.FlagForUpdate(existRow, _identityService.Username, UserAgent);
                }

            }

            _dbContext.MemoDetailGarmentPurchasingDetails.UpdateRange(existDb.MemoDetailGarmentPurchasingDetail);

            foreach (var item in viewModel.Items)
            {
                if (item.Id == 0)
                {
                    MemoDetailGarmentPurchasingDetailModel model = new MemoDetailGarmentPurchasingDetailModel();

                    model.GarmentDeliveryOrderId = item.GarmentDeliveryOrderId;
                    model.GarmentDeliveryOrderNo = item.GarmentDeliveryOrderNo;
                    model.RemarksDetail = item.RemarksDetail;
                    model.PaymentRate = item.PaymentRate;
                    model.PurchasingRate = item.PurchasingRate;
                    model.MemoAmount = item.MemoAmount;
                    model.MemoIdrAmount = item.MemoIdrAmount;
                    model.MemoDetailId = id;

                    EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                    DbSetDetail.Add(model);
                }
            }
        }

        public async Task<MemoDetailGarmentPurchasingModel> ReadByIdAsync(int id)
        {
            return await DbSet.Include(m => m.MemoDetailGarmentPurchasingDetail)
                .FirstOrDefaultAsync(d => d.Id.Equals(id) && !d.IsDeleted);
        }

        public async Task DeleteModel(int id)
        {
            MemoDetailGarmentPurchasingModel model = await ReadByIdAsync(id);
            foreach (var item in model.MemoDetailGarmentPurchasingDetail)
            {
                EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);
            }
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent, true);
            DbSet.Update(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            await DeleteModel(id);
            return await _dbContext.SaveChangesAsync();
        }

        public ReadResponse<ReportPDF> GetPDF(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas)
        {
            try
            {
                var memoDetailsGarments = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();
                var memoDetailsGarmentsDetails = _dbContext.MemoDetailGarmentPurchasingDetails.AsQueryable();
                var garmentDebts = _dbContext.GarmentDebtBalances.AsQueryable();

                var searchAttributes = new List<string>
                {
                    "AccountingBookType"
                };

                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Search(memoDetailsGarments, searchAttributes, keyword);

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Filter(memoDetailsGarments, filterDictionary);

                var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Order(memoDetailsGarments, orderDictionary);

                var pageable = new Pageable<MemoDetailGarmentPurchasingModel>(memoDetailsGarments, page - 1, size);

                var memoDetails = pageable.Data.Where(s => s.MemoDate.Date.Year == date.Date.Year & s.MemoDate.Date.Month == date.Date.Month);

                var memoDetailDetails = from memoDetail in memoDetails
                                        join memoDetailsGarmentsDetail in memoDetailsGarmentsDetails
                           on memoDetail.Id equals memoDetailsGarmentsDetail.MemoDetailId
                                        select new
                                        {
                                            memoDetail.Id,
                                            memoDetail.MemoId,
                                            memoDetail.MemoNo,
                                            memoDetail.MemoDate,
                                            memoDetail.AccountingBookType,
                                            memoDetailsGarmentsDetail.MemoDetailId,
                                            memoDetailsGarmentsDetail.GarmentDeliveryOrderNo,
                                            memoDetailsGarmentsDetail.GarmentDeliveryOrderId,
                                            memoDetailsGarmentsDetail.RemarksDetail,
                                            memoDetailsGarmentsDetail.PaymentRate,
                                            memoDetailsGarmentsDetail.PurchasingRate,
                                            memoDetailsGarmentsDetail.MemoAmount,
                                            memoDetailsGarmentsDetail.MemoIdrAmount,
                                            memoDetailsGarmentsDetail.SupplierCode,
                                            memoDetailsGarmentsDetail.SupplierName,
                                        };

                var reports = from memoDetailDetail in memoDetailDetails
                              join garmentDebt in garmentDebts
                              on memoDetailDetail.GarmentDeliveryOrderId equals garmentDebt.GarmentDeliveryOrderId
                              select new ReportPDF
                              {
                                  Id = memoDetailDetail.Id,
                                  MemoId = memoDetailDetail.MemoId,
                                  MemoNo = memoDetailDetail.MemoNo,
                                  MemoDate = memoDetailDetail.MemoDate,
                                  InternalNoteNo = garmentDebt.InternalNoteNo,
                                  BillsNo = garmentDebt.BillsNo,
                                  PaymentBills = garmentDebt.PaymentBills,
                                  GarmentDeliveryOrderNo = memoDetailDetail.GarmentDeliveryOrderNo,
                                  RemarksDetail = memoDetailDetail.RemarksDetail,
                                  CurrencyCode = garmentDebt.CurrencyCode,
                                  MemoAmount = memoDetailDetail.MemoAmount,
                                  MemoIdrAmount = memoDetailDetail.MemoIdrAmount,
                                  AccountingBookType = memoDetailDetail.AccountingBookType,
                                  PurchasingRate = memoDetailDetail.PurchasingRate,
                                  SupplierCode = memoDetailDetail.SupplierCode,
                                  SupplierName = memoDetailDetail.SupplierName
                              };

                if (valas > -1)
                {
                    if (valas == 0)
                    {
                        reports = reports.Where(s => s.CurrencyCode == "IDR");
                    }
                    else
                    {
                        reports = reports.Where(s => s.CurrencyCode != "IDR");
                    }
                }

                int totalData = pageable.TotalCount;
                return new ReadResponse<ReportPDF>(reports.ToList(), totalData, orderDictionary, new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
