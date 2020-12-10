namespace Db.TimeTrack.DbModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("TimeInterval")]
    public partial class TimeInterval
    {
        public int Id { get; set; }

        public DateTime TimeFrom { get; set; }

        public DateTime TimeTill { get; set; }

        [StringLength(3)]
        public string JobCategoryId { get; set; }

        public int? TaskItemId { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public int? InvoiceId { get; set; }

        public int? DoneById { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual lkuJobCategory lkuJobCategory { get; set; }

        public virtual Person Person { get; set; }

        public virtual TaskItem TaskItem { get; set; }
    }
}
