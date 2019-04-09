
using AutoMapper;
using HC.AbpCore.Companys.Accounts;
using HC.AbpCore.Companys.Accounts.Dtos;

namespace HC.AbpCore.Companys.Accounts.Mapper
{

	/// <summary>
    /// 配置Account的AutoMapper
    /// </summary>
	internal static class AccountMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Account,AccountListDto>();
            configuration.CreateMap <AccountListDto,Account>();

            configuration.CreateMap <AccountEditDto,Account>();
            configuration.CreateMap <Account,AccountEditDto>();

        }
	}
}
