namespace GlobalThinkersHelper.Model.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public partial class Database : DbContext
    {
        public Database()
            : base("name=Database")
        {
        }

        public virtual DbSet<client> clients { get; set; }
        public virtual DbSet<_event> events { get; set; }
        public virtual DbSet<hall> halls { get; set; }
        public virtual DbSet<item> items { get; set; }
        public virtual DbSet<item_list> item_list { get; set; }
        public virtual DbSet<price_list> price_list { get; set; }
        public virtual DbSet<receipt> receipts { get; set; }
        public virtual DbSet<reservation> reservations { get; set; }
        public virtual DbSet<term> terms { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<client>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .Property(e => e.company_name)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .Property(e => e.contact)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<client>()
                .HasMany(e => e.receipts)
                .WithRequired(e => e.client)
                .HasForeignKey(e => e.client_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<client>()
                .HasMany(e => e.reservations)
                .WithRequired(e => e.client)
                .HasForeignKey(e => e.client_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<_event>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<_event>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<_event>()
                .HasMany(e => e.reservations)
                .WithOptional(e => e._event)
                .HasForeignKey(e => e.event_id);

            modelBuilder.Entity<hall>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .HasMany(e => e.price_lists)
                .WithRequired(e => e.hall)
                .HasForeignKey(e => e.hall_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<hall>()
                .HasMany(e => e.terms)
                .WithRequired(e => e.hall)
                .HasForeignKey(e => e.hall_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<item>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.item_list)
                .WithRequired(e => e.item)
                .HasForeignKey(e => e.item_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<price_list>()
                .Property(e => e.day)
                .IsUnicode(false);

            modelBuilder.Entity<receipt>()
                .HasMany(e => e.receipts)
                .WithOptional(e => e.primary_reciept)
                .HasForeignKey(e => e.receipt_id);

            modelBuilder.Entity<receipt>()
                .Property(e => e.serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<reservation>()
                .HasMany(e => e.terms)
                .WithRequired(e => e.reservation)
                .HasForeignKey(e => e.reservation_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<reservation>()
               .HasMany(e => e.receipts)
               .WithRequired(e => e.reservation)
               .HasForeignKey(e => e.reservation_id)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<reservation>()
                .HasMany(e => e.clients)
                .WithMany(e => e.attends_events)
                .Map(m => m.ToTable("attends", "is-project"));

            modelBuilder.Entity<term>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<term>()
                .HasMany(e => e.items)
                .WithRequired(e => e.term)
                .HasForeignKey(e => e.term_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.clients)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.receipts)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.reservations)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);
        }
    }
}
