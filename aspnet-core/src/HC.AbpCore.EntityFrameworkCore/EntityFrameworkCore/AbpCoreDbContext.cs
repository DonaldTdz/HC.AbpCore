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
using HC.AbpCore.Companys.Accounts;
using HC.AbpCore.Companys;
using HC.AbpCore.Customers;
using HC.AbpCore.Products;
using HC.AbpCore.Suppliers;
using HC.AbpCore.DataDictionarys;
using HC.AbpCore.Projects;
using HC.AbpCore.Projects.ProjectDetails;

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

        public virtual DbSet<Company> Companys { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<DataDictionary> DataDictionaries { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }
    }
}
