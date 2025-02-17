using BuildingBlocks.CQRS.Command;

namespace Identity.Application.Handlers.RoleHandlers.UpdateRole;

public record UpdateRoleCommand(string NewRoleName, string OldRoleName) : ICommand<UpdateRoleResult>;
