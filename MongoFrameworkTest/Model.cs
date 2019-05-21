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
            EntityMapping.GetOrCreateDefinition(typeof(DeviceView)).CollectionName = "KC.Fengniaowu.Hebe.Devices";
        }

        public MongoDbSet<DeviceView> DeviceModels { get; set; }
    }
    public class DeviceView : KolibreActorState
    {
        /// <summary>
        ///     设备唯一标识 (UUID)
        /// </summary>
        [Required]
        public string DeviceId { get; set; }

        /// <summary>
        ///     设备mac地址
        /// </summary>
        public string DeviceMac { get; set; }

        /// <summary>
        ///     设备类型
        /// </summary>
        [Required]
        public string DeviceType { get; set; }

        /// <summary>
        ///     设备序列号
        /// </summary>
        public string DeviceSn { get; set; }

        /// <summary>
        ///     自定义设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        ///     自定义设备描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     设备所在房源Id (商户定义)
        /// </summary>
        public string HomeId { get; set; }

        /// <summary>
        ///     设备所在房间Id (商户定义)
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        ///     设备所属商户Id
        /// </summary>
        public string AssetTenancyId { get; set; }

        /// <summary>
        ///     设备是否关联至蜂鸟平台
        /// </summary>
        public bool IsAssociated { get; set; }

        /// <summary>
        ///     设备绑定的公寓Id
        /// </summary>
        public string ApartmentId { get; set; }

        /// <summary>
        ///     设备关联房间Id
        /// </summary>
        public string AssociatedRoomId { get; set; }

        public bool IsPublic { get; set; }

        /// <summary>
        ///     进行关联操作用户的访问凭证
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     关联时间
        /// </summary>
        public DateTimeOffset? AssociateTime { get; set; }

        /// <summary>
        ///     关联人
        /// </summary>
        public string AssociatorName { get; set; }

        /// <summary>
        ///     绑定网关uuid (门锁)  TODO
        /// </summary>
        public string CenterUUID { get; set; }

        /// <summary>
        ///     设备绑定时间 (接口返回 - Lock)
        /// </summary>
        public DateTimeOffset? BindTime { get; set; }

        /// <summary>
        ///     设备在线状态
        /// </summary>
        public bool OnOffLine { get; set; }

        /// <summary>
        ///     最近一次在线状态更新时间
        /// </summary>
        public DateTimeOffset? OnOffTime { get; set; }

        /// <summary>
        ///     设备电量
        /// </summary>
        public int DevicePower { get; set; }

        /// <summary>
        ///     最近一次电量刷新时间
        /// </summary>
        public DateTimeOffset? PowerRefreshTime { get; set; }

        /// <summary>
        ///     设备型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        ///     设备型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        ///     设备信号值 (门锁)
        /// </summary>
        public int Lqi { get; set; }

        /// <summary>
        ///     信号刷新时间
        /// </summary>
        public DateTimeOffset? LqiRefreshTime { get; set; }

        /// <summary>
        ///     设备版本信息 (Json字符串)
        /// </summary>
        public string Versions { get; set; }

        /// <summary>
        ///     设备供应商
        /// </summary>
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

        /// <summary>
        ///     其他数据
        /// </summary>
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
        //public long Id { get; set; }

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
