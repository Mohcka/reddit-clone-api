using System.Collections.Generic;

using reddit_clone_api.Models;

namespace reddit_clone_api.Models.DTO {
  public class PostWithCommentsDTO {
    public Post Post { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
  }
}