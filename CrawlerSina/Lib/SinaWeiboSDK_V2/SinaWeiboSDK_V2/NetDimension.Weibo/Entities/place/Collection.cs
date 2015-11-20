using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDimension.Json;
using NetDimension.Json.Linq;

namespace NetDimension.Weibo.Entities.place
{
    public class Collection:EntityBase
    {
        [JsonProperty(PropertyName = "statuses")]
        public List<NetDimension.Weibo.Entities.place.Entity> Statuses { get; internal set; }
       
    }
}
