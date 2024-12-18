﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend_for_angular_CRUD;


public class UserConfig: IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(u => u.Id);
		builder.Property(u => u.Id).IsRequired().HasDefaultValueSql("NEWID()");
		builder.Property(u => u.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
		builder.Property(u => u.Surname).HasColumnName("Surname").IsRequired().HasMaxLength(256);
		builder.Property(u => u.Age).HasColumnName("Age").IsRequired().HasMaxLength(256);
		builder.Property(u => u.RowVersion).HasColumnName("RowVersion").IsRequired().IsRowVersion();
	}
}
