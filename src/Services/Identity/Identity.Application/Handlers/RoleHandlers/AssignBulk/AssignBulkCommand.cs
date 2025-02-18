namespace Identity.Application.Handlers.RoleHandlers.AssignBulk;

public record AssignBulkCommand(List<string> UserIds, string RoleName) : ICommand<AssignBulkResult>;
