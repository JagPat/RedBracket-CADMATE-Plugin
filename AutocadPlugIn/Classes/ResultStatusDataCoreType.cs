using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAutocadPlugIn
{
    public class ResultStatusDataCoreType
    {

          public int id { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public bool iscustom { get; set; }

        public int companyid { get; set; }

        public string master { get; set; }

        public string colorId { get; set; }

        public int createdBy { get; set; }

        public string createdOn { get; set; }

        public int updatedBy { get; set; }

        public bool active { get; set; }

        public bool deleted { get; set; }


    }
}
