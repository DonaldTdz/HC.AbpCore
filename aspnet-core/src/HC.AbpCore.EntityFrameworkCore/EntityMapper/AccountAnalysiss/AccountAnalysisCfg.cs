

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HC.AbpCore.Reports.AccountAnalysisReport;

namespace HC.AbpCore.EntityMapper.AccountAnalysiss
{
    public class AccountAnalysisCfg : IEntityTypeConfiguration<AccountAnalysis>
    {
        public void Configure(EntityTypeBuilder<AccountAnalysis> builder)
        {

            builder.ToTable("AccountAnalysiss", YoYoAbpefCoreConsts.SchemaNames.CMS);

            
			builder.Property(a => a.Name).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.Remarks).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.Detail).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


