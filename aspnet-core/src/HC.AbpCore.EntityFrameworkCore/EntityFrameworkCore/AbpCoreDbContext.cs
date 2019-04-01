using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using HC.AbpCore.Authorization.Roles;
using HC.AbpCore.Authorization.Users;
using HC.AbpCore.MultiTenancy;
using HC.AbpCore.Wechat.Messages;
using HC.AbpCore.Wechat.Subscribes;
using HC.AbpCore.Wechat.Users;
using HC.AbpCore.DingTalk.DingTalkConfigs;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.DingTalk.Organizations;
using HC.AbpCore.EssentialData.Customers;

namespace HC.AbpCore.EntityFrameworkCore
{
    public class AbpCoreDbContext : AbpZeroDbContext<Tenant, Role, User, AbpCoreDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public AbpCoreDbContext(DbContextOptions<AbpCoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WechatMessage> WechatMessages { get; set; }

        public virtual DbSet<WechatSubscribe> WechatSubscribes { get; set; }

        public virtual DbSet<WechatUser> WechatUsers { get; set; }

        public virtual DbSet<DingTalkConfig> DingTalkConfigs { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
    }
}
