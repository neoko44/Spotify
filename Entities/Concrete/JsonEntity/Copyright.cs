﻿using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.JsonEntity
{
    public class Copyright:IEntity
    {
        public string text { get; set; }
        public string type { get; set; }
    }
}
