using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class UserFavorite:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FavoriteId { get; set; }
        public string TrackId { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }

    }
}
