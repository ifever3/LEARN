using MongoDB.Driver;
using MongoDB.Bson;

namespace LEARN.WF;

public class mongodb
{
    public void mgconnadd()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("test");

        var collection = database.GetCollection<BsonDocument>("wc");
        var document = new BsonDocument { { "name", "tom" } };
        collection.InsertOne(document);

        var documents = collection.Find(new BsonDocument()).ToList();
        foreach (var d in documents)
        {
            Console.WriteLine(d);
        }
        
        var filter = Builders<BsonDocument>.Filter.Eq("name", "tom");

        // 查询符合条件的文档
        var ds = collection.Find(filter).ToList();
        foreach (var dt in ds)
        {
            Console.WriteLine(dt);
        }

        var filter1 = Builders<BsonDocument>.Filter.Eq("name", "tom");

        // 删除文档
        var result = collection.DeleteMany(filter1);
        Console.WriteLine("Deleted count: {0}", result.DeletedCount);

    }
}
