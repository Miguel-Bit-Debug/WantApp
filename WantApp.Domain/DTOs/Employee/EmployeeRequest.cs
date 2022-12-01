namespace WantApp.Domain.DTOs.Employee;

public record EmployeeRequest(string Email, string Password, string Name, string EmployeeCode);