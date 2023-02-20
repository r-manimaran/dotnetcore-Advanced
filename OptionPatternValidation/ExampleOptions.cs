using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace OptionPatternValidation
{
    public class ExampleOptions
    {
        public const string SectionName = "Example";
        [Range(1,9)]
        public int RetryCount { get; set; }
        [Required]
        public LogLevel Level { get; set; }
    }
}
