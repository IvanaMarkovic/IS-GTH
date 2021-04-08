namespace GlobalThinkersHelper.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("is-project.receipt")]
    public partial class receipt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public receipt()
        {
            receipts = new HashSet<receipt>();
        }

        public long id { get; set; }

        public long user_id { get; set; }

        public long client_id { get; set; }

        public long reservation_id { get; set; }

        [Column(TypeName = "bit")]
        public bool installments { get; set; }

        public long? receipt_id { get; set; }

        public decimal amount { get; set; }

        public decimal? discount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? payment_date { get; set; }

        [Column(TypeName = "bit")]
        public bool paid { get; set; }

        [Column(TypeName = "bit")]
        public bool status { get; set; }

        [StringLength(45)]
        public string serial_number { get; set; }

        public virtual client client { get; set; }

        public virtual reservation reservation { get; set; }

        public virtual user user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<receipt> receipts { get; set; }

        public virtual receipt primary_reciept { get; set; }
    }
}
