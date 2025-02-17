using BuildingBlocks.CQRS.Command;


namespace Identity.Application.Handlers.RoleHandlers.CreateRole;

public record CreateRoleCommand(string RoleName) : ICommand<CreateRoleResult>;
