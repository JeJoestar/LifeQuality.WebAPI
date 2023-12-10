using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public class GeneralBloodAnalysisData : BloodAnalysisData
    {
        public string WBC { get; set; }
        public string HGB { get; set; }
        public string RBC { get; set; }
        public string HCT { get; set; }
        public string MCV { get; set; }
        public override void CreateData()
        {
            var arrayToData = new { WBC, HGB, RBC, HCT, MCV };
            Data = JsonConvert.SerializeObject(arrayToData);
        }
    }
}
