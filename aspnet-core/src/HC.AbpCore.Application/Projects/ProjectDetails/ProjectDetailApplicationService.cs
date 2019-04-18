
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using HC.AbpCore.Projects.ProjectDetails;
using HC.AbpCore.Projects.ProjectDetails.Dtos;
using HC.AbpCore.Projects.ProjectDetails.DomainService;
using HC.AbpCore.Products;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Projects.ProjectDetails
{
    /// <summary>
    /// ProjectDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ProjectDetailAppService : AbpCoreAppServiceBase, IProjectDetailAppService
    {
        private readonly IRepository<ProjectDetail, Guid> _entityRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IProjectDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProjectDetailAppService(
        IRepository<ProjectDetail, Guid> entityRepository,
          IRepository<Product, int> productRepository
        , IProjectDetailManager entityManager
        )
        {
            _productRepository = productRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取ProjectDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<ProjectDetailListDto>> GetPagedAsync(GetProjectDetailsInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.CreationTime)
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<ProjectDetailListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<ProjectDetailListDto>>();

            return new PagedResultDto<ProjectDetailListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取ProjectDetailListDto信息
        /// </summary>

        public async Task<ProjectDetailListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<ProjectDetailListDto>();
        }

        /// <summary>
        /// 获取编辑 ProjectDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetProjectDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetProjectDetailForEditOutput();
            ProjectDetailEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<ProjectDetailEditDto>();

                //projectDetailEditDto = ObjectMapper.Map<List<projectDetailEditDto>>(entity);
            }
            else
            {
                editDto = new ProjectDetailEditDto();
            }

            output.ProjectDetail = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改ProjectDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdateAsync(CreateOrUpdateProjectDetailInput input)
        {

            if (input.ProjectDetail.Id.HasValue)
            {
                await UpdateAsync(input.ProjectDetail);
            }
            else
            {
                await CreateAsync(input.ProjectDetail);
            }
        }


        /// <summary>
        /// 新增ProjectDetail
        /// </summary>

        protected virtual async Task<ProjectDetailEditDto> CreateAsync(ProjectDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <ProjectDetail>(input);
            if (!input.ProductId.HasValue)
            {
                var product = await _productRepository.GetAll().Where(aa => aa.Name == input.Name && aa.Specification == input.Specification && aa.Unit == input.Unit && aa.IsEnabled == true).FirstOrDefaultAsync();
                if (product == null)
                    input.ProductId = await _productRepository.InsertAndGetIdAsync(new Product() { Name = input.Name, Specification = input.Specification, Unit = input.Unit, Type = 0, IsEnabled = true });
                else
                {
                    input.ProductId = product.Id;
                }
            }

            var entity = input.MapTo<ProjectDetail>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<ProjectDetailEditDto>();
        }

        /// <summary>
        /// 编辑ProjectDetail
        /// </summary>

        protected virtual async Task UpdateAsync(ProjectDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            if (!input.ProductId.HasValue)
            {
                var product = await _productRepository.GetAll().Where(aa => aa.Name == input.Name && aa.Specification == input.Specification && aa.Unit == input.Unit && aa.IsEnabled == true).FirstOrDefaultAsync();
                if (product == null)
                    input.ProductId = await _productRepository.InsertAndGetIdAsync(new Product() { Name = input.Name, Specification = input.Specification, Unit = input.Unit, Type = 0, IsEnabled = true });
                else
                {
                    input.ProductId = product.Id;
                }
            }

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除ProjectDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除ProjectDetail的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        public async Task<List<DropDownDto>> GetDropDownsByprojectIdAsync(int projectId)
        {
            var query = _entityRepository.GetAll();
            var entityList = await query
                    .OrderBy(a => a.CreationTime).AsNoTracking()
                    .Select(c => new DropDownDto() { Text = c.Name, Value = c.Id.ToString() })
                    .ToListAsync();
            return entityList;
        }

        /// <summary>
        /// 导出ProjectDetail为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

    }
}

