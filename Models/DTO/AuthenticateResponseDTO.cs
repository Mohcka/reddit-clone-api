namespace reddit_clone_api.Models.DTO
{
  /// <summary>
  /// Defines the shape of the data to return upon successful authentication.
  /// </summary>
  public class AuthenticateResponseDTO
  {
    public string Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }

    public AuthenticateResponseDTO(User user, string token){
      Id = user.Id;
      Username = user.UserName;
      Token = token;
    }
  }
}