using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFollowService
    {
        IResult FollowPlaylist(int playlistId);
        IResult UnfollowPlaylist(int playlistId);

    }
}
