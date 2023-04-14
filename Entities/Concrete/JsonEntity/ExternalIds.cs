using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.JsonEntity
{
    public class ExternalIds:IEntity
    {
        public string isrc { get; set; }
        public string ean { get; set; }
        public string upc { get; set; }
    }
}
