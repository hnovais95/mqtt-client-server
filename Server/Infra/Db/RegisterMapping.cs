using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

namespace Server.Infra.Db
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
