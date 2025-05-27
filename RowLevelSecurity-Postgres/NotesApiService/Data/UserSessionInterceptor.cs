using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Diagnostics;
using System.Security.Claims;

namespace NotesApiService.Data;

public class UserSessionInterceptor(IHttpContextAccessor httpContextAccessor): DbConnectionInterceptor
{
    public override async Task ConnectionOpenedAsync(
                    DbConnection connection, 
                    ConnectionEndEventData eventData, 
                    CancellationToken cancellationToken = default)
    {
        // Start a trace activity
        using var activity = new Activity("SetUserSessionOnConnection");
        activity.Start();

        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
           // ?? throw new InvalidOperationException("User session not found.");

        if (!string.IsNullOrWhiteSpace(userId))
        {
            using var command = connection.CreateCommand();

            command.CommandText = $"SET app.app_user = '{userId}'";

            command.CommandType = System.Data.CommandType.Text;

            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            // Add userId as a tag to the activity for observability
            activity?.AddTag("db.user_id", userId);
        }
        activity?.Stop();

        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

}
