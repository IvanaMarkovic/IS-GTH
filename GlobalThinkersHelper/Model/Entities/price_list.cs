namespace GlobalThinkersHelper.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("is-project.price_list")]
    public partial class price_list
    {
        public price_list() { }

        public long id { get; set; }

        public long hall_id { get; set; }

        public float price_hour { get; set; }

        public TimeSpan? time_from { get; set; }

        public TimeSpan? time_to { get; set; }

        [Column(TypeName = "bit")]
        public bool status { get; set; }

        [Column(TypeName = "enum")]
        [Required]
        [StringLength(65532)]
        public string day { get; set; }

        [NotMapped]
        public string daySr { get; set; }

        public virtual hall hall { get; set; }
    }
}
