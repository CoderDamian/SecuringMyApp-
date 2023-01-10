using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAPI.Models;

namespace MyAPI.Mappings
{
    public class LoginModelMap : IEntityTypeConfiguration<LoginModel>
    {
        public void Configure(EntityTypeBuilder<LoginModel> builder)
        {
            builder.ToTable("LOGINMODEL", "CIA");

            builder.Property(p => p.Id)
                .HasColumnName("ID");

            builder.Property(p => p.UserName)
                .HasColumnName("USERNAME");

            builder.Property(p => p.Password)
                .HasColumnName("PASSWORD");

            builder.Property(p => p.RefreshToken)
                .HasColumnName("REFRESHTOKEN");

            builder.Property(p => p.RefreshTokenExpiryTime)
                .HasColumnName("REFRESHTOJENEXPIRYTIME");
        }
    }
}
