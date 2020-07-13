using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace reddit_clone_api.Models.DTO {
  public class PostWithCommentsResponseDTO {
    [Required]
    public Post Post { get; set; }
    [Required]
    public IEnumerable<Comment> Comments { get; set; }
  }
}