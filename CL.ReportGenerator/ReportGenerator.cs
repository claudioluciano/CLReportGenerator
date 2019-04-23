using CL.ReportGenerator.RazorEngine;
using Dapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CL.ReportGenerator
{
    public class ReportGenerator : IReportGenerator
    {
        private readonly IConnection _connection;
        private readonly IRazorViewToStringRenderer _razorEngine;
        private readonly IConverter _converter;

        public ReportGenerator(IConnection connection, IRazorViewToStringRenderer razorEngine, IConverter converter)
        {
            _connection = connection;
            _razorEngine = razorEngine;
            _converter = converter;
        }

        public async Task<IActionResult> GenerateReportAsPDF<TModel>(string fileName, string reportPath, string query, object param = null, CommandType type = CommandType.Query, ReportOrientation reportOrientation = ReportOrientation.Portrait)
        {
            var data = await RunQuery<TModel>(query, param, type);

            return await GeneratePDF(fileName, reportPath, data, reportOrientation);
        }

        public async Task<IActionResult> GenerateReportAsPDFSingleDoc<TModel>(string fileName, string reportPath, string query, object param = null, CommandType type = CommandType.Query, ReportOrientation reportOrientation = ReportOrientation.Portrait)
        {
            var data = await RunSingleQuery<TModel>(query, param, type);

            return await GeneratePDF(fileName, reportPath, data, reportOrientation);
        }

        private async Task<IEnumerable<TModel>> RunQuery<TModel>(string query, object param = null, CommandType type = CommandType.Query)
        {
            using (IDbConnection conn = _connection.Connection)
            {
                conn.Open();

                DynamicParameters dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(param);

                var data = await conn.QueryAsync<TModel>(query, dynamicParameters, commandType: (System.Data.CommandType)type);

                conn.Close();

                return data;
            }
        }

        private async Task<TModel> RunSingleQuery<TModel>(string query, object param = null, CommandType type = CommandType.Query)
        {
            using (IDbConnection conn = _connection.Connection)
            {
                conn.Open();

                DynamicParameters dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(param);

                var data = await conn.QuerySingleAsync<TModel>(query, dynamicParameters, commandType: (System.Data.CommandType)type);

                conn.Close();

                return data;
            }
        }

        private async Task<DownloadFileAsAttachmentResult> GeneratePDF<TModel>(string fileName, string reportPath, TModel data, ReportOrientation reportOrientation)
        {
            var view = await GetViewAsString(reportPath, data);

            var doc = PdfDocument(view, reportOrientation);

            var fileBytes = _converter.Convert(doc);

            return new DownloadFileAsAttachmentResult(fileName + ".pdf", fileBytes, "application/pdf");

        }

        private async Task<string> GetViewAsString<TModel>(string reportPath, TModel model)
        {
            return await _razorEngine.RenderViewToStringAsync(reportPath, model);
        }

        private HtmlToPdfDocument PdfDocument(string content, ReportOrientation reportOrientation = ReportOrientation.Portrait)
        {
            return new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = (Orientation)(int)reportOrientation
                },
                Objects = {
                        new ObjectSettings()
                        {
                            HtmlContent = content
                        }
                    }
            };
        }

        public async Task<IActionResult> GenerateReportAsEXCEL<TModel>(string fileName, string query, object param = null, CommandType type = CommandType.Query, ReportOrientation reportOrientation = ReportOrientation.Portrait)
        {
            using (IDbConnection conn = _connection.Connection)
            {
                conn.Open();

                DynamicParameters dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(param);

                var tes = (System.Data.CommandType)type;

                var data = await conn.QueryAsync<TModel>(query, dynamicParameters, commandType: tes);

                return new DownloadFileAsAttachmentResult(fileName + ".xls", CreateTable(data), "application/vnd.ms-excel"); ;
            }
        }

        private byte[] CreateTable<T>(IEnumerable<T> data)
        {
            StringBuilder builder = new StringBuilder();

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));



            //builder.Append("<table border='1' style =' table-layout:fixed;'>");
            //builder.Append("<tr><td> start of text</td>");

            //builder.Append("<td class='bg'><strong>1st Shift</strong></td>");

            //builder.Append("<td>rest of text</td></tr>");
            //builder.Append("</table>");

            foreach (PropertyDescriptor prop in props)
            {
                builder.Append(prop.DisplayName); // header
                builder.Append("\t");
            }

            builder.AppendLine();

            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    builder.Append(prop.Converter.ConvertToString(prop.GetValue(item)));
                    builder.Append("\t");
                }
                builder.AppendLine();
            }

            //Convert to Byte Array 
            return Encoding.ASCII.GetBytes(builder.ToString());
        }
    }
}