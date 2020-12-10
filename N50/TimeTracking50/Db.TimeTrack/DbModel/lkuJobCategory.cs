namespace Db.TimeTrack.DbModel
{
	using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("lkuJobCategory")]
    public partial class lkuJobCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public lkuJobCategory()
        {
            TimeIntervals = new HashSet<TimeInterval>();
            TimePerDays = new HashSet<TimePerDay>();
        }

        [StringLength(3)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsBillable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeInterval> TimeIntervals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimePerDay> TimePerDays { get; set; }
    }
}
