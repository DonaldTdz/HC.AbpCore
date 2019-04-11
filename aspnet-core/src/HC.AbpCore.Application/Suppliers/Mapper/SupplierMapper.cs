
using AutoMapper;
using HC.AbpCore.Suppliers;
using HC.AbpCore.Suppliers.Dtos;

namespace HC.AbpCore.Suppliers.Mapper
{

	/// <summary>
    /// 配置Supplier的AutoMapper
    /// </summary>
	internal static class SupplierMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Supplier,SupplierListDto>();
            configuration.CreateMap <SupplierListDto,Supplier>();

            configuration.CreateMap <SupplierEditDto,Supplier>();
            configuration.CreateMap <Supplier,SupplierEditDto>();

        }
	}
}
