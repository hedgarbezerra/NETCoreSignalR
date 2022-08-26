using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NETCoreSignalR.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;

namespace NETCoreSignalR.Repository.Configurations.EntityMapping
{
    public class a
    {
        public void SD()
        {
            var builder = new FluentConfiguration<EventLog>();
            var efbuilder = builder.ToTable("aa")
                .AsPrimaryKey("")
                .WithForeignKey("")
                .HasField("")
                .HasField(a => a.MessageTemplate)
                .Build();
        }
    }
    // class created for training porpuses only
    [ExcludeFromCodeCoverage]
    public class FluentConfiguration<T> :
        ITypeSelection<T>,
        ITableNameSelection<T>,
        IPrimaryKeySelection<T>,
        IForeignKeySelection<T>,
        IHasFieldConfiguration<T> where T : class

    {
        private readonly EntityTypeBuilder<T> _builder;
        private FluentConfiguration(EntityTypeBuilder<T> builder)
        {
            _builder = builder;
        }

        public FluentConfiguration() { }

        public static FluentConfiguration<T> Begin(EntityTypeBuilder<T> builder)
        {
            return new FluentConfiguration<T>(builder);
        }

        public ITableNameSelection<T> ToTable(string tableName)
        {
            _builder.ToTable(tableName);

            return this;
        }

        public IPrimaryKeySelection<T> AsPrimaryKey(string keyName)
        {
            _builder.HasKey(keyName);

            return this;
        }
        public IPrimaryKeySelection<T> AsPrimaryKey(Expression<Func<T, object>> expression)
        {
            _builder.Property(expression);

            return this;
        }
        public IForeignKeySelection<T> WithForeignKey(string relatedTypeName, string navigationName)
        {
            _builder.HasOne(relatedTypeName, navigationName);

            return this;
        }

        public IForeignKeySelection<T> WithForeignKey(string navigationName)
        {
            _builder.HasOne(navigationName);

            return this;
        }

        public IHasFieldConfiguration<T> HasField(string keyName)
        {
            //logic
            return this;
        }

        public IHasFieldConfiguration<T> HasField(Expression<Func<T, object>> expression)
        {
            //logic
            return this;
        }

        public T Build()
        {
            return null;
        }
    }


    public interface ITypeSelection<T> where T : class
    {
        public ITableNameSelection<T> ToTable(string tableName);
    }

    public interface ITableNameSelection<T> where T : class
    {
        public IPrimaryKeySelection<T> AsPrimaryKey(string keyName);
        public IPrimaryKeySelection<T> AsPrimaryKey(Expression<Func<T, object>> expression);
    }

    public interface IPrimaryKeySelection<T> : IBuilder<T> where T : class
    {
        public IForeignKeySelection<T> WithForeignKey(string relatedTypeName, string navigationName);
        public IForeignKeySelection<T> WithForeignKey(string navigationName);
    }

    public interface IForeignKeySelection<T> : IBuilder<T> where T : class
    {
        public IHasFieldConfiguration<T> HasField(string keyName);
        public IHasFieldConfiguration<T> HasField(Expression<Func<T, object>> expression);
    }
    public interface IHasFieldConfiguration<T> : IForeignKeySelection<T> where T : class
    {
    }
    public interface IBuilder<T> where T : class
    {
        T Build();
    }

}
