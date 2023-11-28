namespace SimulatedDataGenerator.Models;


public class DriverFile
{
    // Properties for driver file data
    public int ID {get;set;}
    public string DriverID { get; set; } = null!;
    public string Timestamp  { get; set; }= null!;
    public string Activity  { get; set; }= null!;
     
}