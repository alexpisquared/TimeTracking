using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TimeTracker.Models.Mapping
{
    public class InvoiceMap : EntityTypeConfiguration<Invoice>
    {
        public InvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Invoice");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PeriodFrom).HasColumnName("PeriodFrom");
            this.Property(t => t.PeriodUpTo).HasColumnName("PeriodUpTo");
            this.Property(t => t.TotalHours).HasColumnName("TotalHours");
            this.Property(t => t.InvoiceDate).HasColumnName("InvoiceDate");
            this.Property(t => t.HstPercent).HasColumnName("HstPercent");
            this.Property(t => t.RateSubmitted).HasColumnName("RateSubmitted");
            this.Property(t => t.IsSubmitted).HasColumnName("IsSubmitted");
            this.Property(t => t.HasCleared).HasColumnName("HasCleared");
            this.Property(t => t.InvoiceeId).HasColumnName("InvoiceeId");

            // Relationships
            this.HasOptional(t => t.Invoicee)
                .WithMany(t => t.Invoices)
                .HasForeignKey(d => d.InvoiceeId);

        }
    }
}
