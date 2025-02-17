using BuildingBlocks.CQRS.Command;

namespace Identity.Application.Handlers.UserHandlers.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password,
    string Username,
    string PhoneNumber
) : ICommand<LoginUserResult>;
