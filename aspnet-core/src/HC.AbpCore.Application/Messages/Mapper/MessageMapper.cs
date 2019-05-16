
using AutoMapper;
using HC.AbpCore.Messages;
using HC.AbpCore.Messages.Dtos;

namespace HC.AbpCore.Messages.Mapper
{

	/// <summary>
    /// 配置Message的AutoMapper
    /// </summary>
	internal static class MessageMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Message,MessageListDto>();
            configuration.CreateMap <MessageListDto,Message>();

            configuration.CreateMap <MessageEditDto,Message>();
            configuration.CreateMap <Message,MessageEditDto>();

        }
	}
}
