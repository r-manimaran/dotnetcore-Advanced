namespace MinimalApiFilters.Models
{
    public class Customer
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Usesrname { get; init; } = default!;
        public string FullName { get; init; } = default!;
        public string Email { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
    }
}
