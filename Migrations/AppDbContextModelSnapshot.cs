﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TalkStream_API.Database;

#nullable disable

namespace TalkStream_API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("TalkStream_API.Entities.User", b =>
                {
                    b.Property<string>("Uid")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("varchar(120)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserUid")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Uid");

                    b.HasIndex("UserUid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TalkStream_API.Entities.User", b =>
                {
                    b.HasOne("TalkStream_API.Entities.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserUid");
                });

            modelBuilder.Entity("TalkStream_API.Entities.User", b =>
                {
                    b.Navigation("Friends");
                });
#pragma warning restore 612, 618
        }
    }
}
