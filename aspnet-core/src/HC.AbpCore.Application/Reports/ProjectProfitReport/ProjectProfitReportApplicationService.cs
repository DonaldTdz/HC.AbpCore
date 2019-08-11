using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HC.AbpCore.Reports.ProjectProfitReport.Dtos;

namespace HC.AbpCore.Reports.ProjectProfitReport
{
    public class ProjectProfitReportApplicationService : IProjectProfitReportApplicationService
    {
        private readonly IRepository<ProfitStatistic, Guid> _entityRepository;

        private readonly IProfitStatisticManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProfitStatisticAppService(
        IRepository<ProfitStatistic, Guid> entityRepository
        , IProfitStatisticManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }

        public Task<ProjectProfitListDto> GetProjectProfitByIdAsync(GetProjectProfitInput input)
        {
            throw new NotImplementedException();
        }
    }
}
