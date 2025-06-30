using WebApi.Models;

namespace WebApi.Services;

public interface IInsuranceService
{
    List<InsurancePolicy> GetPolicies();
}