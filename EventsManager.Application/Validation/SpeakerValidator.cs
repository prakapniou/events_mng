namespace EventsManager.Application.Validation;

public sealed class SpeakerValidator:AbstractValidator<SpeakerDTO>
{
    public SpeakerValidator()
    {
        RuleFor(_ => _.Name).NotEmpty().NotNull();
    }
}
