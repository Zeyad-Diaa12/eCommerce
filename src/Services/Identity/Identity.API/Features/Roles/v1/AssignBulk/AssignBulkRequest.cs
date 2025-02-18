namespace Identity.API.Features.Roles.v1.AssignBulk;

public record AssignBulkRequest(List<string> UserIds, string RoleName);
