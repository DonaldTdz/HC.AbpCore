

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;

namespace HC.AbpCore.EntityMapper.AdvancePaymentDetails
{
    public class AdvancePaymentDetailCfg : IEntityTypeConfiguration<AdvancePaymentDetail>
    {
        public void Configure(EntityTypeBuilder<AdvancePaymentDetail> builder)
        {

            builder.ToTable("AdvancePaymentDetails", YoYoAbpefCoreConsts.SchemaNames.CMS);

            
			builder.Property(a => a.AdvancePaymentId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.PurchaseDetailId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.Ratio).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.Amount).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.CreationTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


