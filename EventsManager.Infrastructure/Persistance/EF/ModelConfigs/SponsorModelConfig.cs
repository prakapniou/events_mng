namespace EventsManager.Infrastructure.Persistance.EF.ModelConfigs;

internal sealed class SponsorModelConfig : IEntityTypeConfiguration<Sponsor>
{
    public void Configure(EntityTypeBuilder<Sponsor> builder)
    {
        builder
           .Property(_ => _.Name)
           .IsRequired();
    }
}
