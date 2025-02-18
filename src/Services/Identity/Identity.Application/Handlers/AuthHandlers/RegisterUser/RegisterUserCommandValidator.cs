namespace Identity.Application.Handlers.UserHandlers.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .MustAsync(
                async(email, CancellationToken) =>
                {
                    var user = await userManager.FindByEmailAsync(email);
                    return user == null;
                }
            ).WithMessage("Email already exists");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.Password)
            .CustomAsync(async(password, context, CancellationToken) =>
            {
                var user = new User
                {
                    UserName = context.InstanceToValidate.Email,
                    Email = context.InstanceToValidate.Email
                };

                foreach (var validator in userManager.PasswordValidators)
                {
                    var result = await validator.ValidateAsync(userManager, user, password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            context.AddFailure(nameof(context.InstanceToValidate.Password), error.Description);
                        }
                    }
                }
            });

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Re-Enter the password")
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(6)
            .WithMessage("Username must be at least 6 characters long")
            .MustAsync(async(username, CancellationToken) =>
            {
                var user = await userManager.FindByNameAsync(username);
                return user == null;
            }).WithMessage("Username already exists");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters long");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters long");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .IsValidPhoneNumber()
            .Must((phoneNumber) =>
            {
                var user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
                return user == null;
            }).WithMessage("Phone Number already exists");
    }
}
