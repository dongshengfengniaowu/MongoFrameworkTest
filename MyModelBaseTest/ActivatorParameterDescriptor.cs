using System;
using System.Collections.Generic;

namespace MyModelBaseTest
{
	public class ActivatorParameterDescriptor
	{
		#region 成员字段
		private object _argument;
		private string _parameterName;
		private Type _parameterType;
		private object _parameterValue;
		#endregion

		#region 构造函数
		public ActivatorParameterDescriptor(string parameterName, Type parameterType, object argument)
		{
			if(string.IsNullOrWhiteSpace(parameterName))
				throw new ArgumentNullException(nameof(parameterName));
			if(parameterType == null)
				throw new ArgumentNullException(nameof(parameterType));

			_parameterName = parameterName;
			_parameterType = parameterType;
			_argument = argument;
		}
		#endregion

		#region 公共属性
		public string ParameterName
		{
			get
			{
				return _parameterName;
			}
		}

		public Type ParameterType
		{
			get
			{
				return _parameterType;
			}
		}

		public object Argument
		{
			get
			{
				return _argument;
			}
		}

		public object ParameterValue
		{
			get
			{
				return _parameterValue;
			}
			set
			{
				_parameterValue = value;
			}
		}
		#endregion
	}
}
