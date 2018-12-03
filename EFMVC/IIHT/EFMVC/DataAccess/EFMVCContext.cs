namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EFMVCContext : DbContext
    {
        public EFMVCContext()
            : base("name=EFMVC")
        {
            var _dependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Tweet> Tweets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(e => e.user_id)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.fullName)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Tweets)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Person1)
                .WithMany(e => e.People)
                .Map(m => m.ToTable("Following").MapLeftKey("following_id").MapRightKey("user_id"));

            modelBuilder.Entity<Tweet>()
                .Property(e => e.user_id)
                .IsUnicode(false);

            modelBuilder.Entity<Tweet>()
                .Property(e => e.message)
                .IsUnicode(false);
        }
    }
}
