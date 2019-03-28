using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CL.ReportGenerator
{
    public interface IReportGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fileName">Just the name without ext</param>
        /// <param name="reportPath">Relative path for report cshtml </param>
        /// <param name="query">Query or procedure name</param>
        /// <param name="param">parameters for query</param>
        /// <param name="type">query or procedure</param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        Task<IActionResult> GenerateReportAsPDF<TModel>(string fileName, string reportPath, string query, object param = null, CommandType type = CommandType.Query, ReportOrientation reportOrientation = ReportOrientation.Portrait );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fileName">Just the name without ext</param>
        /// <param name="query">Query or procedure name</param>
        /// <param name="param">parameters for query</param>
        /// <param name="type">query or procedure</param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        Task<IActionResult> GenerateReportAsEXCEL<TModel>(string fileName, string query, object param = null, CommandType type = CommandType.Query, ReportOrientation reportOrientation = ReportOrientation.Portrait);
    }
}