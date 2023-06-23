namespace EventsManager.Application.Validation;

internal class SponsorValidator:AbstractValidator<Sponsor>
{
    public SponsorValidator()
    {
        RuleFor(_ => _.Name).NotEmpty().NotNull();
    }
}
