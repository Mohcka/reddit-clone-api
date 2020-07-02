using reddit_clone_api.Models;
using reddit_clone_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace redit_clone_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
      private readonly PostService _bookService;

      public PostsController(PostService bookService) {
        _bookService = bookService;
      }

      [HttpGet]
      public ActionResult<List<Post>> Get() => _bookService.Get();
    }
}