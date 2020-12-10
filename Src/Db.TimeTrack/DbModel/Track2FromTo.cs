namespace Db.TimeTrack.DbModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

	public partial class Track2FromTo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(160)]
        public string Username { get; set; }

        public DateTime WorkedFrom { get; set; }

        public DateTime WorkedTill { get; set; }

        [StringLength(1600)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
