﻿
using Entities.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions;

public static class WorkerExtensions
{
    public static IQueryable<Worker> Sort(this IQueryable<Worker> workers, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return workers.OrderBy(e => e.Name);
        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(Worker).GetProperties(BindingFlags.Public |
       BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();
        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;
            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi =>
           pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
                continue;
            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
        }
        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        if (string.IsNullOrWhiteSpace(orderQuery))
            return workers.OrderBy(e => e.Name);
        return workers.OrderBy(orderQuery);

    }
}
