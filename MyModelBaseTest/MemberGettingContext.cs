
using System;
using System.Reflection;

namespace MyModelBaseTest
{
	/// <summary>
	/// 表示在成员访问程序中的操作上下文。
	/// </summary>
	public class MemberGettingContext
	{
		#region 成员字段
		private object _owner;
		private MemberPathSegment _memberSegment;
		private MemberInfo _memberInfo;
		private MemberGettingContext _parent;
		#endregion

		#region 构造函数
		internal MemberGettingContext(object owner, MemberPathSegment memberSegment, MemberGettingContext parent = null)
		{
			_owner = owner ?? throw new ArgumentNullException(nameof(owner));
			_parent = parent;
			_memberSegment = memberSegment;
		}
		#endregion

		#region 公共属性
		/// <summary>
		/// 获取当前访问上下文的父元素上下文。
		/// </summary>
		public MemberGettingContext Parent
		{
			get
			{
				return _parent;
			}
		}

		/// <summary>
		/// 获取解析过程中当前成员的所有者对象。
		/// </summary>
		public object Owner
		{
			get
			{
				return _owner;
			}
		}

		/// <summary>
		/// 获取当前访问的成员标志。
		/// </summary>
		public MemberPathSegment MemberToken
		{
			get
			{
				return _memberSegment;
			}
		}

		/// <summary>
		/// 获取当前访问的成员信息。
		/// </summary>
		public MemberInfo MemberInfo
		{
			get
			{
				if(_memberInfo == null)
					_memberInfo = MemberAccess.GetMemberInfo(_owner is Type ? (Type)_owner : _owner.GetType(), _memberSegment);

				return _memberInfo;
			}
		}

		/// <summary>
		/// 获取当前访问成员的类型。
		/// </summary>
		public Type MemberType
		{
			get
			{
				if(_owner is Type)
					return (Type)_owner;

				return MemberAccess.GetMemberType(this.MemberInfo);
			}
		}
		#endregion

		#region 公共方法
		public object GetMemberValue()
		{
			object value;

			if(MemberAccess.TryGetMemberValueCore(_owner, _memberSegment, out value))
				return value;

			throw new InvalidOperationException(string.Format("The '{0}' member is not exists in the '{1}' type.", _memberSegment, (_owner is Type ? ((Type)_owner).FullName : _owner.GetType().FullName)));
		}

		public bool TryGetMemberValue(out object value)
		{
			return MemberAccess.TryGetMemberValueCore(_owner, _memberSegment, out value);
		}
		#endregion
	}
}
