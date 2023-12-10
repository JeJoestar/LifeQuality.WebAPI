using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public class SugarBloodAnalysisData : BloodAnalysisData
    {
        public double BloodSugarLevel { get; set; }
        public double BloodGlucose { get; set; }
        public double HbA1c { get; set; }
        public override void CreateData()
        {
            var arrayToData = new { BloodSugarLevel, BloodGlucose, HbA1c };
            Data = JsonConvert.SerializeObject(arrayToData);
        }
    }
}
