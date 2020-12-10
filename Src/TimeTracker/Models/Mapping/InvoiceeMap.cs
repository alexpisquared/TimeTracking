using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TimeTracker.Models.Mapping
{
    public class InvoiceeMap : EntityTypeConfiguration<Invoicee>
    {
        public InvoiceeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.AddressDetails)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.EmailAddress)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.EmailBody)
                .IsRequired();

            this.Property(t => t.PayPeriodMode)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Notes)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Invoicee");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.AddressDetails).HasColumnName("AddressDetails");
            this.Property(t => t.EmailAddress).HasColumnName("EmailAddress");
            this.Property(t => t.EmailBody).HasColumnName("EmailBody");
            this.Property(t => t.CorpRate).HasColumnName("CorpRate");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.LasttDate).HasColumnName("LasttDate");
            this.Property(t => t.PayPeriodMode).HasColumnName("PayPeriodMode");
            this.Property(t => t.PayPeriodStart).HasColumnName("PayPeriodStart");
            this.Property(t => t.PayPeriodLength).HasColumnName("PayPeriodLength");
            this.Property(t => t.Notes).HasColumnName("Notes");

            // Relationships
            this.HasRequired(t => t.lkuPayPeriodMode)
                .WithMany(t => t.Invoicees)
                .HasForeignKey(d => d.PayPeriodMode);

        }
    }
}
