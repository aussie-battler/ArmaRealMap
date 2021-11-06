﻿// <auto-generated />
using System;
using ArmaRealMapWebSite.Entities.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArmaRealMapWebSite.Migrations
{
    [DbContext(typeof(AssetsContext))]
    [Migration("20211106115122_Libraries")]
    partial class Libraries
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.Asset", b =>
                {
                    b.Property<int>("AssetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AssetCategory")
                        .HasColumnType("INTEGER");

                    b.Property<float>("BoundingSphereDiameter")
                        .HasColumnType("REAL");

                    b.Property<float>("CX")
                        .HasColumnType("REAL");

                    b.Property<float>("CY")
                        .HasColumnType("REAL");

                    b.Property<float>("CZ")
                        .HasColumnType("REAL");

                    b.Property<string>("ClassName")
                        .HasColumnType("TEXT");

                    b.Property<float>("Depth")
                        .HasColumnType("REAL");

                    b.Property<int>("GameModID")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Height")
                        .HasColumnType("REAL");

                    b.Property<float>("MaxX")
                        .HasColumnType("REAL");

                    b.Property<float>("MaxY")
                        .HasColumnType("REAL");

                    b.Property<float>("MaxZ")
                        .HasColumnType("REAL");

                    b.Property<float>("MinX")
                        .HasColumnType("REAL");

                    b.Property<float>("MinY")
                        .HasColumnType("REAL");

                    b.Property<float>("MinZ")
                        .HasColumnType("REAL");

                    b.Property<string>("ModelPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("TerrainBuilderTemplateXML")
                        .HasColumnType("TEXT");

                    b.Property<int>("TerrainRegions")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("AssetID");

                    b.HasIndex("GameModID");

                    b.ToTable("Asset");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.AssetPreview", b =>
                {
                    b.Property<int>("AssetPreviewID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AssetID")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Data")
                        .HasColumnType("BLOB");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("AssetPreviewID");

                    b.HasIndex("AssetID");

                    b.ToTable("AssetPreview");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.GameMod", b =>
                {
                    b.Property<int>("GameModID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("GameModID");

                    b.ToTable("GameMod");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.ObjectLibrary", b =>
                {
                    b.Property<int>("ObjectLibraryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Density")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("ObjectCategory")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Probability")
                        .HasColumnType("REAL");

                    b.Property<int>("TerrainRegion")
                        .HasColumnType("INTEGER");

                    b.HasKey("ObjectLibraryID");

                    b.ToTable("ObjectLibrary");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.ObjectLibraryAsset", b =>
                {
                    b.Property<int>("ObjectLibraryAssetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AssetID")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("MaxZ")
                        .HasColumnType("REAL");

                    b.Property<float?>("MinZ")
                        .HasColumnType("REAL");

                    b.Property<int>("ObjectLibraryID")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("PlacementRadius")
                        .HasColumnType("REAL");

                    b.Property<float?>("Probability")
                        .HasColumnType("REAL");

                    b.Property<float?>("ReservedRadius")
                        .HasColumnType("REAL");

                    b.HasKey("ObjectLibraryAssetID");

                    b.HasIndex("AssetID");

                    b.HasIndex("ObjectLibraryID");

                    b.ToTable("ObjectLibraryAsset");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.Asset", b =>
                {
                    b.HasOne("ArmaRealMapWebSite.Entities.Assets.GameMod", "GameMod")
                        .WithMany()
                        .HasForeignKey("GameModID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameMod");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.AssetPreview", b =>
                {
                    b.HasOne("ArmaRealMapWebSite.Entities.Assets.Asset", "Asset")
                        .WithMany("Previews")
                        .HasForeignKey("AssetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.ObjectLibraryAsset", b =>
                {
                    b.HasOne("ArmaRealMapWebSite.Entities.Assets.Asset", "Asset")
                        .WithMany()
                        .HasForeignKey("AssetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArmaRealMapWebSite.Entities.Assets.ObjectLibrary", "ObjectLibrary")
                        .WithMany("Assets")
                        .HasForeignKey("ObjectLibraryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("ObjectLibrary");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.Asset", b =>
                {
                    b.Navigation("Previews");
                });

            modelBuilder.Entity("ArmaRealMapWebSite.Entities.Assets.ObjectLibrary", b =>
                {
                    b.Navigation("Assets");
                });
#pragma warning restore 612, 618
        }
    }
}
