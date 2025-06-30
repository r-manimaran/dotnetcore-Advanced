namespace WebApi.Models;

public class InsurancePolicy
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public float SumAssured { get; set; } 
    public string PolicyNumber { get; set; }
    public string PolicyHolder { get; set; }
    public string PreviousCustomerName { get; set; }
}
