using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TimeTracker.Models.Mapping
{
    public class InvoicerMap : EntityTypeConfiguration<Invoicer>
    {
        public InvoicerMap()
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

            // Table & Column Mappings
            this.ToTable("Invoicer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.AddressDetails).HasColumnName("AddressDetails");
            this.Property(t => t.EmailAddress).HasColumnName("EmailAddress");
        }
    }
}
