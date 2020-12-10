namespace Db.TimeTrack.DbModel
{
	using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

	public partial class lkuTaskStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public lkuTaskStatu()
        {
            TaskItems = new HashSet<TaskItem>();
        }

        [StringLength(3)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
    }
}
