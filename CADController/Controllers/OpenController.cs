using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CADController.Commands;
using CADController.Controllers;
using ArasConnector;
using ArasConnector.Exceptions;
namespace CADController.Controllers
{
    public class OpenController : BaseController
    {       
        List<String> attribute = new List<string>();
        #region "public Methods"       
        public override void Execute(Command command)
        {
            
            OpenCommand cmd = (OpenCommand)command;
            try
            {
                cmd.ItemObject.ObjectId = cmd.ItemId;
                cmd.ItemObject.ItemType = cmd.ItemType;
                System.Data.DataTable ItemInfo = cmd.ItemInfo;

                cmd.ObjectDataInfo.Id = true;
                cmd.ObjectDataInfo.LockStatus = true;
                cmd.ObjectDataInfo.Name = true;
                cmd.ObjectDataInfo.Number = true;
                cmd.ObjectDataInfo.Revision = true;
                cmd.ObjectDataInfo.State = true;
                cmd.ObjectDataInfo.Generation = true;
                attribute.Add("native_file");
                attribute.Add("classification");
                cmd.ObjectDataInfo.Project = true;
                cmd.ObjectDataInfo.ProjectId = true;
                cmd.ObjectDataInfo.CreatedById = true;
                cmd.ObjectDataInfo.CreatedOn = true;
                cmd.ObjectDataInfo.ModifiedById = true;
                cmd.ObjectDataInfo.ModifiedOn = true;
                cmd.ObjectDataInfo.Attributes = attribute;
                
                objConnector.LoadObject(ref ItemInfo, cmd.CkeckOutPath, cmd.ObjectDataInfo);
               
                return;
            }
            catch (ConnectionException ex)
            {
                errorString = ex.Message;
            }



        }

        #endregion
    }
}
