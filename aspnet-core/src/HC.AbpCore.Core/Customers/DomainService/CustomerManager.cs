

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

using HC.AbpCore;
using HC.AbpCore.Customers;
using HC.AbpCore.Common;

namespace HC.AbpCore.Customers.DomainService
{
    /// <summary>
    /// Customer领域层的业务管理
    ///</summary>
    public class CustomerManager :AbpCoreDomainServiceBase, ICustomerManager
    {
		
		private readonly IRepository<Customer,int> _repository;

		/// <summary>
		/// Customer的构造方法
		///</summary>
		public CustomerManager(
			IRepository<Customer, int> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitCustomer()
		{
			throw new NotImplementedException();
		}

        // TODO:编写领域业务代码
        //public async Task<ResultCode> UpdateContactById(int id, string Contact)
        //{
        //    var customer = await _repository.GetAsync(id);
        //    if (customer != null && customer.Contact != Contact)
        //    {
        //        customer.Contact = Contact;
        //        await _repository.UpdateAsync(customer);
        //        return new ResultCode() { Code = 1, Msg = "成功" };
        //    }
        //    else
        //    {
        //        return new ResultCode() { Code = 0, Msg = "当前客户不存在" };
        //    }
        //}

    }
}
