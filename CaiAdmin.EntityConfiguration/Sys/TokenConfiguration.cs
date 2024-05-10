using CaiAdmin.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaiAdmin.EntityConfiguration.Sys
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            //builder.HasKey(e => e.AccessToken);
        }
    }
}
