namespace CL.ReportGenerator
{
    internal class ReportPDFFile
    {
        internal string FileName { get; set; }
        internal byte[] FileBytes { get; set; }

        internal ReportPDFFile(string fileName, byte[] fileBytes)
        {
            FileName = fileName;
            FileBytes = fileBytes;
        }
    }
}
