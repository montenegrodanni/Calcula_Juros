using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecolimentoAtraso.Model.Entity;

namespace RecolimentoAtraso.Database.EntitiesConfiguration;

public class TabelaJurosConfiguration : IEntityTypeConfiguration<TabelaJuros>
{
    public void Configure(EntityTypeBuilder<TabelaJuros> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Ano).IsRequired();
        builder.Property(j => j.Mes).IsRequired();
        builder.Property(j => j.ValorJuros).IsRequired();
    }
}
