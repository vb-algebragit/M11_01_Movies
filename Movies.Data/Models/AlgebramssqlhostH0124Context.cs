using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Movies.Data.Models;

public partial class AlgebramssqlhostH0124Context : DbContext
{
    public AlgebramssqlhostH0124Context()
    {
    }

    public AlgebramssqlhostH0124Context(DbContextOptions<AlgebramssqlhostH0124Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=mssql9.mojsite.com,1555;Database=algebramssqlhost__h-01_24;User Id=algebramssqlhost_v_brl;Password=ysrixca5ungwhkobp2zdjlfetmqv;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("algebramssqlhost_v_brl");

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
