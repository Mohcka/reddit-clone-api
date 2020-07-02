using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace reddit_clone_api.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Title")]
        public string PostTitle { get; set; }
        [BsonElement("Post")]
        public string PostContent { get; set; }

    }
}