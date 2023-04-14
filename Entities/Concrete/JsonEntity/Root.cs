using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.JsonEntity
{
    public class Root:IEntity
    {
        public bool collaborative { get; set; }
        public string description { get; set; }
        public ExternalUrls external_urls { get; set; }
        public Followers followers { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public Owner owner { get; set; }
        public bool @public { get; set; }
        public string snapshot_id { get; set; }
        public Tracks tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public string album_type { get; set; }
        public int total_tracks { get; set; }
        public List<string> available_markets { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public List<Copyright> copyrights { get; set; }
        public ExternalIds external_ids { get; set; }
        public List<object> genres { get; set; }
        public string label { get; set; }
        public int popularity { get; set; }
        public List<Artist> artists { get; set; }
        public string album_group { get; set; }
    }
}
