using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Security.Claims;

namespace NotesApiService.Data;

public class UserSessionInterceptor(IHttpContextAccessor httpContextAccessor): DbConnectionInterceptor
{
    public override async Task ConnectionOpenedAsync(
                    DbConnection connection, 
                    ConnectionEndEventData eventData, 
                    CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
           // ?? throw new InvalidOperationException("User session not found.");

        if (!string.IsNullOrWhiteSpace(userId))
        {
            using var command = connection.CreateCommand();

            command.CommandText = $"SET app.app_user = '{userId}'";

            command.CommandType = System.Data.CommandType.Text;

            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

        }
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

}
