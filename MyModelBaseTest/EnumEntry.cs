
using System;

namespace MyModelBaseTest
{
	/// <summary>
	/// 表示枚举项的描述。
	/// </summary>
	public struct EnumEntry : IFormattable, IFormatProvider, IEquatable<EnumEntry>
	{
		#region 构造函数
		public EnumEntry(Type type, string name, object value, string alias, string description)
		{
			this.Type = type ?? throw new ArgumentNullException(nameof(type));
			this.Name = name;
			this.Value = value;
			this.Alias = alias;
			this.Description = description;
		}
		#endregion

		#region 公共字段
		/// <summary>
		/// 获取枚举项的名称。
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// 获取枚举的类型。
		/// </summary>
		public readonly Type Type;

		/// <summary>
		/// 当前描述的枚举项值，该值有可能为枚举项的值也可能是对应的基元类型值。
		/// </summary>
		public readonly object Value;

		/// <summary>
		/// 获取枚举项的别名，如果未定义建议创建者设置为枚举项的名称。
		/// </summary>
		/// <remarks>枚举项的别名由<seealso cref="Zongsoft.ComponentModel.AliasAttribute"/>自定义特性指定。</remarks>
		public readonly string Alias;

		/// <summary>
		/// 当前描述枚举项的描述文本，如果未定义建议创建者设置为枚举项的名称。
		/// </summary>
		/// <remarks>枚举项的描述由<seealso cref="System.ComponentModel.DescriptionAttribute"/>自定义特性指定。</remarks>
		public readonly string Description;
		#endregion

		#region 重写方法
		public bool Equals(EnumEntry entry)
		{
			return this.Type == entry.Type &&
			       object.Equals(this.Value, entry.Value);
		}

		public override bool Equals(object obj)
		{
			if(obj == null || obj.GetType() != this.GetType())
				return false;

			return this.Equals((EnumEntry)obj);
		}

		public override int GetHashCode()
		{
			if(this.Type == null || this.Value == null || string.IsNullOrEmpty(this.Name))
				return 0;

			return this.Value.GetHashCode();
		}

		public override string ToString()
		{
			if(this.Type == null || this.Value == null || string.IsNullOrEmpty(this.Name))
				return string.Empty;

			string value;

			if(this.Value.GetType().IsPrimitive)
				value = this.Value.ToString();
			else
				value = System.Convert.ChangeType(this.Value, Enum.GetUnderlyingType(this.Type)).ToString();

			return string.Format("{0}.{1}={2}", this.Type.Name, this.Name, value);
		}
		#endregion

		#region 格式方法
		public string ToString(string format)
		{
			if(string.IsNullOrEmpty(format))
				return this.ToString();

			switch(format.Trim().ToLowerInvariant())
			{
				case "d":
				case "description":
					return this.Description;
				case "n":
				case "name":
					return this.Name;
				case "a":
				case "alias":
					return this.Alias;
				case "f":
				case "full":
				case "fullname":
					return this.ToString();
			}

			return this.Value.ToString();
		}

		string IFormattable.ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToString(format);
		}

		object IFormatProvider.GetFormat(Type formatType)
		{
			if(formatType == typeof(ICustomFormatter))
				return this;

			return null;
		}
		#endregion
	}
}