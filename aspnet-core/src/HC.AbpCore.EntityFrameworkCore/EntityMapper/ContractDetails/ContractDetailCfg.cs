

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HC.AbpCore.Contracts.ContractDetails;

namespace HC.AbpCore.EntityMapper.ContractDetails
{
    public class ContractDetailCfg : IEntityTypeConfiguration<ContractDetail>
    {
        public void Configure(EntityTypeBuilder<ContractDetail> builder)
        {

            builder.ToTable("ContractDetails", YoYoAbpefCoreConsts.SchemaNames.CMS);

            
			builder.Property(a => a.ContractId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.RefDetailId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.DeliveryDate).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


