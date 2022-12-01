using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WantApp.Domain.DTOs.Employee;
using WantApp.InfraData.Dapper.Employees;

namespace WantApp.API.Controllers.Employees;

[Authorize(Policy = "EmployeePolicy")]
[Route("v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly QueryEmployees _queryEmployees;

    public EmployeesController(UserManager<IdentityUser> userManager,
                               QueryEmployees queryEmployees)
    {
        _userManager = userManager;
        _queryEmployees = queryEmployees;
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmployer([FromBody] EmployeeRequest request)
    {
        var user = new IdentityUser()
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", request.EmployeeCode),
            new Claim("Name", request.Name)
        };

        var claimResult = await _userManager.AddClaimsAsync(user, userClaims);

        if (!claimResult.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(user.Id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployee(int? page, int? rows)
    {
        if (page == null || rows == null)
        {
            return BadRequest("Informe a pagina e o numero de linhas.");
        }

        var employees = _queryEmployees.Execute(page, rows);
        
        return Ok(employees);
    }
}
