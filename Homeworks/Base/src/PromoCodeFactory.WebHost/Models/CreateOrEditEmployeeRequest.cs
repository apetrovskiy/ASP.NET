using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Models;

public class CreateOrEditEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<RoleItemResponse> Roles { get; set; }
    public int AppliedPromocodesCount { get; set; }
}
