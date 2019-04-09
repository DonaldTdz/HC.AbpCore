
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


using HC.AbpCore.Companys.Accounts;
using HC.AbpCore.Companys.Accounts.Dtos;
using HC.AbpCore.Companys.Accounts.DomainService;



namespace HC.AbpCore.Companys.Accounts
{
    /// <summary>
    /// Account应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class CompanyAccountApplicationService : AbpCoreAppServiceBase, ICompanyAccountApplicationService
    {
        private readonly IRepository<Account, long> _entityRepository;

        private readonly IAccountManager _entityManager;

        private readonly IRepository<Company, int> _companyRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public CompanyAccountApplicationService(
        IRepository<Account, long> entityRepository,
        IRepository<Company, int> companyRepository
        , IAccountManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _companyRepository = companyRepository;
             _entityManager =entityManager;
        }


        /// <summary>
        /// 获取Account的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<AccountListDto>> GetPagedAsync(GetAccountsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderByDescending(aa=>aa.CreationTime).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<AccountListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<AccountListDto>>();

			return new PagedResultDto<AccountListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取AccountListDto信息
		/// </summary>
		 
		public async Task<AccountListDto> GetByIdAsync(EntityDto<long> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<AccountListDto>();
		}

		/// <summary>
		/// 获取编辑 Account
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetAccountForEditOutput> GetForEditAsync(NullableIdDto<long> input)
		{
			var output = new GetAccountForEditOutput();
AccountEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<AccountEditDto>();

				//accountEditDto = ObjectMapper.Map<List<accountEditDto>>(entity);
			}
			else
			{
				editDto = new AccountEditDto();
			}

			output.Account = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改Account的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdateAsync(CreateOrUpdateAccountInput input)
		{

			if (input.Account.Id.HasValue)
			{
				await UpdateAsync(input.Account);
			}
			else
			{
				await CreateAsync(input.Account);
			}
		}


		/// <summary>
		/// 新增Account
		/// </summary>
		
		protected virtual async Task<AccountEditDto> CreateAsync(AccountEditDto input)
		{
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Account>(input);
            input.CreationTime = DateTime.Now;
            var entity=input.MapTo<Account>();
            var company = await _companyRepository.GetAsync(input.CompanyId);
            company.Balance = input.Ending;
            await _companyRepository.UpdateAsync(company);

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<AccountEditDto>();
		}

		/// <summary>
		/// 编辑Account
		/// </summary>
		
		protected virtual async Task UpdateAsync(AccountEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除Account信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task DeleteAsync(EntityDto<long> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除Account的方法
		/// </summary>
		
		public async Task BatchDeleteAsync(List<long> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出Account为excel表,等待开发。
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


