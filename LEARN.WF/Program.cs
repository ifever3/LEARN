using OpenCvSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace LEARN.WF
{
    internal class Program
    {
        static void Main()
        {
            //opencv opencv = new opencv();
            //opencv.ocv();
            //elasticsearch elasticsearch = new elasticsearch();
            //elasticsearch.ExecuteAsync();
            //mongodb a = new mongodb();
            // a.mgconnadd();
           
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string excelFileName = Path.Combine(desktopPath, "data.xlsx");
            string pdfFileName = Path.Combine(desktopPath, "data.pdf");
            // 导出为Excel
            DataExporter exporter = new DataExporter();
            exporter.ExportToExcel(excelFileName);
            //Process.Start(excelFileName);

            // 导出为PDF
            exporter.ExportToPdf(pdfFileName);
            //Process.Start(pdfFileName);
        }     
    }
}
