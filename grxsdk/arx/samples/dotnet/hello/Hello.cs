﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//

using Gssoft.Gscad.Runtime;
using Gssoft.Gscad.ApplicationServices;

[assembly: CommandClass(typeof(hello.HelloCmd))]

namespace hello
{
  public class HelloCmd
  {
    [CommandMethod("Hello")]
    static public void DoIt()
    {

      try
      {
        Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("Hello dotnet");
      }
      catch (System.Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(ex.ToString());
      }
    }
  }
}
