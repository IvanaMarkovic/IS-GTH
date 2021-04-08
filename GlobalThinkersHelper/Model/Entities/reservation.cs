namespace GlobalThinkersHelper.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("is-project.reservation")]
    public partial class reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public reservation()
        {
            terms = new HashSet<term>();
            clients = new HashSet<client>();
            receipts = new HashSet<receipt>();
        }

        public long id { get; set; }

        public long client_id { get; set; }

        public long user_id { get; set; }

        [Column(TypeName = "bit")]
        public bool canceled { get; set; }

        [Column(TypeName = "bit")]
        public bool status { get; set; }

        public long? event_id { get; set; }

        public decimal? event_price { get; set; }

        public virtual client client { get; set; }

        public virtual _event _event { get; set; }

        public virtual user user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<term> terms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<client> clients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<receipt> receipts { get; set; }
    }
}
