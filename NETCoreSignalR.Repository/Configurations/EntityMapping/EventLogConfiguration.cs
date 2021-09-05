﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Util.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Repository.Configurations.EntityMapping
{
    public sealed class EventLogConfiguration : BaseConfiguration<EventLog>
    {
        public EventLogConfiguration()
            : base()
        {
        }

        protected override void ConfigurateFields(EntityTypeBuilder<EventLog> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnType("int")
                .HasColumnName("Id");

            builder.Property(c => c.Message)
                .HasColumnType("nvarchar")
                .HasColumnName("Message");

            builder.Property(c => c.MessageTemplate)
                .HasColumnType("nvarchar")
                .HasColumnName("MessageTemplate");

            builder.Property(c => c.Exception)
                .HasColumnType("nvarchar")
                .HasColumnName("Exception");

            builder.Property(c => c.CreatedTime)
                .HasColumnType("datetime")
                .HasColumnName("TimeStamp");

            builder.Property(c => c.LogLevel)
                .HasColumnType("nvarchar")
                .HasColumnName("Level");

            builder.Property(c => c.Properties)
                .HasColumnType("nvarchar")
                .HasColumnName("Properties");

            builder.Ignore(c => c.XmlContent);
        }

        protected override void ConfigurateFK(EntityTypeBuilder<EventLog> builder)
        {
        }

        protected override void ConfiguratePK(EntityTypeBuilder<EventLog> builder)
        {
            builder.HasKey(c => c.Id);
        }

        protected override void ConfigurateTableName(EntityTypeBuilder<EventLog> builder)
        {
            builder.ToTable("TbLog");
        }
    }
}
