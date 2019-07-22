
using System;

namespace MyModelBaseTest
{
	/// <summary>
	/// 表示成员访问路径段的结构。
	/// </summary>
	public struct MemberPathSegment
	{
		#region 公共字段
		/// <summary>
		/// 获取路径段的名称。
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// 获取路径段的访问器参数值列表。
		/// </summary>
		public readonly object[] Parameters;
		#endregion

		#region 构造函数
		public MemberPathSegment(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			this.Name = name;
			this.Parameters = null;
		}

		public MemberPathSegment(object[] parameters)
		{
			if(parameters == null || parameters.Length == 0)
				throw new ArgumentNullException(nameof(parameters));

			this.Name = string.Empty;
			this.Parameters = parameters;
		}
		#endregion

		#region 公共属性
		/// <summary>
		/// 获取一个值，指示当前路径段是否表示索引器。
		/// </summary>
		public bool IsIndexer
		{
			get
			{
				return this.Parameters != null && this.Parameters.Length > 0;
			}
		}
		#endregion

		#region 重写方法
		public override string ToString()
		{
			if(!string.IsNullOrEmpty(this.Name))
				return this.Name;

			var result = new System.Text.StringBuilder();

			foreach(var parameter in this.Parameters)
			{
				if(result.Length > 0)
					result.Append(", ");

				if(parameter == null)
					result.Append("null");
				else
				{
					if(parameter is string)
						result.Append("\"" + parameter + "\"");
					else
						result.Append(parameter.ToString());
				}
			}

			return "[" + result.ToString() + "]";
		}
		#endregion
	}
}
