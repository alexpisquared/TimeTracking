namespace Db.TimeTrack.DbModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("TaskCategory")]
    public partial class TaskCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        [StringLength(128)]
        public string CreatedBy { get; set; }
    }
}
