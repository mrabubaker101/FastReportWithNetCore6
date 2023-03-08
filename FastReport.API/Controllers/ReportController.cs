using FastReport.Export.PdfSimple;
using FastReport.Utils;
using FastReport.Web;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FastReport.API.Controllers;

[Route("api")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [Produces(contentType: "application/pdf")]
    [Route("test")]
    public IActionResult GetReport()
    {
        var webReport = new WebReport();
        string thisFolder = Config.ApplicationFolder;
        string reportsFolder = Path.Combine(thisFolder, "reports");
        string reportName = "Simple List";
        webReport.Report.Load(Path.Combine(reportsFolder, $"{reportName}.frx"));
        var dataSet = new DataSet();
        dataSet.ReadXml(Path.Combine(reportsFolder, "nwind.xml"));
        webReport.Report.RegisterData(dataSet, "NorthWind");

        webReport.Report.Prepare();
        using (MemoryStream ms = new MemoryStream())
        {
            PDFSimpleExport pdfExport = new PDFSimpleExport();
            pdfExport.Export(webReport.Report, ms);
            ms.Flush();
            return File(ms.ToArray(), "application/pdf", Path.GetFileNameWithoutExtension(reportName) + ".pdf");
        } 
    }
}
