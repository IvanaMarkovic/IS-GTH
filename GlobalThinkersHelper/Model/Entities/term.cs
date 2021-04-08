namespace GlobalThinkersHelper.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("is-project.term")]
    public partial class term
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public term()
        {
            items = new HashSet<item_list>();
        }

        public long id { get; set; }

        public long reservation_id { get; set; }

        public long hall_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime rental_date { get; set; }

        public TimeSpan rent_time_start { get; set; }

        public TimeSpan rent_time_end { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string note { get; set; }

        [Column(TypeName = "bit")]
        public bool status { get; set; }

        public virtual hall hall { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<item_list> items { get; set; }

        public virtual reservation reservation { get; set; }
    }
}
