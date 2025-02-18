namespace Identity.Application.Handlers.RoleHandlers.RemoveBulk;

public record RemoveBulkCommand(List<string> UserIds, string RoleName) : ICommand<RemoveBulkResult>;
