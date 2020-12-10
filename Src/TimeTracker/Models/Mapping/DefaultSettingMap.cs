using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TimeTracker.Models.Mapping
{
    public class DefaultSettingMap : EntityTypeConfiguration<DefaultSetting>
    {
        public DefaultSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.DefaultJobCategoryId)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.InvoiceSubFolder)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("DefaultSetting");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CurrentInvoicerId).HasColumnName("CurrentInvoicerId");
            this.Property(t => t.CurrentInvoiceeId).HasColumnName("CurrentInvoiceeId");
            this.Property(t => t.HstPercent).HasColumnName("HstPercent");
            this.Property(t => t.XmlSomething).HasColumnName("XmlSomething");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.DayStartHour).HasColumnName("DayStartHour");
            this.Property(t => t.LunchStartHour).HasColumnName("LunchStartHour");
            this.Property(t => t.DefaultTaskId).HasColumnName("DefaultTaskId");
            this.Property(t => t.DefaultJobCategoryId).HasColumnName("DefaultJobCategoryId");
            this.Property(t => t.InvoiceSubFolder).HasColumnName("InvoiceSubFolder");

            // Relationships
            this.HasRequired(t => t.Invoicee)
                .WithMany(t => t.DefaultSettings)
                .HasForeignKey(d => d.CurrentInvoiceeId);
            this.HasRequired(t => t.Invoicer)
                .WithMany(t => t.DefaultSettings)
                .HasForeignKey(d => d.CurrentInvoicerId);

        }
    }
}
