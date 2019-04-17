

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
using HC.AbpCore.Projects.ProjectDetails;


namespace HC.AbpCore.Projects.ProjectDetails.DomainService
{
    /// <summary>
    /// ProjectDetail领域层的业务管理
    ///</summary>
    public class ProjectDetailManager :AbpCoreDomainServiceBase, IProjectDetailManager
    {
		
		private readonly IRepository<ProjectDetail,Guid> _repository;

		/// <summary>
		/// ProjectDetail的构造方法
		///</summary>
		public ProjectDetailManager(
			IRepository<ProjectDetail, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitProjectDetail()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
