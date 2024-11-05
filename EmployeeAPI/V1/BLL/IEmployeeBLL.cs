using EmployeeAPI.V1.Models.Request;
using EmployeeAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.V1.BLL
{
    public interface IEmployeeBLL
    {
        EmployeeResponse getEmployeeDetails(EmployeeRequest request);
        EmployeeBranchResponse getEmployeeBranch(EmployeeBranchRequest request);
    }
}
