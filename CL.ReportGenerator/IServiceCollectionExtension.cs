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
        /// <summary>
        /// Load the report generator
        /// </summary>
        /// <param name="serviceCollection">Service Colletio</param>
        /// <param name="connectionString">Connection string for DB</param>
        /// <param name="loadUnmanagedLibrary">In case you dont want to load the wkhtmltox library from this package, ex: this package will run in a docker container with linux</param>
        public static void AddReportGenerator(this IServiceCollection serviceCollection, string connectionString, bool loadUnmanagedLibrary = true)
        {
            serviceCollection.AddTransient<IConnection>(fact => new Connection(connectionString));

            serviceCollection.AddTransient<IReportGenerator, ReportGenerator>();

            serviceCollection.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            
            if(loadUnmanagedLibrary)
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                // carrega o gerador de html para pdf
                var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
                var wkHtmlToPdfPath = Path.Combine(basePath, "wkhtmltox\\libwkhtmltox");
                CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
                context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            }

            // Add converter to DI
            serviceCollection.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }
    }
}
