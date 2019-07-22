using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MongoFrameworkTest
{
    public static class MongoSequence
    {

        public static async Task<long> GetSequenceNo(string ConnectionString,string DatabaseName)
        {
            MongoClient client = new MongoClient(ConnectionString);
            IMongoDatabase database = client.GetDatabase(DatabaseName);
            IMongoCollection<Sequence> collection = database.GetCollection<Sequence>("MongoSequenceTest");
            var filter = Builders<Sequence>.Filter.Eq(m => m.Id, "Device");
            var projection = Builders<Sequence>.Projection.Exclude(m => m.Id).Include(m => m.Seq);
            var deviceSequence = await collection.Find<Sequence>(filter).Project(projection).FirstOrDefaultAsync();
            if (deviceSequence == null)
            {
                Sequence sequence = new Sequence { Id = "Device", Seq = 0 };
                await collection.InsertOneAsync(sequence);
                Sequence updateResult = await collection.FindOneAndUpdateAsync<Sequence>(
                     Builders<Sequence>.Filter.Eq(m=>m.Id, "Device"),
                     Builders<Sequence>.Update.Set(m=>m.Seq,1)
                     );
            }
           
            var findCommandDocument = BsonDocument.Parse("{findAndModify:\"MongoSequenceTest\",query:{_id:\"Device\"},update:{$inc:{Seq:1}},upsert:true}");
            BsonDocument resultDocument = (await database.RunCommandAsync<BsonDocument>(findCommandDocument)).GetElement("value").Value.ToBsonDocument();
            Sequence result = BsonSerializer.Deserialize<Sequence>(resultDocument.ToJson());
            return result.Seq;

         }
    }

    public class Sequence
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
        [BsonElement("Seq")]
        public long Seq  { get; set; }
    }

}
