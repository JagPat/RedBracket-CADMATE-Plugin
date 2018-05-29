using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBracketConnector
{
    public class SearchCriteria
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string fileNo { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchCriteriaType type { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public StatusCriteria status { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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
