namespace AuditTrail.EFCore.Demo.Models
{
    public class Audit
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? Affectedcolumns { get; set; }
        public string PrimaryKey { get; set; }
    }
}
