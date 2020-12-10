namespace Db.TimeTrack.DbModel
{
	using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("Person")]
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            TaskItems = new HashSet<TaskItem>();
            TaskItems1 = new HashSet<TaskItem>();
            TaskItems2 = new HashSet<TaskItem>();
            TaskItems3 = new HashSet<TaskItem>();
            TimeIntervals = new HashSet<TimeInterval>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NameShort { get; set; }

        [StringLength(500)]
        public string NameFull { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(2500)]
        public string Notes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeInterval> TimeIntervals { get; set; }
    }
}
