namespace Db.TimeTrack.DbModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	public partial class Track1Simple
    {
        public int Id { get; set; }

        [Required]
        [StringLength(160)]
        public string Username { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Date { get; set; }

        public double TotalHours { get; set; }

        [StringLength(1600)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
