using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.JsonEntity
{
    public class Image:IEntity
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
}
