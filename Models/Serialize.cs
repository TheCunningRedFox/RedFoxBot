using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCunningFoxBot.Models
{
    public static class Serialize
    {
        public static string ToJson(this Quote self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
