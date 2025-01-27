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
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            //
            Console.WriteLine($"GetEmployeeByIdAsync 0001 {id}");
            //
            var employee = await _employeeRepository.GetByIdAsync(id);
            //
            Console.WriteLine($"GetEmployeeByIdAsync 0002 {employee.Id}, {employee.FirstName}, {employee.LastName}, {employee.Email}");
            //

            if (employee == null)
                return NotFound();
            //
            Console.WriteLine($"GetEmployeeByIdAsync 0003 +");
            //

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
            //
            Console.WriteLine($"GetEmployeeByIdAsync 0004 {employeeModel.Id}, {employeeModel.FullName}, {employeeModel.Email}");
            //

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
            var roles = await _roleRepository.GetRangeByIdsAsync(request.Roles.Select(x => x.Id).ToList());
            // TODO: mapper
            // Employee employee = new() { Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"), FirstName = request.FirstName, LastName = request.LastName, Email = request.Email, Roles = roles.ToList() };
            var employee = EmployeeMapper.MapFromModel(request, roles);
            Console.WriteLine($"id={employee.Id}, first={employee.FirstName}, last={employee.LastName}, email={employee.Email}");
            //, Roles = request.Roles.Select(role => new Role() { Name = role.Name, Description = role.Description }) };
            // Console.WriteLine("CreateEmployeeAsync 0006");
            await _employeeRepository.AddAsync(employee);
            Console.WriteLine("CreateEmployeeAsync 0007");
            Console.WriteLine($"nameof(GetEmployeeByIdAsync) = {nameof(GetEmployeeByIdAsync)}");
            Console.WriteLine($"employee.Id = {employee.Id}");
            Console.WriteLine($"new id = employee.Id = {new { id = employee.Id }}");
            //
            try
            {
                Console.WriteLine($"GetByIdAsync(employee.Id); {employee.Id}");
                (_employeeRepository as InMemoryRepository<Employee>).Data.ToList().ForEach(d => Console.WriteLine($"data item: {d.Id}, {d.Email}"));
                var saved = await _employeeRepository.GetByIdAsync(employee.Id);
                Console.WriteLine($"before?");
                Console.WriteLine($"null == saved {null == saved}");
                if (null != saved)
                {
                    Console.WriteLine($"saved.Id {saved.Id}");
                    Console.WriteLine($"saved.FullName {saved.FullName}");
                    Console.WriteLine($"saved.Email {saved.Email}");
                    Console.WriteLine($"CreateEmployeeAsync -> GetEmployeeByIdAsync: {saved.Id}, {saved.FullName}, {saved.Email}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}; {e.StackTrace}");
            }
            Console.WriteLine("probably, before the failure !!!!!!!!!!!!!!!!!!!!!");
            //
            return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = employee.Id }, employee.Id);
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
        [HttpDelete("{id:guid}")]
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