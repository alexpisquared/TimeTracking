namespace Db.TimeTrack.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("TaskItem")]
    public partial class TaskItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskItem()
        {
            TimePerDays = new HashSet<TimePerDay>();
            TimeIntervals = new HashSet<TimeInterval>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(160)]
        public string Name { get; set; }

        [StringLength(1600)]
        public string Description { get; set; }

        public double? EstimateInHours { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        [Required]
        [StringLength(3)]
        public string TaskStatusId { get; set; }

        public int? AsignedTo { get; set; }

        public int? AssignedBy { get; set; }

        public int? CompleteBy { get; set; }

        public virtual lkuTaskStatu lkuTaskStatu { get; set; }

        public virtual Person Person { get; set; }

        public virtual Person Person1 { get; set; }

        public virtual Person Person2 { get; set; }

        public virtual Person Person3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimePerDay> TimePerDays { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeInterval> TimeIntervals { get; set; }
    }
}
