using EqualityComparer;
using MongoDB.Driver;
using MongoFramework;
using MongoFramework.Infrastructure.Mapping;
using MyModelBaseTest;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Convert = MyModelBaseTest.Convert;

namespace MongoFrameworkTest
{
    public interface IChangeTrackable : INotifyPropertyChanged
    {
        bool HasChanges();

        bool HasChanges(params string[] names);

        IDictionary<string, object> GetChangedProperties();
        IEnumerable<string> ChangedProperties();
        void Update(IDictionary<string, object> properties);

        void Update();

        void SaveChanges();
    }
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object value, JsonSerializerSettings settings = null)
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
            get => Data.ToJsonString(JsonSettings.DataJsonSettings);
            set => Data = new Dictionary<string, string>(value.FromJson<Dictionary<string, string>>(JsonSettings.DataJsonSettings), StringComparer.OrdinalIgnoreCase);
        }

        [JsonIgnore]
        [NotMapped]
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
    public class DeviceView : TrackableKolibreActorState
    {
        private long _SerialNumber;
        /// <summary>
        /// 序号
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        public virtual long SerialNumber
        {
            get => this._SerialNumber;
            set => this.SetPropertyValue(() => this.SerialNumber, ref _SerialNumber, value);

        }
        private string _DeviceId;
        /// <summary>
        ///     设备唯一标识 (UUID)
        /// </summary>
        [Required]
        public virtual string DeviceId
        {
            get => this._DeviceId;
            set => this.SetPropertyValue(() => this.DeviceId, ref _DeviceId, value);
        }
        private string _DeviceMac;
        /// <summary>
        ///     设备mac地址
        /// </summary>
        public virtual string DeviceMac
        {
            get => this._DeviceMac;
            set => this.SetPropertyValue(() => this.DeviceMac, ref _DeviceMac, value);
        }
        private string _DeviceType;
        /// <summary>
        ///     设备类型
        /// </summary>
        [Required]
        public virtual string DeviceType
        {
            get => this._DeviceType;
            set => this.SetPropertyValue(() => this.DeviceType, ref _DeviceType, value);
        }
        private string _DeviceSn;
        /// <summary>
        ///     设备序列号
        /// </summary>

        public virtual string DeviceSn
        {
            get => this._DeviceSn;
            set => this.SetPropertyValue(() => this.DeviceSn, ref _DeviceSn, value);
        }
        private string _DeviceName;
        /// <summary>
        ///     自定义设备名称
        /// </summary>

        public virtual string DeviceName
        {
            get => this._DeviceName;
            set => this.SetPropertyValue(() => this.DeviceName, ref _DeviceName, value);
        }
        private string _Description;
        /// <summary>
        ///     自定义设备描述
        /// </summary>
        public virtual string Description
        {
            get => this._Description;
            set => this.SetPropertyValue(() => this.Description, ref _Description, value);
        }
        private string _HomeId;
        /// <summary>
        ///     设备所在房源Id (商户定义)
        /// </summary>
        public virtual string HomeId
        {
            get => this._HomeId;
            set => this.SetPropertyValue(() => this.HomeId, ref _HomeId, value);
        }
        private string _RoomId;
        /// <summary>
        ///     设备所在房间Id (商户定义)
        /// </summary>
        public virtual string RoomId
        {
            get => this._RoomId;
            set => this.SetPropertyValue(() => this.RoomId, ref _RoomId, value);
        }
        private string _AssetTenancyId;
        /// <summary>
        ///     设备所属商户Id
        /// </summary>
        public virtual string AssetTenancyId
        {
            get => this._AssetTenancyId;
            set => this.SetPropertyValue(() => this.AssetTenancyId, ref _AssetTenancyId, value);
        }
        private bool _IsAssociated;
        /// <summary>
        ///     设备是否关联至蜂鸟平台
        /// </summary>
        public virtual bool IsAssociated
        {
            get => this._IsAssociated;
            set => this.SetPropertyValue(() => this.IsAssociated, ref _IsAssociated, value);
        }
        private string _ApartmentId;
        /// <summary>
        ///     设备绑定的公寓Id
        /// </summary>
        public virtual string ApartmentId
        {
            get => this._ApartmentId;
            set => this.SetPropertyValue(() => this.ApartmentId, ref _ApartmentId, value);
        }
        private string _AssociatedRoomId;
        /// <summary>
        ///     设备关联房间Id
        /// </summary>
        public virtual string AssociatedRoomId
        {
            get => this._AssociatedRoomId;
            set => this.SetPropertyValue(() => this.AssociatedRoomId, ref _AssociatedRoomId, value);
        }
        private bool _IsPublic;
        public virtual bool IsPublic
        {
            get => this._IsPublic;
            set => this.SetPropertyValue(() => this.IsPublic, ref _IsPublic, value);
        }
        private string _Token;
        /// <summary>
        ///     进行关联操作用户的访问凭证
        /// </summary>
        public virtual string Token
        {
            get => this._Token;
            set => this.SetPropertyValue(() => this.Token, ref _Token, value);
        }
        private DateTimeOffset? _AssociateTime;
        /// <summary>
        ///     关联时间
        /// </summary>
        public virtual DateTimeOffset? AssociateTime
        {
            get => this._AssociateTime;
            set => this.SetPropertyValue(() => this.AssociateTime, ref _AssociateTime, value);
        }
        public string _AssociatorName;
        /// <summary>
        ///     关联人
        /// </summary>
        public virtual string AssociatorName
        {
            get => this._AssociatorName;
            set => this.SetPropertyValue(() => this.AssociatorName, ref _AssociatorName, value);
        }
        private string _CenterUUID;
        /// <summary>
        ///     绑定网关uuid (门锁)  TODO
        /// </summary>
        public virtual string CenterUUID
        {
            get => this._CenterUUID;
            set => this.SetPropertyValue(() => this.CenterUUID, ref _CenterUUID, value);
        }
        private DateTimeOffset? _BindTime;
        /// <summary>
        ///     设备绑定时间 (接口返回 - Lock)
        /// </summary>
        public virtual DateTimeOffset? BindTime
        {
            get => this._BindTime;
            set => this.SetPropertyValue(() => this.BindTime, ref _BindTime, value);
        }
        private bool _OnOffLine;
        /// <summary>
        ///     设备在线状态
        /// </summary>
        public virtual bool OnOffLine
        {
            get => this._OnOffLine;
            set => this.SetPropertyValue(() => this.OnOffLine, ref _OnOffLine, value);
        }
        private DateTimeOffset? _OnOffTime;
        /// <summary>
        ///     最近一次在线状态更新时间
        /// </summary>
        public virtual DateTimeOffset? OnOffTime
        {
            get => this._OnOffTime;
            set => this.SetPropertyValue(() => this.OnOffTime, ref _OnOffTime, value);
        }
        private int _DevicePower;
        /// <summary>
        ///     设备电量
        /// </summary>
        public virtual int DevicePower
        {
            get => this._DevicePower;
            set => this.SetPropertyValue(() => this.DevicePower, ref _DevicePower, value);
        }
        private DateTimeOffset? _PowerRefreshTime;
        /// <summary>
        ///     最近一次电量刷新时间
        /// </summary>
        public virtual DateTimeOffset? PowerRefreshTime
        {
            get => this._PowerRefreshTime;
            set => this.SetPropertyValue(() => this.PowerRefreshTime, ref _PowerRefreshTime, value);
        }
        private string _Model;
        /// <summary>
        ///     设备型号
        /// </summary>
        public virtual string Model
        {
            get => this._Model;
            set => this.SetPropertyValue(() => this.Model, ref _Model, value);
        }
        private string _ModelName;
        /// <summary>
        ///     设备型号名称
        /// </summary>
        public virtual string ModelName
        {
            get => this._ModelName;
            set => this.SetPropertyValue(() => this.ModelName, ref _ModelName, value);
        }
        private int _Lqi;
        /// <summary>
        ///     设备信号值 (门锁)
        /// </summary>
        public virtual int Lqi
        {
            get => this._Lqi;
            set => this.SetPropertyValue(() => this.Lqi, ref _Lqi, value);
        }
        private DateTimeOffset? _LqiRefreshTime;
        /// <summary>
        ///     信号刷新时间
        /// </summary>
        public virtual DateTimeOffset? LqiRefreshTime
        {
            get => this._LqiRefreshTime;
            set => this.SetPropertyValue(() => this.LqiRefreshTime, ref _LqiRefreshTime, value);
        }
        private string _Versions;
        /// <summary>
        ///     设备版本信息 (Json字符串)
        /// </summary>
        public virtual string Versions
        {
            get => this._Versions;
            set => this.SetPropertyValue(() => this.Versions, ref _Versions, value);
        }
        private string _Supplier;
        /// <summary>
        ///     设备供应商
        /// </summary>
        public virtual string Supplier
        {
            get => this._Supplier;
            set => this.SetPropertyValue(() => this.Supplier, ref _Supplier, value);
        }
        private string _OpenId;
        public virtual string OpenId
        {
            get => this._OpenId;
            set => this.SetPropertyValue(() => this.OpenId, ref _OpenId, value);
        }
        private string _DeviceCustomName;
        public virtual string DeviceCustomName
        {
            get => this._DeviceCustomName;
            set => this.SetPropertyValue(() => this.DeviceCustomName, ref _DeviceCustomName, value);
        }
        [Column("OtherData")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string OtherDataPayload
        {
            get
            {
                return OtherData.ToJsonString(JsonSettings.DataJsonSettings);
            }
            set
            {
                OtherData = new Dictionary<string, string>(value.FromJson<Dictionary<string, string>>(JsonSettings.DataJsonSettings), StringComparer.OrdinalIgnoreCase);
                this.SetPropertyValue(() => this.OtherDataPayload, value);
            }
        }
        [JsonIgnore]
        [NotMapped]
        public Dictionary<string, string> OtherData { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private bool _Enabled;
        public virtual bool Enabled
        {
            get => this._Enabled;
            set => this.SetPropertyValue(() => this.Enabled, ref _Enabled, value);
        }
    }
    public class KolibreActorState 
    {
        protected KolibreActorState()
        {
        }

        protected KolibreActorState(string actorId)
        {
            ActorId = actorId.ToString();
        }
        public string GetData(string key)
        {
            return Data.ContainsKey(key) ? Data[key] : null;
        }
        public void SetData(string key, string value)
        {
            if (value == null)
            {
                Data.Remove(key);
            }

            Data[key] = value;
        }
        [Key]
        public virtual string ActorId { get; set; }


        public virtual DateTimeOffset CreateTime { get; set; }


        public virtual DateTimeOffset UpdateTime { get; set; }

        [Column("Data")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string DataPayload
        {
            get
            {
                return Data.ToJsonString(JsonSettings.DataJsonSettings);
            }
            set
            {
                Data = new Dictionary<string, string>(value.FromJson<Dictionary<string, string>>(JsonSettings.DataJsonSettings), StringComparer.OrdinalIgnoreCase);
            }
        }
        [JsonIgnore]
        [NotMapped]
        public virtual Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [JsonIgnore]
        [NotMapped]
        public virtual IList<string> DataKeys => Data.Keys.ToList();
    }
    public abstract class TrackableKolibreActorState : KolibreActorState, IChangeTrackable
    {
        #region Private member field

        private object _WriteLock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        private ConcurrentDictionary<string, PropertyToken> _properties;
        private ConcurrentDictionary<string, object> _changedProperties;

        #endregion Private member field

        #region Member field

        [Key]
        public override string ActorId
        {
            get => this.GetPropertyValue(() => this.ActorId);
            set => this.SetPropertyValue(() => this.ActorId, value);
        }

        public override DateTimeOffset CreateTime
        {
            get => this.GetPropertyValue(() => this.CreateTime);
            set => this.SetPropertyValue(() => this.CreateTime, value);
        }

        public override DateTimeOffset UpdateTime
        {
            get => this.GetPropertyValue(() => this.UpdateTime);
            set => this.SetPropertyValue(() => this.UpdateTime, value);
        }

        [Column("Data")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string DataPayload
        {
            get
            {
                return Data.ToJsonString(JsonSettings.DataJsonSettings);
            }
            set
            {
                Data = new Dictionary<string, string>(value.FromJson<Dictionary<string, string>>(JsonSettings.DataJsonSettings), StringComparer.OrdinalIgnoreCase);
                this.SetPropertyValue(() => this.DataPayload, value);
            }
        }

        [JsonIgnore]
        [NotMapped]
        public override Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [JsonIgnore]
        [NotMapped]
        public override IList<string> DataKeys => Data.Keys.ToList();

        #endregion Member field

        #region Constructor

        protected TrackableKolibreActorState()
        {
        }

        protected TrackableKolibreActorState(string actorId)
        {
            ActorId = actorId.ToString();
        }

        protected TrackableKolibreActorState(TrackableKolibreActorState model)
        {
            if (model == null)
                return;

            var properties = model.GetChangedProperties();

            foreach (var property in properties)
            {
                this.SetPropertyValue(property.Key, property.Value);
            }
        }

        #endregion Constructor

        #region Protection attribute

        protected bool HasProperties()
        {
            return _properties != null && _properties.Count > 0;
        }

        protected ConcurrentDictionary<string, PropertyToken> Properties()
        {
            if (_properties == null)
                System.Threading.Interlocked.CompareExchange(ref _properties, new ConcurrentDictionary<string, PropertyToken>(), null);
            return _properties;
        }

        #endregion Protection attribute

        #region Protection method

        protected T GetPropertyValue<T>(string propertyName, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            PropertyToken token;
            var properties = _properties;

            if (properties != null && properties.TryGetValue(propertyName, out token))
                return (T)token.Value;

            var property = this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (property == null)
                throw new InvalidOperationException(string.Format("The '{0}' property is not exists.", propertyName));

            return this.GetPropertyDefaultValue(property, defaultValue);
        }

        protected T GetPropertyValue<T>(string propertyName, Func<T> valueFactory)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            var properties = _properties;
            PropertyToken token;

            if (properties != null && properties.TryGetValue(propertyName, out token))
                return (T)token.Value;

            var value = valueFactory();
            this.SetPropertyValue(propertyName, value);
            return value;
        }

        protected T GetPropertyValue<T>(Expression<Func<T>> propertyExpression, T defaultValue = default(T))
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var property = this.GetPropertyInfo(propertyExpression);
            var properties = _properties;
            PropertyToken token;

            if (properties != null && properties.TryGetValue(property.Name, out token))
                return (T)token.Value;
            return this.GetPropertyDefaultValue(property, defaultValue);
        }

        protected T GetPropertyValue<T>(Expression<Func<T>> propertyExpression, Func<T> valueFactory)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            var property = this.GetPropertyInfo(propertyExpression);
            var properties = _properties;
            PropertyToken token;

            if (properties != null && properties.TryGetValue(property.Name, out token))
                return (T)token.Value;

            var value = valueFactory();
            this.SetPropertyValueCore(property, value);
            return value;
        }

        protected void SetPropertyValue<T>(string propertyName, ref T target, T value)
        {
            if (IsSame(propertyName, value)) return;
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            target = value;
            this.RaisePropertyChanged(propertyName, value);
        }

        protected void SetPropertyValue<T>(Expression<Func<T>> propertyExpression, ref T target, T value)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));
            var property = this.GetPropertyInfo(propertyExpression);
            if (IsSame(property.Name, value)) return;
            target = value;

            this.RaisePropertyChanged(property.Name, value);
        }

        protected void SetPropertyValue<T>(string propertyName, T value, Action<T, T> onChanged = null)
        {
            if (IsSame(propertyName, value)) return;
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var changed = true;
            var properties = this.Properties();
            object originalValue = null;

            properties.AddOrUpdate(propertyName,
                key =>
                {
                    return this.CreatePropertyToken(propertyName, typeof(T), value);
                }, (_, original) =>
                {
                    originalValue = original.Value;
                    var propertyValue = this.OnPropertySet(original.Name, original.Type, original.Value, value);
                    changed = !object.Equals(propertyValue, original.Value);

                    if (changed)
                        return original.Clone(propertyValue);
                    else
                        return original;
                });

            if (changed)
            {
                var newValue = properties[propertyName].Value;

                onChanged?.Invoke((T)originalValue, (T)newValue);

                this.RaisePropertyChanged(propertyName, newValue);
            }
        }

        protected void SetPropertyValue<T>(Expression<Func<T>> propertyExpression, T value, Action<T, T> onChanged = null)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));
            var property = this.GetPropertyInfo(propertyExpression);
            if (IsSame(property.Name, value)) return;
            if (onChanged == null)
                this.SetPropertyValueCore(property, value);
            else
                this.SetPropertyValueCore(property, value, (oldValue, newValue) => onChanged((T)oldValue, (T)newValue));
        }

        private void SetPropertyValueCore(PropertyInfo property, object value, Action<object, object> onChanged = null)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var changed = true;
            var properties = this.Properties();
            object originalValue = null;

            properties.AddOrUpdate(property.Name,
                key =>
                {
                    originalValue = this.GetPropertyDefaultValue(property);

                    var propertyValue = this.OnPropertySet(property.Name, property.PropertyType, originalValue, value);
                    return this.CreatePropertyToken(property.Name, property.PropertyType, propertyValue);
                }, (_, original) =>
                {
                    originalValue = original.Value;
                    var propertyValue = this.OnPropertySet(original.Name, original.Type, original.Value, value);
                    changed = !object.Equals(propertyValue, original.Value);

                    if (changed)
                        return original.Clone(propertyValue);
                    else
                        return original;
                });

            if (changed)
            {
                var newValue = properties[property.Name].Value;

                if (onChanged != null)
                    onChanged(originalValue, newValue);

                this.RaisePropertyChanged(property.Name, newValue);
            }
        }

        #endregion Protection method

        #region 公共方法

        /// <summary>
        /// 返回当前对象是否有属性值被改变过。
        /// </summary>
        public bool HasChanges()
        {
            return _changedProperties != null && _changedProperties.Count > 0;
        }

        /// <summary>
        /// 返回指定名称的属性是否被改变过。
        /// </summary>
        /// <param name="names">指定要判断的属性名数组。</param>
        /// <returns>返回一个值，指示指定的属性是否发生过改变。</returns>
        /// <remarks>
        ///		<para>如果指定了多个属性名，则其中任意一个属性值发生过改变，返回值即为真(True)。</para>
        ///		<para>如果没有指定属性名（即<paramref name="names"/>参数为空(null)或零个成员）则该实例中只要有任何属性发生过改变都返回真(True)。</para>
        /// </remarks>
        public bool HasChanges(params string[] names)
        {
            var isChanged = _changedProperties != null && _changedProperties.Count > 0;

            if (names == null || names.Length == 0)
                return isChanged;

            if (isChanged)
            {
                foreach (var name in names)
                {
                    if (_changedProperties.ContainsKey(name))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取当前对象中被改变过的属性集。
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetChangedProperties()
        {
            if (_changedProperties == null)
                System.Threading.Interlocked.CompareExchange(ref _changedProperties, new ConcurrentDictionary<string, object>(), null);

            return _changedProperties;
        }

        /// <summary>
        /// 更新当前对象的属性值。
        /// </summary>
        /// <param name="properties">指定要更新的属性集。</param>
        public void Update(IDictionary<string, object> properties)
        {
            if (properties == null)
                return;

            foreach (var property in properties)
            {
                this.SetPropertyValue(property.Key, property.Value);
            }
        }

        public IEnumerable<string> ChangedProperties()
        {
            return _changedProperties.Keys;
        }

        public void Update()
        {
            Update(GetChangedProperties());
        }

        public void SaveChanges()
        {
            Monitor.Enter(_WriteLock);
            try
            {
                Update(GetChangedProperties());
                _changedProperties = new ConcurrentDictionary<string, object>();
            }
            finally
            {
                Monitor.Exit(_WriteLock);
            }
        }

        #endregion 公共方法

        #region 虚拟方法

        protected virtual object OnPropertySet(string name, Type type, object oldValue, object newValue)
        {
            return MyModelBaseTest.Convert.ConvertValue(newValue, type);
        }

        protected virtual PropertyToken CreatePropertyToken(string name, Type type, object value)
        {
            return new PropertyToken(name, type, value);
        }

        #endregion 虚拟方法

        #region 激发事件

        private void RaisePropertyChanged(string propertyName, object value)
        {
            //将发生改变的属性加入到变更集中
            this.SetChangedProperty(propertyName, value);

            //激发“PropertyChanged”事件
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            this.PropertyChanged?.Invoke(this, args);
        }

        #endregion 激发事件

        #region 私有方法

        private bool IsSame(string propertyName, object value)
        {
            if (_properties == null)
            {
                return false;
            }
            PropertyToken token;
            _properties.TryGetValue(propertyName, out token);
            return MemberComparer.Equal(token.Value, value);
        }

        private void SetChangedProperty(string name, object value)
        {
            if (_changedProperties == null)
                System.Threading.Interlocked.CompareExchange(ref _changedProperties, new ConcurrentDictionary<string, object>(), null);

            _changedProperties[name] = value;
        }

        private PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("The expression is not a property expression.", nameof(propertyExpression));

            if (memberExpression.Member.MemberType != MemberTypes.Property)
                throw new InvalidOperationException($"The '{memberExpression.Member.Name}' member is not property.");

            return (PropertyInfo)memberExpression.Member;
        }

        private object GetPropertyDefaultValue(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

            if (attribute != null)
                return Convert.ConvertValue(attribute.Value, property.PropertyType);

            return TypeExtension.GetDefaultValue(property.PropertyType);
        }

        private T GetPropertyDefaultValue<T>(PropertyInfo property, T defaultValue)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

            if (attribute != null)
                return Convert.ConvertValue<T>(attribute.Value, defaultValue);

            return defaultValue;
        }

        #endregion 私有方法
    }
    public struct PropertyToken
    {
        #region 成员字段

        public readonly string Name;
        public readonly Type Type;
        public object Value;

        #endregion 成员字段

        #region 构造函数

        public PropertyToken(string name, Type type, object value)
        {
            if (string.IsNullOrEmpty((name)))
                throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Value = value;
        }

        #endregion 构造函数

        #region 公共方法

        public PropertyToken Clone(object value)
        {
            return new PropertyToken(this.Name, this.Type, value);
        }

        #endregion 公共方法

        #region 重写方法

        public override string ToString()
        {
            return string.Format("{0}({1})={2}", this.Name, this.Type.Name, this.Value);
        }

        #endregion 重写方法
    }
}
