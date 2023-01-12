using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context) : base(context) { }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges) => FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        public Company GetCompany(Guid companyId, bool trackChanges)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void CreateCompany(Company company) => Create(company);
    }
}
