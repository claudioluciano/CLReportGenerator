using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CL.ReportGenerator
{
    public class DownloadFileAsAttachmentResult : FileContentResult
    {
        public DownloadFileAsAttachmentResult(string fileName, byte[] fileContents, string contentType) : base(fileContents, contentType)
        {
            this.FileDownloadName = fileName;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={FileDownloadName}");

            base.ExecuteResult(context);
        }
    }
}
