
using System;
using System.Collections.Generic;

namespace MyModelBaseTest
{
	public class MemberSettingContext<T> : MemberGettingContext
	{
		#region 成员字段
		private T _value;
		private int _valueEvaluated;
		private Func<T> _valueFactory;
		#endregion

		#region 构造函数
		internal MemberSettingContext(object owner, MemberPathSegment memberToken, T value, MemberGettingContext parent = null) : base(owner, memberToken, parent)
		{
			_value = value;
			_valueEvaluated = 1;
		}

		internal MemberSettingContext(object owner, MemberPathSegment memberToken, Func<T> valueFactory, MemberGettingContext parent = null) : base(owner, memberToken, parent)
		{
			if(valueFactory == null)
				throw new ArgumentNullException(nameof(valueFactory));

			_valueFactory = valueFactory;
		}
		#endregion

		#region 公共属性
		/// <summary>
		/// 获取设置的值。
		/// </summary>
		/// <remarks>
		///		<para>如果指定的是一个取值工厂，即使多次获取该属性亦可确保取值计算只会被调用一次。</para>
		/// </remarks>
		public T Value
		{
			get
			{
				if(_valueFactory != null && _valueEvaluated == 0 && System.Threading.Interlocked.CompareExchange(ref _valueEvaluated, 1, 0) == 0)
					_value = _valueFactory();

				return _value;
			}
		}
		#endregion

		#region 公共方法
		public void Setup(bool throwsOnError = true)
		{
			MemberAccess.SetMemberValueCore(this.Owner, this.MemberToken, () => this.Value, throwsOnError);
		}
		#endregion
	}
}
