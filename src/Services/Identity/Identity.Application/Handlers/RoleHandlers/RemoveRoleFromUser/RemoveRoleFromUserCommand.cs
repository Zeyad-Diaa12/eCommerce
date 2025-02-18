namespace Identity.Application.Handlers.RoleHandlers.RemoveRoleFromUser;

public record RemoveRoleFromUserCommand(string UserId, string RoleName) : ICommand<RemoveRoleFromUserResult>;
