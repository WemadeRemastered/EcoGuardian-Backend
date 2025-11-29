
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EcoGuardian_Backend.CRM.Domain.Model.Aggregates;

namespace EcoGuardian_Backend.Shared.Infrastructure.Persistence.EFC.Configuration.Builders
{
    public class AnswerConfigurationBuilder : IEntityTypeConfiguration<Answer>
    {

        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Content)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.HasOne<Question>()
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}