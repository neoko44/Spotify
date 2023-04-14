using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class LibraryManager : ILibraryService
    {
        private ILibraryDal _libraryDal;

        public LibraryManager(ILibraryDal libraryDal)
        {
            _libraryDal = libraryDal;
        }

        public IResult Add(Library library)
        {
            _libraryDal.Add(library);

            return new SuccessResult();
        }
    }
}
