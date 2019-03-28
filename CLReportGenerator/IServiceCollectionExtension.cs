using CL.ReportGenerator.RazorEngine;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace CL.ReportGenerator
{
    public static class IServiceCollectionExtension
    {
        public static void AddReportGenerator(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddTransient<IConnection>(fact => new Connection(connectionString));

            serviceCollection.AddTransient<IReportGenerator, ReportGenerator>();

            serviceCollection.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            string assemblyPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(IServiceCollectionExtension)).Location);

            // carrega o gerador de html para pdf
            var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
            var wkHtmlToPdfPath = Path.Combine(assemblyPath, "..\\..\\content", $"wkhtmltox\\v0.12.4\\{architectureFolder}\\libwkhtmltox");
            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(wkHtmlToPdfPath);

            serviceCollection.AddScoped<IConverter>(fact => new SynchronizedConverter(new PdfTools()));
        }
    }
}
