using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class LibraryDto:IDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int TrackCount { get; set; }
    }
}
