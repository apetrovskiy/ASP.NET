using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    using PromoCodeFactory.WebHost.Mappers;
    using PromoCodeFactory.DataAccess.Repositories;

    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _roleRepository;

        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync([FromRoute] Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);


            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    // why is this hidden?
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }




        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <returns>EmployeeResponse</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync(CreateOrEditEmployeeRequest request)
        {
            List<string> requestRoleNames = [.. request.Roles.Select(r => r.Name)];
            var allRoles = await _roleRepository.GetAllAsync();
            var roles = await _roleRepository.GetRangeByIdsAsync(allRoles.ToList().Where(r => requestRoleNames.Contains(r.Name)).Select(r => r.Id).ToList());
            var employee = EmployeeMapper.MapFromModel(model: request, roles: roles);
            await _employeeRepository.AddAsync(employee);
            return CreatedAtAction("GetEmployeeById", new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Обновить работника
        /// </summary>
        /// <param name="id">Id работника, например <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
        /// <param name="request">Данные запроса></param>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> EditEmployeeByIdAsync([FromRoute] Guid id, [FromBody] CreateOrEditEmployeeRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            List<string> requestRoleNames = [.. request.Roles.Select(r => r.Name)];
            var allRoles = await _roleRepository.GetAllAsync();
            var roles = await _roleRepository.GetRangeByIdsAsync(allRoles.ToList().Where(r => requestRoleNames.Contains(r.Name)).Select(r => r.Id).ToList());
            employee = EmployeeMapper.MapFromModel(model: request, roles: roles, employee: employee);
            await _employeeRepository.UpdateAsync(employee);
            return CreatedAtAction("GetEmployeeById", new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Удалить работника
        /// </summary>
        /// <param name="id">Id работника, например <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (null == employee) return NotFound();
            await _employeeRepository.DeleteAsync(employee);
            return NoContent();
        }
    }
}