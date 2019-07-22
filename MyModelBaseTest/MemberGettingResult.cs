
using System;

namespace MyModelBaseTest
{
	public struct MemberGettingResult
	{
		public bool Cancel;
		public object Value;

		public MemberGettingResult(object value, bool cancel = false)
		{
			this.Value = value;
			this.Cancel = cancel;
		}
	}
}
