namespace Identity.Application.Handlers.RoleHandlers.GetUsersInRole;

public record GetUsersInRoleQuery(string RoleName) : IQuery<GetUsersInRoleResult>;
