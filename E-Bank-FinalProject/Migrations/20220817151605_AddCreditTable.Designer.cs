﻿// <auto-generated />
using System;
using E_Bank_FinalProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace E_Bank_FinalProject.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220817151605_AddCreditTable")]
    partial class AddCreditTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("E_Bank_FinalProject.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountDescription")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("AccountID");

                    b.HasIndex("UserID");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.CreditCard", b =>
                {
                    b.Property<int>("CreditCardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<string>("CreditCardName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CreditCardID");

                    b.HasIndex("AccountID");

                    b.ToTable("CreditCard", (string)null);
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("RoleID");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConfirmedPassword")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserID");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.UserRoles", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.HasIndex("UserID");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.Account", b =>
                {
                    b.HasOne("E_Bank_FinalProject.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.CreditCard", b =>
                {
                    b.HasOne("E_Bank_FinalProject.Models.Account", "Account")
                        .WithMany("CreditCards")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.UserRoles", b =>
                {
                    b.HasOne("E_Bank_FinalProject.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_Bank_FinalProject.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.Account", b =>
                {
                    b.Navigation("CreditCards");
                });

            modelBuilder.Entity("E_Bank_FinalProject.Models.User", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
