namespace EventsManager.Application.Validation;

public sealed class SpeakerValidator:AbstractValidator<Speaker>
{
    public SpeakerValidator()
    {
        RuleFor(_ => _.Name).NotEmpty().NotNull();
    }
}
