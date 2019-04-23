using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CL.ReportGenerator
{
    public interface IConnection
    {
        IDbConnection Connection { get; }

        string ConnectionString { get; }
    }
}
