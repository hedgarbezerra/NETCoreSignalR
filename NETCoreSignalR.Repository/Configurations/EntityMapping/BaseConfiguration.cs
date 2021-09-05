﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Repository.Configurations.EntityMapping
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            ConfigurateTableName(builder);
            ConfiguratePK(builder);
            ConfigurateFields(builder);
            ConfigurateFK(builder);
        }
        protected abstract void ConfigurateFields(EntityTypeBuilder<T> builder);

        protected abstract void ConfigurateFK(EntityTypeBuilder<T> builder);

        protected abstract void ConfiguratePK(EntityTypeBuilder<T> builder);
        protected abstract void ConfigurateTableName(EntityTypeBuilder<T> builder);

    }
}
