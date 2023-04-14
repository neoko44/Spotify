using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UpdatePlaylistNameDto:IDto
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; }
    }
}
