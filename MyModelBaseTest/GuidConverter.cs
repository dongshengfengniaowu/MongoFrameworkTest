using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace MyModelBaseTest
{
	public class GuidConverter : System.ComponentModel.GuidConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if(sourceType == typeof(byte[]))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType == typeof(InstanceDescriptor) || destinationType == typeof(byte[]))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if(value is byte[])
			{
				byte[] array = value as byte[];

				if(array.Length == 16)
					return new Guid(array);

				if(array.Length > 16)
				{
					return new Guid(BitConverter.ToUInt32(array, 0), BitConverter.ToUInt16(array, 4), BitConverter.ToUInt16(array, 6),
						array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15]);
				}
			}

			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if(destinationType == null)
				throw new ArgumentNullException("destinationType");

			if(destinationType == typeof(InstanceDescriptor) && value is Guid)
			{
				ConstructorInfo ctor = typeof(Guid).GetConstructor(new Type[] { typeof(string) });

				if(ctor != null)
					return new InstanceDescriptor(ctor, new object[] { value.ToString() });
			}
			else if(destinationType == typeof(byte[]) && value is Guid)
				return ((Guid)value).ToByteArray();

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
