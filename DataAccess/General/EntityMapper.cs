using System;
using System.Linq;
using Dapper;

namespace DataAccess
{
	public class EntityMapper
	{
		public static void RegisterTypeMaps()
		{
			var types = from assem in AppDomain.CurrentDomain.GetAssemblies().ToList()
						from type in assem.GetTypes()
						where type.IsClass && type.Namespace == "DataAccess.Entities"
						select type;

			types.ToList().ForEach(type =>
			{
				var mapper = (SqlMapper.ITypeMap)Activator
					.CreateInstance(typeof(ColumnAttributeTypeMapper<>)
									.MakeGenericType(type));
				SqlMapper.SetTypeMap(type, mapper);
			});
		}
	}
}
