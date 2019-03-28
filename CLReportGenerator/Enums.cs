

namespace CL.ReportGenerator
{
    public enum CommandType
    {
        //
        // Summary:
        //     An SQL text command. (Default.)
        Query = 1,
        //
        // Summary:
        //     The name of a stored procedure.
        StoredProcedure = 4,
    }

    public enum ReportOrientation
    {
        Landscape = 0,
        Portrait = 1
    }
}
