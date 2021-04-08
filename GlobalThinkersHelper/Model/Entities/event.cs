namespace GlobalThinkersHelper.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("is-project.event")]
    public partial class _event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public _event()
        {
            reservations = new HashSet<reservation>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Column(TypeName = "bit")]
        public bool deleted { get; set; }

        [Required]
        [StringLength(45)]
        public string type { get; set; }

        public decimal? event_price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reservation> reservations { get; set; }
    }
}
