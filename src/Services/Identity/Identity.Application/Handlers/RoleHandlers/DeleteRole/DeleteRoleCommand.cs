namespace Identity.Application.Handlers.RoleHandlers.DeleteRole;

public record DeleteRoleCommand(string RoleName) : ICommand<DeleteRoleResult>;
