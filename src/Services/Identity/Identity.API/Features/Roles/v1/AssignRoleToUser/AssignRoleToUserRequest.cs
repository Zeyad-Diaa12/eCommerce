namespace Identity.API.Features.Roles.v1.AssignRoleToUser;

public record AssignRoleToUserRequest(string UserId, string RoleName);