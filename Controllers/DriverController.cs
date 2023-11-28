using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulatedDataGenerator.Data;
using SimulatedDataGenerator.Models;


namespace SimulatedDataGenerator.Controllers;

[ApiController]
[Route("[controller]")]
public class DiverController : ControllerBase
{

    private readonly ApiDBContext _context;
    private readonly ILogger<DiverController> _logger;

    public DiverController(ILogger<DiverController> logger,ApiDBContext context)
    {
        _logger = logger;
        _context=context;
    }
    [HttpGet("GetDataSimulator")]
    public async Task<IActionResult> Get()
    {        
        var result = await StartDataGeneration();
        foreach (var driverFile in result)
        {
            _context.Add(driverFile);
        }
        await _context.SaveChangesAsync();
        var allDriverFiles= await _context.SetDriverFiles.ToListAsync();
        return Ok(allDriverFiles);
    } 

    static async Task<List<DriverFile>> StartDataGeneration()
    {
        List<DriverFile> simulatedData = new List<DriverFile>();
        var count=1;
        while (count<11)
        {
            // Generate data for 1 driver
            simulatedData = GenerateSimulatedData(count);
            count++;
           
        }
        return simulatedData;
    } 

    private static List<DriverFile> GenerateSimulatedData(int numberOfDrivers)
    {
        List<DriverFile> result = new List<DriverFile>();
        Random random = new Random();

        for (int i = 1; i <= numberOfDrivers; i++)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";            
            int num = random.Next(0, chars.Length);
            string driverID = $"Driver {chars[num]}";

            // Simulate multiple files for the same driver
            for (int fileNumber = 1; fileNumber <= random.Next(1, 4); fileNumber++)
            {
                DateTime currentDate = DateTime.Now;

                // Simulate activities for a day
                for (int j = 0; j < 10; j++) // Adjust the number of activities as needed
                {
                    DateTime startTime = currentDate.AddHours(random.Next(8, 18));
                    DateTime endTime = startTime.AddMinutes(random.Next(30, 240));

                    // Check for violations
                    if (j % 2 == 0 && (endTime - startTime).TotalHours > 4)
                    {
                        endTime = startTime.AddHours(4);
                    }

                    if (j % 2 != 0 && (endTime - startTime).TotalMinutes < 45)
                    {
                        endTime = startTime.AddMinutes(45);
                    }

                    // Create a record
                    DriverFile record = new DriverFile
                    {
                        Timestamp = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        DriverID = driverID,
                        Activity = j % 2 == 0 ? "Driving" : "Rest"
                    };

                    result.Add(record);
                }
            }
        }

        return result;
    }
}
