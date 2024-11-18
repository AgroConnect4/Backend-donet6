using agroApp.Domain;
using agroApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<EventComment> EventComments { get; set; }
        public DbSet<PostShare> PostShares { get; set; }
        public DbSet<EventShare> EventShares { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
             //Adicione a linha abaixo se ainda não estiver configurada
         //    optionsBuilder.UseSqlServer("Data Source=sqlserveragroconnect.database.windows.net;Initial Catalog=sql_agroconnect;User ID=kamydados;Password=Pacoca2005.;Connect Timeout=30;Encrypt=True",
        //        b => b.MigrationsAssembly("agroApp.Infra.Data"));
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //builder.Services.AddDbContext<AppDbContext>(options =>
        //options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        //            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Server=localhost;Database=agroconnect;Uid=root;Pwd=QQS%W37472022;")),
        //            b => b.MigrationsAssembly("agroApp.Infra.Data")));
        //}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); 

            // Relacionamento User-Profile (um para um)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);

            // Relacionamento User-UserRole (um para muitos)
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            // Relacionamento Role-UserRole (um para muitos)
            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Relacionamento Post-User (um para muitos)
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.OrganizedEvents)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User) 
                .WithMany(u => u.Posts) 
                .HasForeignKey(p => p.UserId);

                modelBuilder.Entity<Post>()
                .HasMany(p => p.Shares)
                .WithOne(s => s.Post)
                .HasForeignKey(s => s.PostId);

                modelBuilder.Entity<Event>()
                .HasMany(e => e.Shares)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId);

            // Relacionamento User-PostComment
            modelBuilder.Entity<User>()
                .HasMany(u => u.PostComments) 
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            // Relacionamento User-EventComment
            modelBuilder.Entity<User>()
                .HasMany(u => u.EventComments) 
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            // Relacionamento User-PostShare
            modelBuilder.Entity<User>()
                .HasMany(u => u.PostShares)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            // Relacionamento User-EventShare
            modelBuilder.Entity<User>()
                .HasMany(u => u.EventShares)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments) // Corrigindo a propriedade para PostComment
                .WithOne(c => c.Post) 
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Comments)
                .WithOne(c => c.Event)
                .HasForeignKey(c => c.EventId);

            // Relacionamento Notification-User (um para muitos)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            // Relacionamento Connection-User (um para muitos)
            modelBuilder.Entity<Connection>()
                .HasOne(c => c.User)
                .WithMany(u => u.Connections) 
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Relacionamento Connection-ConnectedUser (um para muitos)
            modelBuilder.Entity<Connection>()
                .HasOne(c => c.ConnectedUser)
                .WithMany(u => u.ConnectedTo) 
                .HasForeignKey(c => c.ConnectedUserId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Relacionamento PostComment-Post (um para muitos)
            modelBuilder.Entity<PostComment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento PostShare-Post (um para muitos)
            modelBuilder.Entity<PostShare>()
                .HasOne(s => s.Post)
                .WithMany(p => p.Shares)
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento EventComment-Event (um para muitos)
            modelBuilder.Entity<EventComment>()
                .HasOne(c => c.Event)
                .WithMany(e => e.Comments)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento EventShare-Event (um para muitos)
            modelBuilder.Entity<EventShare>()
                .HasOne(s => s.Event)
                .WithMany(e => e.Shares)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento PostComment-User (um para muitos)
            modelBuilder.Entity<PostComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.PostComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento EventComment-User (um para muitos)
            modelBuilder.Entity<EventComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.EventComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostShare>()
                .HasOne(s => s.User) // 's' para o PostShare
                .WithMany(u => u.PostShares)
                .HasForeignKey(s => s.UserId) // 's' para o PostShare
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento EventShare-User (um para muitos)
            modelBuilder.Entity<EventShare>()
                .HasOne(s => s.User) // 's' para o EventShare
                .WithMany(u => u.EventShares)
                .HasForeignKey(s => s.UserId) // 's' para o EventShare
                .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<PostComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.PostComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.EventComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}