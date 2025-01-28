using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Mappers;

using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.Core.Domain.Administration;

public class EmployeeMapper
{
    public static Employee MapFromModel(CreateOrEditEmployeeRequest model, IEnumerable<Role> roles, Employee employee = null)
    {
        if (null == employee)
        {
            employee = new();
            employee.Id = Guid.NewGuid();
        }

        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;
        employee.Email = model.Email;
        employee.Roles = roles.Select(x => new Role { Id = x.Id, Name = x.Name, Description = x.Description }).ToList();
        employee.AppliedPromocodesCount = model.AppliedPromocodesCount;
        return employee;
    }
}
