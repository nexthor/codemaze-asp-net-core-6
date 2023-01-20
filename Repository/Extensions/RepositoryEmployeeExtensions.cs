using Entities.Models;
using Shared.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> query, uint minAge, uint maxAge)
                                        => query.Where(e => (e.Age >= minAge && e.Age <= maxAge));

        public static IQueryable<Employee> Search(this IQueryable<Employee> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return query;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return query.Where(e => e.Name != null && e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string? orderByQuery)
        {
            if (string.IsNullOrWhiteSpace(orderByQuery)) return employees;

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQuery);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Name);

            return employees.OrderBy(orderQuery);
        }
    }
}
