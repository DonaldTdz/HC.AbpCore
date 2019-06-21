

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
using HC.AbpCore.Products;
using Abp.AutoMapper;

namespace HC.AbpCore.Projects.ProjectDetails.DomainService
{
    /// <summary>
    /// ProjectDetail领域层的业务管理
    ///</summary>
    public class ProjectDetailManager : AbpCoreDomainServiceBase, IProjectDetailManager
    {

        private readonly IRepository<ProjectDetail, Guid> _repository;
        private readonly IRepository<Product, int> _productRepository;

        /// <summary>
        /// ProjectDetail的构造方法
        ///</summary>
        public ProjectDetailManager(
            IRepository<ProjectDetail, Guid> repository,
            IRepository<Product, int> productRepository
        )
        {
            _repository = repository;
            _productRepository = productRepository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitProjectDetail()
        {
            throw new NotImplementedException();
        }


        // TODO:编写领域业务代码


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="projectDetail"></param>
        /// <returns></returns>
        //public async Task CreateAsync(List<ProjectDetail> projectDetails)
        //{
        //    foreach (var item in projectDetails)
        //    {

        //    }
        //}


        //public async Task<ProjectDetail> UpdateAsync(ProjectDetail projectDetail)
        //{
        //    //TODO:更新前的逻辑判断，是否允许更新
        //    if (projectDetail.ProductId.HasValue)
        //    {
        //        var product = await _productRepository.GetAsync(projectDetail.ProductId.Value);
        //        if (product.Unit != projectDetail.Unit)
        //        {
        //            product.Unit = projectDetail.Unit;
        //            await _productRepository.UpdateAsync(product);
        //        }
        //    }

        //    var entity = await _repository.GetAsync(projectDetail.Id);
        //    entity.Name = projectDetail.Name;
        //    entity.Num = projectDetail.Num;
        //    entity.Price = projectDetail.Price;
        //    entity.ProductId = projectDetail.ProductId;
        //    entity.Specification = projectDetail.Specification;
        //    entity.Unit = projectDetail.Unit;
        //    // ObjectMapper.Map(input, entity);
        //    projectDetail = await _repository.UpdateAsync(entity);
        //    return projectDetail;
        //}


    }
}
