using System.Threading.Tasks;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc;

namespace CL.ReportGenerator
{
    public interface IReportGenerator
    {
        Task<IActionResult> GenerateReportAsPDF<TModel>(string fileName, string reportPath, string query, object param = null, CommandType type = CommandType.Query, Orientation orientation = Orientation.Portrait);

        Task<IActionResult> GenerateReportAsEXCEL<TModel>(string fileName, string query, object param = null, CommandType type = CommandType.Query, Orientation orientation = Orientation.Portrait);
    }
}