using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using DataAccess.Entities;

namespace DataAccess
{
	public static class RegisterMapping
	{
		public static void Register()
		{
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new CustomerMap());
                config.ForDommel();
            });
        }
    }
}
