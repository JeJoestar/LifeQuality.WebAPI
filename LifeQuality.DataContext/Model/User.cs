using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public class User: EntityWithUpdateCreateFields
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
