using Core.Utilities.Results;
using Entities.Concrete.JsonEntity;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITrackPoolService
    {
        Task<IDataResult<List<TrackDto>>> GetTrackPoolAsync(); // Get Track Pool
        Task<IDataResult<Root>> GetAlbumAsync(string albumId); // Get Album


    }
}
