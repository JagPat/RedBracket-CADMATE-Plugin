﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//

using Gssoft.Gscad.Runtime;
using Gssoft.Gscad.ApplicationServices;
using Gssoft.Gscad.Geometry;
using Gssoft.Gscad.DatabaseServices;

[assembly: CommandClass(typeof(addline.addlinecmd))]

namespace addline
{
  public class addlinecmd
  {
    [CommandMethod("addline")]
    static public void DoIt()
    {
      Point3d st = new Point3d(100.0,100.0,0.0);
      Point3d ed = new Point3d(200.0, 200.0, 0.0);
      Line lin = new Line(st,ed);

      Database db = HostApplicationServices.WorkingDatabase;
      Transaction trans = db.TransactionManager.StartTransaction();

      try
      {

        BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
        BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        btr.AppendEntity(lin);
        trans.AddNewlyCreatedDBObject(lin, true);
        trans.Commit();
      }
      catch (System.Exception ex)
      {
        Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.ToString());
        trans.Abort();
      }


    }
  }
}
