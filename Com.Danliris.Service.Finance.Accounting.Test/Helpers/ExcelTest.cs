using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Helpers
{
   public class ExcelTest
    {
        enum Align
        {
            horizontal = 1,
            vertical = 1,
        }
        [Fact]
        public void CreateExcel_Success()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Step 3: here we add 5 rows.
            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);

            var mergeCells = new List<(string cells, Enum hAlign, Enum vAlign)>
                          {
                              ("A1",Align.horizontal,Align.vertical)

                          };
            var dtSourceList = new List<(DataTable table, string sheetName, List<(string cells, Enum hAlign, Enum vAlign)> mergeCells)>()
            {
                (table,"sheetName",mergeCells)
            };

            var result = Excel.CreateExcel(dtSourceList, true);
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateExcelWithTitleNonDateFilterWith_Success()
        {
            string title = "title";
            string date = "date";

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bon Terima Unit", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bukti Pengeluaran Bank", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor NI/SPB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Memo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Invoice", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Koreksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tempo Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice DPP", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice DPP Valas", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice PPN", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice Total", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mutasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(string) });

            dt.Rows.Add("", "", "", "", "", "", "", "", "", "TOTAL", "", "", "IDR", 0.ToString("#,##0.#0"));

            var result = Excel.CreateExcelWithTitleNonDateFilter(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Kartu Hutang") }, title, date, true, 1);
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateExcelWithTitleNonDateFilterWithSupplierName_Success()
        {
            string title = "title";
            string supplierName = "supplierName";
            string date = "date";

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bon Terima Unit", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bukti Pengeluaran Bank", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor NI/SPB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Memo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Invoice", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Koreksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tempo Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice DPP", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice DPP Valas", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice PPN", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice Total", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mutasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(string) });

            dt.Rows.Add("", "", "", "", "", "", "", "", "", "TOTAL", "", "", "IDR", 0.ToString("#,##0.#0"));

            var result = Excel.CreateExcelWithTitleNonDateFilterWithSupplierName(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Kartu Hutang") }, title, supplierName, date, true, 1);
            Assert.NotNull(result);
        }
    }
}
