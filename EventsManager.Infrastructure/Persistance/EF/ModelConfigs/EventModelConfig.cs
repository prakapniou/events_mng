namespace EventsManager.Infrastructure.Persistance.EF.ModelConfigs;

internal sealed class EventModelConfig : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .HasMany(_ => _.Speakers)
            .WithMany();

        builder
            .HasMany(_ => _.Sponsors)
            .WithMany();

        builder
            .Property(_ => _.Name)
            .IsRequired();

        builder
            .Property(_ => _.Topic)
            .IsRequired();

        builder
            .Property(_ => _.Description)
            .IsRequired();

        builder
            .Property(_ => _.Schedule)
            .IsRequired();

        builder
            .Property(_ => _.Address)
            .IsRequired();

        builder
            .Property(_ => _.Spending)
            .IsRequired();
    }
}
