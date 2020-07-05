using System.ComponentModel.DataAnnotations;

namespace reddit_clone_api.Models.DTO
{
  /// <summary>
  /// Defines the expected parameters for the request when authenticating a user.
  /// </summary>
  public class AuthenticateRequestDTO
  {
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
  }
}