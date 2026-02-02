using Messaging_Service.src._01_Domain.Core.Common;
using Messaging_Service.src._01_Domain.Core.Entities;
using Messaging_Service.src._01_Domain.Core.Interfaces.Repositories;
using Messaging_Service.src._03_Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System;

namespace Messaging_Service.src._03_Infrastructure.Data
{
    public class MessagingDbContext : DbContext, IUnitOfWork
    {
        public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }
        public DbSet<MessageTemplate> MessageTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Registering explicit configurations
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new ChatParticipantConfiguration());

            var messageEntity = modelBuilder.Entity<Message>();

            // Configure MessageContent (Value Object)
            messageEntity.OwnsOne(x => x.Content, navigation =>
            {
                navigation.Property(p => p.Text).HasColumnName("ContentText").IsRequired().HasMaxLength(2000);
            });

            // Configure AuditInfo (Value Object)
            messageEntity.OwnsOne(x => x.Audit, navigation =>
            {
                navigation.Property(p => p.IpAddress).HasColumnName("IpAddress").HasMaxLength(50);
                navigation.Property(p => p.UserAgent).HasColumnName("UserAgent").HasMaxLength(500);
            });
        }

        public async Task<int> CommitAsync()
        {
            var tehranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SetCreationDate(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tehranTimeZone));
                        break;

                    case EntityState.Modified:
                        entry.Entity.SetModificationDate(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tehranTimeZone));
                        break;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
