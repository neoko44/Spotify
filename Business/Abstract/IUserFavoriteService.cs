using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserFavoriteService
    {
        Task<IResult> Add(string trackId);
        IResult Delete(string trackId);
    }
}
