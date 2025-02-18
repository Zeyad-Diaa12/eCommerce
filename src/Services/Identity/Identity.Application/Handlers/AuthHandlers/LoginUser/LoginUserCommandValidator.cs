namespace Identity.Application.Handlers.AuthHandlers.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        RuleFor(x => x)
            .Must((request) =>
            {
                return !string.IsNullOrEmpty(request.Username) || !string.IsNullOrEmpty(request.Email) || !string.IsNullOrEmpty(request.PhoneNumber);
            })
            .WithMessage("Provide Username, Email or Phone Number to LogIn");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Not a valid Email address")
            .MustAsync(async(email, ct) =>
            {
                var user = await userManager.FindByEmailAsync(email);
                return user != null;
            })
            .WithMessage("User does not exist")
            .When(x => !string.IsNullOrEmpty(x.Email), ApplyConditionTo.AllValidators);

        RuleFor(x => x.Username)
            .MustAsync(async (username, ct) =>
            {
                var user = await userManager.FindByNameAsync(username);
                return user != null;
            })
            .WithMessage("User does not exist")
            .When(x => !string.IsNullOrEmpty(x.Username), ApplyConditionTo.AllValidators);

        RuleFor(x => x.PhoneNumber)
            .IsValidPhoneNumber()
            .WithMessage("Not a valid Phone Number")
            .Must((phoneNumber) =>
            {
                var user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
                return user != null;
            })
            .WithMessage("User does not exist")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber), ApplyConditionTo.AllValidators);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}
