using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public class CholesterolBloodAnalysisData : BloodAnalysisData
    {
        public double CholesterolLevel { get; set; }
        public double Triglyceride { get; set; }
        public double NormalLevelRequirement { get; set; }
        public override void CreateData()
        {
            var arrayToData = new { CholesterolLevel, Triglyceride, NormalLevelRequirement };
            Data = JsonConvert.SerializeObject(arrayToData);
        }
    }
}
