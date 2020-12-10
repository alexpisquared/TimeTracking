namespace Db.TimeTrack.DbModel
{
	using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("Invoicer")]
    public partial class Invoicer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoicer()
        {
            DefaultSettings = new HashSet<DefaultSetting>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(2000)]
        public string AddressDetails { get; set; }

        [Required]
        [StringLength(150)]
        public string FromEmail { get; set; }

        [Required]
        [StringLength(150)]
        public string CEO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DefaultSetting> DefaultSettings { get; set; }
    }
}
