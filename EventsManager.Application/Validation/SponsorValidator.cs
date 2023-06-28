namespace EventsManager.Application.Validation;

public sealed class SponsorValidator:AbstractValidator<SponsorDTO>
{
    public SponsorValidator()
    {
        RuleFor(_ => _.Name).NotEmpty().NotNull();
    }
}
