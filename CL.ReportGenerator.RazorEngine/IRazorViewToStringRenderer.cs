using System.Threading.Tasks;

namespace CL.ReportGenerator.RazorEngine
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}