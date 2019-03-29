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
    [Migration("20190312151201_Wechat_Initial")]
    public partial class Wechat_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {

        }
    }
}
