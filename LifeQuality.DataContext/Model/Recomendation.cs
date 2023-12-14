using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public class Recomendation: EntityWithUpdateCreateFields
    {
        public int RecieverId { get; set; }
        public Patient Reciever { get; set; }
        public int AnalysisId { get; set; }
        public BloodAnalysisData Analysis { get; set; }
        public string Content { get; set;}
    }
}
