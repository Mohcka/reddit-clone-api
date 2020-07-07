using System.Collections;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

using MongoDB.Driver;

using reddit_clone_api.Models;
using reddit_clone_api.Models.DTO;

using reddit_clone_api.Helpers;

namespace reddit_clone_api.Services
{
  public interface IUserService
  {
    AuthenticateResponseDTO Authenticate(AuthenticateRequestDTO model);
    IEnumerable<User> GetAll();

    Task<User> GetUser(string Id);
  }

  public class UserService : IUserService
  {
    private readonly IMongoCollection<User> _users;

    private readonly AppSettings _appSettings;

    public UserService(IRedditCloneDatabaseSettings dbSettings, IAppSettings appSettings)
    {
      var client = new MongoClient(dbSettings.ConnectionString);
      var database = client.GetDatabase(dbSettings.DatabaseName);

      _users = database.GetCollection<User>(dbSettings.UsersCollectionName);

      _appSettings = appSettings as AppSettings;

      // TODO: Seed data within
    }

    public async void Register()
    {
      await _users.InsertOneAsync(new User());
    }

    /// <summary>
    /// TODO: write docs
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public AuthenticateResponseDTO Authenticate(AuthenticateRequestDTO model) // TODO: Why did I put this DTO here
    {
      System.Console.WriteLine(_users.Find(x => true).ToList());


      var user = _users.Find(u => u.UserName == model.Username && u.Password == model.Password).SingleOrDefault();


      // no user was found
      if (user == null)
      {
        return null;
      }

      // Authenticate if succesful
      var token = generateJwtToken(user);

      return new AuthenticateResponseDTO(user, token);
    }

    /// <summary>
    /// TODO: Write docs
    /// </summary>
    /// <returns></returns>
    public IEnumerable<User> GetAll()
    {
      return _users.AsQueryable();
    }

    public async Task<User> GetUser(string id)
    {
      return await _users.Find<User>(u => u.Id == id).FirstOrDefaultAsync();
    }

    public IEnumerable<User> GetPosts()
    {
      return _users.AsQueryable().ToList();
    }

    // Helper methods

    /// <summary>
    /// Generates a token upon successful authentication which the client
    /// will then use to validate upon later requests
    /// 
    /// Taken from:
    /// https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api#user-service-cs
    /// </summary>
    /// <param name="user">User's information used to generate token</param>
    /// <returns>JWToken</returns>
    private string generateJwtToken(User user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        // set claim
        Subject = new ClaimsIdentity(new Claim[]{
          new Claim(ClaimTypes.Name, user.Id) // TODO: test ToString()
        }),
        // set expiration
        Expires = DateTime.UtcNow.AddMinutes(60),
        // apply credentials from secret
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}