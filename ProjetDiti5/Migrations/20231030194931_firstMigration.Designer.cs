﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjetDiti5;

#nullable disable

namespace ProjetDiti5.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231030194931_firstMigration")]
    partial class firstMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjetDiti5.Model.Article", b =>
                {
                    b.Property<int>("IdArticle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdArticle"));

                    b.Property<int>("CodeArticle")
                        .HasColumnType("int");

                    b.Property<DateTime>("DatePeremption")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<double>("PrixUnit")
                        .HasColumnType("float");

                    b.Property<int>("Quantite")
                        .HasColumnType("int");

                    b.HasKey("IdArticle");

                    b.ToTable("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
