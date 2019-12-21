using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace EntityCache
{
    // Static class that will serve us convert any class of type T to a Dictionary<string, object>
    // where string is class member name and object is the member value.
    public static class ObjectAndDictionaryTwoWayConverter
    {
        public static Dictionary<string, object> ConvertObjectToDictionary<T>(T i_EntityToConvert) where T: Entity
        {
            Dictionary<string, object> entityDictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(i_EntityToConvert))
            {
                object value = property.GetValue(i_EntityToConvert);
                entityDictionary.Add(property.Name, value);
            }

            return entityDictionary;
        }

         public static T ConvertDictionaryToObject<T>(Dictionary<string, object> i_EntityDictionary) where T: Entity, new()
        {
            var tObject = new T();
            var tObjectType = tObject.GetType();

            foreach (var item in i_EntityDictionary)
            {
                PropertyInfo p = tObjectType.GetProperty(item.Key);
                Type obj = p.PropertyType;
                tObjectType.GetProperty(item.Key).SetValue(tObject, Convert.ChangeType(item.Value, obj), null);
            }

            return tObject;
        }

    }
}
