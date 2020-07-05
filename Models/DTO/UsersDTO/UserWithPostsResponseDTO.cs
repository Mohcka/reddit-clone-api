using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace reddit_clone_api.Models.DTO {
  /// <summary>
  /// Model used to respond to client with Username and the User's Posts
  /// </summary>
  public class UserWithPostsResponseDTO {
    [Required]
    public string Username { get; set; }
    [Required]
    public IEnumerable<Post> Posts { get; set; } 
  }
}