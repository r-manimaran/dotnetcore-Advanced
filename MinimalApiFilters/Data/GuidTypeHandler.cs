using Dapper;
using System.Data;

namespace MinimalApiFilters.Data;

public class GuidTypeHandler: SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        if (value is string stringValue)
        {
            if (Guid.TryParse(stringValue, out Guid valueGuid))
            {
                return valueGuid;
            }
        }
        else if (value is byte[] bytes)
        {
            return new Guid(bytes);
        }

        throw new ArgumentException($"unable to convert {value} to Guid.", nameof(value));
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }
}
