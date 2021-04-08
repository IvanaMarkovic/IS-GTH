namespace GlobalThinkersHelper.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("is-project.client")]
    public partial class client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public client()
        {
            receipts = new HashSet<receipt>();
            reservations = new HashSet<reservation>();
            attends_events = new HashSet<reservation>();
        }

        public long id { get; set; }

        public long user_id { get; set; }

        [Required]
        [StringLength(255)]
        public string first_name { get; set; }

        [Required]
        [StringLength(255)]
        public string last_name { get; set; }

        [StringLength(255)]
        public string company_name { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string contact { get; set; }

        [NotMapped]
        public Contact Contact { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string note { get; set; }

        [Column(TypeName = "bit")]
        public bool status { get; set; }

        public virtual user user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<receipt> receipts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reservation> reservations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reservation> attends_events { get; set; }
    }
}
