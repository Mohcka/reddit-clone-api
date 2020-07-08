using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace reddit_clone_api.Models.DTO {
  // TODO: May not need, remove later
  public class CreatePostDTO {
    [Required]
    public string PostTitle { get; set; }
    [Required]
    public string PostContent { get; set; }
  }
}