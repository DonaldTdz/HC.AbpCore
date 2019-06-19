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
using HC.AbpCore.Tenders;
using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Contracts.ContractDetails;
using HC.AbpCore.Contracts;
using HC.AbpCore.Invoices;
using HC.AbpCore.Invoices.InvoiceDetails;
using HC.AbpCore.Reimburses;
using HC.AbpCore.Reimburses.ReimburseDetails;
using HC.AbpCore.TimeSheets;
using HC.AbpCore.PaymentPlans;
using HC.AbpCore.Messages;
using System.Threading.Tasks;
using HC.AbpCore.Tasks;
using HC.AbpCore.Customers.CustomerContacts;

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

        public virtual DbSet<Tender> Tenders { get; set; }

        public virtual DbSet<Purchase> Purchases { get; set; }

        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }

        public virtual DbSet<ContractDetail> ContractDetails { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public virtual DbSet<Reimburse> Reimburses { get; set; }

        public virtual DbSet<ReimburseDetail> ReimburseDetails { get; set; }

        public virtual DbSet<TimeSheet> TimeSheets { get; set; }

        public virtual DbSet<PaymentPlan> PaymentPlans { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<CompletedTask> CompletedTasks { get; set; }

        public virtual DbSet<CustomerContact> CustomerContacts { get; set; }
    }
}
