using System;
using System.Text;
using System.Text.Json;
using System.ComponentModel;

namespace Common.Models
{
    public abstract class Model
    {
        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(this);
                stringBuilder.Append($"{name}: {value} ");
            }
            return stringBuilder.ToString().TrimEnd();
        }

        public T ConvertToEntity<T>()
        {
            var serialized = JsonSerializer.Serialize(this);
            var entity = JsonSerializer.Deserialize<T>(serialized);

            if (entity == null)
            {
                throw new FormatException();
            }

            return entity;
        }

        public static T Parse<T>(object entity) where T : Model
        {
            var entitySerialized = JsonSerializer.Serialize(entity);
            var model = JsonSerializer.Deserialize<T>(entitySerialized);

            if (model == null)
            {
                throw new FormatException();
            }

            return model;
        }
    }
}
