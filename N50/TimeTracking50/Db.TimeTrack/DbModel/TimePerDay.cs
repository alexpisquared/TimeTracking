namespace Db.TimeTrack.DbModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("TimePerDay")]
    public partial class TimePerDay
    {
        public int Id { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime WorkedOn { get; set; }

        public double WorkedHours { get; set; }

        public double? HourStarted { get; set; }

        [StringLength(3)]
        public string JobCategoryId { get; set; }

        public int? TaskItemId { get; set; }

        [StringLength(800)]
        public string Note { get; set; }

        public int? InvoiceId { get; set; }

        public bool IsLocked { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual lkuJobCategory lkuJobCategory { get; set; }

        public virtual TaskItem TaskItem { get; set; }
    }
}
