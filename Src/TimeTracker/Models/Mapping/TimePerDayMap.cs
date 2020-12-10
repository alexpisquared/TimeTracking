using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TimeTracker.Models.Mapping
{
    public class TimePerDayMap : EntityTypeConfiguration<TimePerDay>
    {
        public TimePerDayMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.JobCategoryId)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Note)
                .HasMaxLength(800);

            // Table & Column Mappings
            this.ToTable("TimePerDay");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WorkedOn).HasColumnName("WorkedOn");
            this.Property(t => t.WorkedHours).HasColumnName("WorkedHours");
            this.Property(t => t.HourStarted).HasColumnName("HourStarted");
            this.Property(t => t.JobCategoryId).HasColumnName("JobCategoryId");
            this.Property(t => t.TaskItemId).HasColumnName("TaskItemId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");

            // Relationships
            this.HasOptional(t => t.Invoice)
                .WithMany(t => t.TimePerDays)
                .HasForeignKey(d => d.InvoiceId);
            this.HasOptional(t => t.lkuJobCategory)
                .WithMany(t => t.TimePerDays)
                .HasForeignKey(d => d.JobCategoryId);
						//this.HasOptional(t => t.TaskItem)
						//		.WithMany(t => t.TimePerDays)
						//		.HasForeignKey(d => d.TaskItemId);

        }
    }
}
