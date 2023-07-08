﻿// <auto-generated />
using System;
using Finance_Organizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Finance_Organizer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230708161630_ChangeTheCategoriesName")]
    partial class ChangeTheCategoriesName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Finance_Organizer.Categories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("CommunalServices")
                        .HasColumnType("float");

                    b.Property<double>("Leisure")
                        .HasColumnType("float");

                    b.Property<double>("Meal")
                        .HasColumnType("float");

                    b.Property<double>("Medicine")
                        .HasColumnType("float");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<double>("Purchases")
                        .HasColumnType("float");

                    b.Property<double>("Savings")
                        .HasColumnType("float");

                    b.Property<double>("Transport")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("TheCategories");
                });

            modelBuilder.Entity("Finance_Organizer.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Finance_Organizer.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoriesId");

                    b.HasIndex("PersonId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Finance_Organizer.Transaction", b =>
                {
                    b.HasOne("Finance_Organizer.Categories", "Categories")
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Finance_Organizer.Person", null)
                        .WithMany("Transactions")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Finance_Organizer.Person", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
