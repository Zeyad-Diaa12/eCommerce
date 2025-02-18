namespace Identity.Application.Handlers.RoleHandlers.AssignRoleToUser;

public record AssignRoleToUserCommand(string UserId, string RoleName) : ICommand<AssignRoleToUserResult>;
