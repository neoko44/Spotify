using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        IUserLibraryService _libraryService;

        public LibrariesController(IUserLibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("getlibrary")]
        public IActionResult GetUserLibrary()
        {
            var result = _libraryService.GetUserLibrary();
            return Ok(result);
        }
    }
}
