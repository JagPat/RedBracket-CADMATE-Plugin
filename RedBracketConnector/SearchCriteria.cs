using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBracketConnector
{
    public class SearchCriteria
    {
        public string fileNo { set; get; }

        public string name { set; get; }

        public SearchCriteriaType type { set; get; }

        public StatusCriteria status { set; get; }

        public SearchCriteriaFolder folder { set; get; }
    }

    public class SearchCriteriaType
    {
        public string name { set; get; }
    }

    public class StatusCriteria
    {
        public string statusname { set; get; }
    }

    public class SearchCriteriaFolder
    {
        public string name { set; get; }
    }

}
