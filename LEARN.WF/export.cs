using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class DataExporter
{
    // 模拟数据，实际情况下从数据源获取
    private List<DataRecord> dataRecords = new List<DataRecord>()
    {
        new DataRecord { 序号 = 1, 会员号 = "VIP0001", 上傳時間 = DateTime.Parse("2024/5/29 16:00:00"), SellerPermit = true, 存放地址 = "\\172.16.11.251\0001", 是否有效 = false, Notes = "填寫有問題", 操作時間 = DateTime.Parse("2024/5/29 16:10:00"), 操作人 = "OWEN.X" },
        new DataRecord { 序号 = 2, 会员号 = "VIP0001", 上傳時間 = DateTime.Parse("2024/5/29 17:50:00"), SellerPermit = false, 存放地址 = "\\172.16.11.251\0001", 是否有效 = false, Notes = "2024/6/15客人資料變更,轉為失效", 操作時間 = DateTime.Parse("2024/5/29 18:03:00"), 操作人 = "LETITIA.D" },
        new DataRecord { 序号 = 3, 会员号 = "VIP0001", 上傳時間 = DateTime.Parse("2024/6/30 16:30:00"), SellerPermit = true, 存放地址 = "\\172.16.11.251\0001", 是否有效 = true, Notes = "", 操作時間 = null, 操作人 = null },
        new DataRecord { 序号 = 4, 会员号 = "VIP0017", 上傳時間 = DateTime.Parse("2024/5/29 17:50:00"), SellerPermit = true, 存放地址 = "\\172.16.11.251\0007", 是否有效 = true, Notes = "", 操作時間 = null, 操作人 = null },
        new DataRecord { 序号 = 5, 会员号 = "VIP0100", 上傳時間 = DateTime.Parse("2024/6/30 16:30:00"), SellerPermit = true, 存放地址 = "\\172.16.11.251\0100", 是否有效 = true, Notes = "", 操作時間 = null, 操作人 = null },
        new DataRecord { 序号 = 6, 会员号 = "VIP0150", 上傳時間 = DateTime.Parse("2024/5/29 20:05:00"), SellerPermit = true, 存放地址 = "\\172.16.11.251\0150", 是否有效 = true, Notes = "", 操作時間 = null, 操作人 = null },
    };

    public void ExportToExcel(string fileName)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 设置为非商业使用
        ExcelPackage pck = new ExcelPackage();
        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

        // 设置标题行
        ws.Cells["A1"].Value = "序号";
        ws.Cells["B1"].Value = "会员号(唯一標識)";
        ws.Cells["C1"].Value = "上传时间";
        ws.Cells["D1"].Value = "Seller Permit (图片附件)";
        ws.Cells["E1"].Value = "存放地址";
        ws.Cells["F1"].Value = "是否有效/通过";
        ws.Cells["G1"].Value = "Notes";
        ws.Cells["H1"].Value = "操作时间";
        ws.Cells["I1"].Value = "操作人";

        // 填充数据
        for (int i = 0; i < dataRecords.Count; i++)
        {
            ws.Cells[i + 2, 1].Value = dataRecords[i].序号;
            ws.Cells[i + 2, 2].Value = dataRecords[i].会员号;
            ws.Cells[i + 2, 3].Value = dataRecords[i].上傳時間;
            ws.Cells[i + 2, 4].Value = dataRecords[i].SellerPermit ? "✓" : "";
            ws.Cells[i + 2, 5].Value = dataRecords[i].存放地址;
            ws.Cells[i + 2, 6].Value = dataRecords[i].是否有效 ? "是" : "否";
            ws.Cells[i + 2, 7].Value = dataRecords[i].Notes;
            ws.Cells[i + 2, 8].Value = dataRecords[i].操作時間;
            ws.Cells[i + 2, 9].Value = dataRecords[i].操作人;
        }

        // 保存Excel文件
        pck.SaveAs(fileName);
    }

    public void ExportToPdf(string fileName)
    {
        // 创建PDF文档
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));

        // 打开文档
        doc.Open();

        // 创建表格
        PdfPTable table = new PdfPTable(9);
        table.WidthPercentage = 110;

        // 创建字体
        BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        Font font = new Font(baseFont, 12, Font.NORMAL);

        // 设置表头
        PdfPCell cell = new PdfPCell(new Phrase("序号", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("会员号", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("上传时间", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("Seller Permit (图片附件)", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("存放地址", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("是否有效/通过", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("Notes", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("操作时间", font));
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("操作人", font));
        table.AddCell(cell);

        // 填充数据
        foreach (var record in dataRecords)
        {
            cell = new PdfPCell(new Phrase(record.序号.ToString()));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.会员号));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.上傳時間.ToString()));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.SellerPermit ? "✓" : ""));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.存放地址));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.是否有效 ? "是" : "否"));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.Notes));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.操作時間?.ToString()));
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(record.操作人));
            table.AddCell(cell);
        }

        // 添加表格到文档
        doc.Add(table);

        // 关闭文档
        doc.Close();
    }

    // 数据记录类
    public class DataRecord
    {
        public int 序号 { get; set; }
        public string 会员号 { get; set; }
        public DateTime 上傳時間 { get; set; }
        public bool SellerPermit { get; set; }
        public string 存放地址 { get; set; }
        public bool 是否有效 { get; set; }
        public string Notes { get; set; }
        public DateTime? 操作時間 { get; set; }
        public string 操作人 { get; set; }
    }
}
