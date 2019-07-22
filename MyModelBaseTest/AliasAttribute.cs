using System;
using System.Collections.Generic;
using System.Text;

namespace MyModelBaseTest
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public class AliasAttribute : Attribute
	{
		#region 成员变量
		private string _alias;
		#endregion

		#region 构造函数
		public AliasAttribute(string alias)
		{
			_alias = alias ?? string.Empty;
		}
		#endregion

		#region 公共属性
		public string Alias
		{
			get
			{
				return _alias;
			}
		}
		#endregion
	}
}
