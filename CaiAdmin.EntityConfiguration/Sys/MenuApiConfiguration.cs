using CaiAdmin.Entity.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaiAdmin.EntityConfiguration.Sys
{
    public class MenuApiConfiguration : IEntityTypeConfiguration<MenuApi>
    {
        public void Configure(EntityTypeBuilder<MenuApi> builder)
        {
            builder.HasKey(e => new { e.ApiId, e.MenuId });
        }
    }
}
