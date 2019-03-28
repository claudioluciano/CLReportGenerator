using System.Threading.Tasks;
using CL.ReportGenerator.Demo.Model;
using Microsoft.AspNetCore.Mvc;

namespace CL.ReportGenerator.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IReportGenerator _reportGenerator;

        public ValuesController(IReportGenerator reportGenerator)
        {
            _reportGenerator = reportGenerator;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = "SELECT * FROM Pessoa";

            return await _reportGenerator.GenerateReportAsEXCEL<Pessoa>("Relatorio", query);
        }
    }
}
