using System;
using System.ComponentModel;
using System.Data;

namespace SWPCCBilling.Infrastructure
{
    public static class DataReaderExtensions
    {
        public static T GetObject<T>(this IDataReader dr)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(typeof(T)))
            {
                if (property.IsReadOnly)
                    continue;

                try
                {
                    int ordinal = dr.GetOrdinal(property.Name);
                    if (!dr.IsDBNull(ordinal))
                    {
                        object sourceValue = dr.GetValue(ordinal);
                        object targetValue = null;

                        if (sourceValue != null)
                        {
                            Type conversionType = property.PropertyType;

                            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var converter = new NullableConverter(property.PropertyType);
                                conversionType = converter.UnderlyingType;
                            }

                            targetValue = Convert.ChangeType(sourceValue, conversionType);
                        }

                        if (targetValue != null)
                            property.SetValue(obj, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(String.Format("Failed to set property: {0}", property.Name), ex);
                }
            }

            return obj;
        }
    }
}