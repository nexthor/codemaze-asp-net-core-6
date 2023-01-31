using Entities.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryCompanyExtensions
    {
        public static IQueryable<Company> Search(this IQueryable<Company> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return query;

            var lowerCaseTerm = searchTerm.ToLower();

            return query.Where(company => company.Name != null && company.Name.ToLower().Contains(lowerCaseTerm)!);
        }

        public static IQueryable<Company> Sort(this IQueryable<Company> companies, string? orderByQuery)
        {
            if (string.IsNullOrWhiteSpace(orderByQuery)) return companies;

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Company>(orderByQuery);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return companies.OrderBy(e => e.Name);

            return companies.OrderBy(orderQuery);
        }
    }
}
