using MongoDB.Driver;
using MongoFramework;
using MongoFramework.Infrastructure.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MongoFrameworkTest
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object value, JsonSerializerSettings settings = null)
        {
            if (value == null)
            {
                return null;
            }


                return JsonConvert.SerializeObject(value, settings);

            
        }
        public static T FromJson<T>(this string s, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(s, settings);
        }
    }
    public static class JsonSettings
    {
        public static readonly JsonSerializerSettings DataJsonSettings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            DefaultValueHandling = DefaultValueHandling.Include,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            Formatting = Formatting.None,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };
    }
    public class DeviceMongoContext : MongoDbContext
    {
        public DeviceMongoContext(IMongoDbConnection Connection) : base(Connection)
        {
            EntityMapping.GetOrCreateDefinition(typeof(DeviceView)).CollectionName = "Test.Devices";
            EntityMapping.GetOrCreateDefinition(typeof(DeviceReadingModel)).CollectionName = "Test.DeviceReadings";
        }
        public MongoDbSet<DeviceReadingModel> DeviceReadingModels { get; set; }
        public MongoDbSet<DeviceView> DeviceModels { get; set; }
    }
    public class DeviceReadingModel
    {
        /// <summary>
        ///     Id
        /// </summary>
        [Key]
        [Required]
        public string DeviceReadingId { get; set; }

        public string DeviceId { get; set; }

        public string DeviceMac { get; set; }

        [Required]
        public string DeviceType { get; set; }

        public string DeviceSn { get; set; }

        public string DeviceName { get; set; }

        public string Description { get; set; }
        [Required]
        public double DeviceReading { get; set; }

        public double SettlementReading { get; set; }

        public string ServiceID { get; set; }

        public bool Success { get; set; }

        [Required]
        public DateTimeOffset ReadingTime { get; set; }

        [Required]
        public string Supplier { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public DateTimeOffset CreateTime { get; set; }

        [Required]
        public DateTimeOffset UpdateTime { get; set; }

        [Column("Data")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string DataPayload
        {
            get => Data.ToJson(JsonSettings.DataJsonSettings);
            set => Data = new Dictionary<string, string>(value.FromJson<Dictionary<string, string>>(JsonSettings.DataJsonSettings), StringComparer.OrdinalIgnoreCase);
        }

        [JsonIgnore]
        [NotMapped]
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
    public class DeviceView : KolibreActorState
    {
        [Required]
        public string DeviceId { get; set; }
        public string DeviceMac { get; set; }
        [Required]
        public string DeviceType { get; set; }

        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }


        public string Description { get; set; }


        public string HomeId { get; set; }


        public string RoomId { get; set; }


        public string AssetTenancyId { get; set; }


        public bool IsAssociated { get; set; }


        public string ApartmentId { get; set; }


        public string AssociatedRoomId { get; set; }

        public bool IsPublic { get; set; }

        public string Token { get; set; }


        public DateTimeOffset? AssociateTime { get; set; }


        public string AssociatorName { get; set; }


        public string CenterUUID { get; set; }

        public DateTimeOffset? BindTime { get; set; }

        public bool OnOffLine { get; set; }


        public DateTimeOffset? OnOffTime { get; set; }


        public int DevicePower { get; set; }

 
        public DateTimeOffset? PowerRefreshTime { get; set; }


        public string Model { get; set; }


        public string ModelName { get; set; }


        public int Lqi { get; set; }


        public DateTimeOffset? LqiRefreshTime { get; set; }


        public string Versions { get; set; }


        public string Supplier { get; set; }

        public string OpenId { get; set; }

        public string DeviceCustomName { get; set; }

        [Column("OtherData")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string OtherDataPayload
        {
            get => OtherData.ToJson(JsonSettings.DataJsonSettings);
            set => OtherData = new Dictionary<string, string>(value.FromJson<Dictionary<string, string>>(JsonSettings.DataJsonSettings), StringComparer.OrdinalIgnoreCase);
        }

        [JsonIgnore]
        [NotMapped]
        public Dictionary<string, string> OtherData { get; set; } = new Dictionary<string, string>();

        public bool Enabled { get; set; }
    }
    public abstract class KolibreActorState
    {
        protected KolibreActorState()
        {
        }

        public string this[string key]
        {
            get => Data.ContainsKey(key) ? Data[key] : null;
            set
            {
                if (value == null)
                {
                    Data.Remove(key);
                }

                Data[key] = value;
            }
        }


        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key]
        public string ActorId { get; set; }

        public DateTimeOffset CreateTime { get; set; } = default(DateTimeOffset);

        public DateTimeOffset UpdateTime { get; set; }

        [Column("Data")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string DataPayload
        {
            get => Data.ToJson(JsonSettings.DataJsonSettings);
            set => Data = new Dictionary<string, string>(value.FromJson<Dictionary<string, string>>(JsonSettings.DataJsonSettings), StringComparer.OrdinalIgnoreCase);
        }

        [JsonIgnore]
        [NotMapped]
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [JsonIgnore]
        [NotMapped]
        public IList<string> DataKeys => Data.Keys.ToList();
    }
}
