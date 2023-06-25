namespace EventsManager.Infrastructure.EF.ModelConfigs;

internal sealed class SpeakerModelConfig : IEntityTypeConfiguration<Speaker>
{
    public void Configure(EntityTypeBuilder<Speaker> builder)
    {
        builder
           .Property(_ => _.Name)
           .IsRequired();
    }
}
