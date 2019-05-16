

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.Messages;

namespace HC.AbpCore.Messages.Dtos
{
    public class GetMessageForEditOutput
    {

        public MessageEditDto Message { get; set; }

    }
}