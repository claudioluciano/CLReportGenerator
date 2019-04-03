using CL.ReportGenerator.RazorEngine;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace CL.ReportGenerator
{
    public static class IServiceCollectionExtension
    {
        public static void AddReportGenerator(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddTransient<IConnection>(fact => new Connection(connectionString));

            serviceCollection.AddTransient<IReportGenerator, ReportGenerator>();

            serviceCollection.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            // carrega o gerador de html para pdf
            var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
            var wkHtmlToPdfPath = Path.Combine(basePath, $"wkhtmltox\\v0.12.4\\{architectureFolder}\\libwkhtmltox");
            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(wkHtmlToPdfPath);

            // Add converter to DI
            serviceCollection.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }
    }
}
