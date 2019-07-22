using System;

namespace MyModelBaseTest
{
	/// <summary>
	/// 表示对象实例的创建器的接口。
	/// </summary>
	public interface IActivator
	{
		/// <summary>
		/// 返回一个值指示是否能创建指定类型的实例。
		/// </summary>
		/// <param name="type">指定是否可创建的类型。</param>
		/// <param name="argument">指定的判断参数。</param>
		/// <returns>返回是否能创建的值，如果为真(True)表示可以创建，否则不能创建。</returns>
		bool CanCreate(Type type, object argument);

		/// <summary>
		/// 创建指定类型的实例。
		/// </summary>
		/// <param name="type">指定要创建的对象类型。</param>
		/// <param name="argument">指定的创建参数。</param>
		/// <param name="binder">指定的构造函数参数绑定委托。</param>
		/// <returns>返回创建的对象实例。</returns>
		object Create(Type type, object argument, Func<ActivatorParameterDescriptor, bool> binder = null);
	}
}
