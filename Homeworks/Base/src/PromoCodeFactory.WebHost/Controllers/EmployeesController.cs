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
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
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
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать сотрудника
        /// </summary>
        /// <returns>EmployeeResponse</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync(CreateOrEditEmployeeRequest request)
        {
            // //Получаем предпочтения из бд и сохраняем большой объект
            // var preferences = await _preferenceRepository
            //     .GetRangeByIdsAsync(request.PreferenceIds);

            // Customer customer = CustomerMapper.MapFromModel(request, preferences);

            // await _customerRepository.AddAsync(customer);

            // return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, customer.Id);


            // try
            // {
            // Console.WriteLine("CreateEmployeeAsync 0004");
            var roles = await _roleRepository.GetRangeByIdsAsync(request.Roles.Select(x => x.Id).ToList());
            // Console.WriteLine("CreateEmployeeAsync 0005");
            Employee employee = new() { FirstName = request.FirstName, LastName = request.LastName, Email = request.Email, Roles = roles.ToList() };
            //, Roles = request.Roles.Select(role => new Role() { Name = role.Name, Description = role.Description }) };
            // Console.WriteLine("CreateEmployeeAsync 0006");
            await _employeeRepository.AddAsync(employee);
            Console.WriteLine("CreateEmployeeAsync 0007");
            Console.WriteLine($"nameof(GetEmployeesAsync) = {nameof(GetEmployeesAsync)}");
            Console.WriteLine($"employee.Id = {employee.Id}");
            Console.WriteLine($"new id = employee.Id = {new { id = employee.Id }}");
            return CreatedAtAction(nameof(GetEmployeesAsync), new { id = employee.Id }, employee.Id);
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine($"{e} {e.Message} {e.StackTrace}");
            //     return NotFound();
            // }
        }

        /// <summary>
        /// Удалить работника
        /// </summary>
        /// <param name="id">Id работника, например <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            /*
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            await _customerRepository.DeleteAsync(customer);

            return NoContent();
            */
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (null == employee) return NotFound();
            await _employeeRepository.DeleteAsync(employee);
            return NoContent();
        }
    }
}