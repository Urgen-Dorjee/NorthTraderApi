﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NorthTraderAPI.Models;

namespace NorthTraderAPI.EntityConfigurations;


public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {

        builder.Property(e => e.CategoryId).HasColumnName("CategoryID");
        builder.Property(e => e.CategoryName).IsRequired().HasMaxLength(15);
        builder.Property(e => e.Description).HasColumnType("ntext");
        builder.Property(e => e.Picture).HasColumnType("image");
    }
}


