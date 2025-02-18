namespace Identity.API.Features.Roles.v1.RemoveBulk;

public record RemoveBulkRequest(List<string> UserIds, string RoleName);
