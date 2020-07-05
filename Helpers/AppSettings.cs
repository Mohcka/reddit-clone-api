namespace reddit_clone_api.Helpers
{
  /// <summary>
  /// TODO: Write Docs
  /// Excerpt from: https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api#app-settings-cs
  /// "Contains properties defined in the appsettings.json file and is 
  /// used for accessing application settings via objects that are injected 
  /// into classes using the ASP.NET Core built in dependency injection (DI) 
  /// system."
  /// </summary>
  public class AppSettings : IAppSettings
  {
    public string Secret { get; set; }
  }
  
  public interface IAppSettings
  {
    string Secret { get; set; }
  }
}