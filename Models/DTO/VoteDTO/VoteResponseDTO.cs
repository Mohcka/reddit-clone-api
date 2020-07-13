using System.ComponentModel.DataAnnotations;

namespace reddit_clone_api.Models.DTO {
  public class VoteResponseDTO {
    /// <summary>
    /// The latest vote being made to the content
    /// </summary>
    /// <value></value>
    [Required]
    public string UserVote { get; set; }
    /// <summary>
    /// The current vote status of the content
    /// </summary>
    /// <value></value>
    [Required]
    public int NumVotes { get; set; }
  }
}