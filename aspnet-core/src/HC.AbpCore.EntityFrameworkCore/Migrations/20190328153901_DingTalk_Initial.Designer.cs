using HC.AbpCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Migrations
{
    [DbContext(typeof(AbpCoreDbContext))]
    [Migration("20190328153901_DingTalk_Initial")]
    public partial class DingTalk_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {

        }
    }
}
