namespace MinimalApiFilters.Models
{
    public class Customer
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string Email { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
