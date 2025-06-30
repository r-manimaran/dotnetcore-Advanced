using WebApi.Models;

namespace WebApi.Services;

public class InsuranceService : IInsuranceService
{
    public List<InsurancePolicy> GetPolicies() => [
            new InsurancePolicy()
            {
                CustomerName = "Customer 1",
                Id = 1,
                SumAssured = 5000.02f
            },
            new InsurancePolicy()
            {
                CustomerName = "Customer 2",
                Id = 2,
                SumAssured = 6500.00f
            }
        ];
}
