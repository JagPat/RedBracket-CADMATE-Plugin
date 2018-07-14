using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadPlugIn
{
    public class ResultStatusData
    {

        public int id { get; set; }

        public string statusname { get; set; }

        public string type { get; set; }

        public bool iscustom { get; set; }

        public int companyid { get; set; }

        public ResultStatusDataCoreType coretype { get; set; }

        public int priority { get; set; }

        public int createdBy { get; set; }

        public string createdOn { get; set; }

        public int updatedBy { get; set; }

        public string updatedOn { get; set; }

        public bool active { get; set; }

        public bool deleted { get; set; }

        public string name { get; set; }

        public bool IsClosed { get; set; }

    }
}
