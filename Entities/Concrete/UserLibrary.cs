using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class UserLibrary:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LibraryId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int TrackCount { get; set; }
        public bool Status { get; set; }
        public int PlaylistId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }

    }
}
