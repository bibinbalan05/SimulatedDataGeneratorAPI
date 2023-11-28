using Microsoft.EntityFrameworkCore;
using SimulatedDataGenerator.Models;

namespace SimulatedDataGenerator.Data;

public class ApiDBContext : DbContext
{
  public ApiDBContext(DbContextOptions<ApiDBContext>options)  
  :base(options)
  {

  }
  public DbSet<DriverFile> SetDriverFiles{get;set;}
}