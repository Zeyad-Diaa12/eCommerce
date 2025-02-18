namespace Identity.API.Features.Roles.v1.RemoveRoleFromUser;

public record RemoveRoleFromUserRequest(string UserId, string RoleName);
