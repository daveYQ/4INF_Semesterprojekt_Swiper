﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swiper.Server.Models;

#nullable disable

namespace Swiper.Server.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20240403140412_AddedImages")]
    partial class AddedImages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Swiper.Server.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Swiper.Server.Models.Relationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ALikedB")
                        .HasColumnType("bit");

                    b.Property<bool>("BLikedA")
                        .HasColumnType("bit");

                    b.Property<int>("UserAId")
                        .HasColumnType("int");

                    b.Property<int>("UserBId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserAId");

                    b.HasIndex("UserBId");

                    b.ToTable("Relationships");
                });

            modelBuilder.Entity("Swiper.Server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Swiper.Server.Models.Image", b =>
                {
                    b.HasOne("Swiper.Server.Models.User", null)
                        .WithMany("Images")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Swiper.Server.Models.Relationship", b =>
                {
                    b.HasOne("Swiper.Server.Models.User", "UserA")
                        .WithMany()
                        .HasForeignKey("UserAId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Swiper.Server.Models.User", "UserB")
                        .WithMany()
                        .HasForeignKey("UserBId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserA");

                    b.Navigation("UserB");
                });

            modelBuilder.Entity("Swiper.Server.Models.User", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
