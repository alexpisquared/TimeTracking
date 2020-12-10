using AAV.Sys.AsLink;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;

namespace Db.TimeTrack.DbModel
{

  public partial class A0DbContext : DbContext
  {
    public static A0DbContext Create() => new A0DbContext();

    A0DbContext() : base(SqlConStrHelper.ConStr("TimeTrackDb", _dbgRls, dbLocation, "haha", "haha")) { }

    const string _dbgRls =
#if DEBUG
          "Dbg";
#else
          "Rls";
#endif

    const SqlConStrHelper.DbLocation dbLocation =
#if AZURE_IS_AFFORDABLE                             
        /// May 23, 2019:
        /// apparently, TimeTrackDb..._GP (gen.purp. DB) takes $2/day !!! 
        /// ...but if it is once a month, then it is better than $.10/day.
        /// Final decision pending on either auto stop after 6 hr works its miracle.
      SqlConStrHelper.DbLocation.Azure;   // need to make invoices from Ofc!!!
#elif ONEDRIVE_LOCALDB                              
      SqlConStrHelper.DbLocation.Local;   // need to make invoices from Ofc!!!
#else // SQL_Db_Instance    
      SqlConStrHelper.DbLocation.DbIns;   // keep as a fallback for dev-t (codefirst/datafirst model gen, etc.)
#endif
  }
}