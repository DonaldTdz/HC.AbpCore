

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
using HC.AbpCore.Reimburses.ReimburseDetails;
using Abp.AutoMapper;

namespace HC.AbpCore.Reimburses.ReimburseDetails.DomainService
{
    /// <summary>
    /// ReimburseDetail领域层的业务管理
    ///</summary>
    public class ReimburseDetailManager : AbpCoreDomainServiceBase, IReimburseDetailManager
    {

        private readonly IRepository<ReimburseDetail, Guid> _repository;
        private readonly IRepository<Reimburse, Guid> _reimburseRepository;

        /// <summary>
        /// ReimburseDetail的构造方法
        ///</summary>
        public ReimburseDetailManager(
            IRepository<ReimburseDetail, Guid> repository,
            IRepository<Reimburse, Guid> reimburseRepository
        )
        {
            _reimburseRepository = reimburseRepository;
            _repository = repository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitReimburseDetail()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码



        public async Task<decimal> Create(ReimburseDetail reimburseDetail)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var reimburse = await _reimburseRepository.GetAsync(reimburseDetail.ReimburseId);
            reimburse.Amount += reimburseDetail.Amount;
            await _reimburseRepository.UpdateAsync(reimburse);

            await _repository.InsertAsync(reimburseDetail);
            return reimburse.Amount ?? 0;
        }


        public async Task<decimal> Update(ReimburseDetail reimburseDetail)
        {
            var entity = await _repository.GetAsync(reimburseDetail.Id);
            var reimburse = await _reimburseRepository.GetAsync(reimburseDetail.ReimburseId);

            reimburse.Amount = reimburse.Amount - entity.Amount + reimburseDetail.Amount;
            await _reimburseRepository.UpdateAsync(reimburse);
            await _repository.UpdateAsync(reimburseDetail);
            return reimburse.Amount ?? 0;
        }

        public async Task<decimal> Delete(Guid id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            var entity = await _repository.GetAsync(id);
            var reimburse = await _reimburseRepository.GetAsync(entity.ReimburseId);
            reimburse.Amount -= entity.Amount;
            await _reimburseRepository.UpdateAsync(reimburse);

            await _repository.DeleteAsync(id);

            return reimburse.Amount??0;
        }
    }
}
