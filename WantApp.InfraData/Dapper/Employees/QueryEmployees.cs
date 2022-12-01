using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WantApp.Domain.DTOs.Employee;

namespace WantApp.InfraData.Dapper.Employees;

public class QueryEmployees
{
    private readonly IConfiguration _configuration;

    public QueryEmployees(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int? page, int? rows)
    {
        var db = new SqlConnection(_configuration["DefaultConnection"]);
        var query = @"
                select Email, ClaimValue as Name
                from AspNetUsers u inner join
                AspNetUserClaims c
                on u.id = c.UserId and claimType = 'Name'
                order by name
                offset (@page -1) * @rows ROWS FETCH NEXT @rows ROWS ONLY
            ";

        return db.Query<EmployeeResponse>(query, new { page, rows });
    }
}
