using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VeriTabaniOdev.Models;

public partial class FileDibiDbSonContext : DbContext
{
    public FileDibiDbSonContext()
    {
    }

    public FileDibiDbSonContext(DbContextOptions<FileDibiDbSonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fixture> Fixtures { get; set; }

    public virtual DbSet<League> Leagues { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Pointstable> Pointstables { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Scoringsystem> Scoringsystems { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=fileDibiDbSon4;Username=postgres;Password=cakmak99");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fixture>(entity =>
        {
            entity.HasKey(e => e.Matchid).HasName("fixtures_pkey");

            entity.ToTable("fixtures");

            entity.Property(e => e.Matchid).HasColumnName("matchid");
            entity.Property(e => e.Awayscore)
                .HasDefaultValue(0)
                .HasColumnName("awayscore");
            entity.Property(e => e.Awayteamid).HasColumnName("awayteamid");
            entity.Property(e => e.Homescore)
                .HasDefaultValue(0)
                .HasColumnName("homescore");
            entity.Property(e => e.Hometeamid).HasColumnName("hometeamid");
            entity.Property(e => e.Leagueid).HasColumnName("leagueid");
            entity.Property(e => e.Matchdate).HasColumnName("matchdate");
            entity.Property(e => e.Matchresult)
                .HasMaxLength(10)
                .HasComputedColumnSql("\nCASE\n    WHEN (homescore > awayscore) THEN 'HomeWin'::text\n    WHEN (homescore < awayscore) THEN 'AwayWin'::text\n    WHEN (homescore = awayscore) THEN 'Draw'::text\n    ELSE 'Pending'::text\nEND", true)
                .HasColumnName("matchresult");

            entity.HasOne(d => d.Awayteam).WithMany(p => p.FixtureAwayteams)
                .HasForeignKey(d => d.Awayteamid)
                .HasConstraintName("fixtures_awayteamid_fkey");

            entity.HasOne(d => d.Hometeam).WithMany(p => p.FixtureHometeams)
                .HasForeignKey(d => d.Hometeamid)
                .HasConstraintName("fixtures_hometeamid_fkey");

            entity.HasOne(d => d.League).WithMany(p => p.Fixtures)
                .HasForeignKey(d => d.Leagueid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fixtures_leagueid_fkey");
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.HasKey(e => e.Leagueid).HasName("leagues_pkey");

            entity.ToTable("leagues");

            entity.Property(e => e.Leagueid).HasColumnName("leagueid");
            entity.Property(e => e.Leaguename)
                .HasMaxLength(100)
                .HasColumnName("leaguename");
            entity.Property(e => e.Sportid).HasColumnName("sportid");

            entity.HasOne(d => d.Sport).WithMany(p => p.Leagues)
                .HasForeignKey(d => d.Sportid)
                .HasConstraintName("leagues_sportid_fkey");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Playerid).HasName("players_pkey");

            entity.ToTable("players");

            entity.Property(e => e.Playerid).HasColumnName("playerid");
            entity.Property(e => e.Goals)
                .HasDefaultValue(0)
                .HasColumnName("goals");
            entity.Property(e => e.Playername)
                .HasMaxLength(100)
                .HasColumnName("playername");
            entity.Property(e => e.Positionid).HasColumnName("positionid");
            entity.Property(e => e.Teamid).HasColumnName("teamid");

            entity.HasOne(d => d.Position).WithMany(p => p.Players)
                .HasForeignKey(d => d.Positionid)
                .HasConstraintName("players_positionid_fkey");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.Teamid)
                .HasConstraintName("players_teamid_fkey");
        });

        modelBuilder.Entity<Pointstable>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("pointstable");

            entity.Property(e => e.Goaldifference).HasColumnName("goaldifference");
            entity.Property(e => e.Goalsconceded).HasColumnName("goalsconceded");
            entity.Property(e => e.Goalsscored).HasColumnName("goalsscored");
            entity.Property(e => e.Leagueid).HasColumnName("leagueid");
            entity.Property(e => e.Leaguename)
                .HasMaxLength(100)
                .HasColumnName("leaguename");
            entity.Property(e => e.Matchesdrawn).HasColumnName("matchesdrawn");
            entity.Property(e => e.Matcheslost).HasColumnName("matcheslost");
            entity.Property(e => e.Matchesplayed).HasColumnName("matchesplayed");
            entity.Property(e => e.Matcheswon).HasColumnName("matcheswon");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.Teamname)
                .HasMaxLength(100)
                .HasColumnName("teamname");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Positionid).HasName("positions_pkey");

            entity.ToTable("positions");

            entity.Property(e => e.Positionid).HasColumnName("positionid");
            entity.Property(e => e.Positionname)
                .HasMaxLength(50)
                .HasColumnName("positionname");
            entity.Property(e => e.Sportid).HasColumnName("sportid");

            entity.HasOne(d => d.Sport).WithMany(p => p.Positions)
                .HasForeignKey(d => d.Sportid)
                .HasConstraintName("positions_sportid_fkey");
        });

        modelBuilder.Entity<Scoringsystem>(entity =>
        {
            entity.HasKey(e => e.Sportid).HasName("scoringsystem_pkey");

            entity.ToTable("scoringsystem");

            entity.Property(e => e.Sportid)
                .ValueGeneratedNever()
                .HasColumnName("sportid");
            entity.Property(e => e.Pointsdraw)
                .HasDefaultValue(1)
                .HasColumnName("pointsdraw");
            entity.Property(e => e.Pointsloss)
                .HasDefaultValue(0)
                .HasColumnName("pointsloss");
            entity.Property(e => e.Pointswin)
                .HasDefaultValue(3)
                .HasColumnName("pointswin");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Sportid).HasName("sports_pkey");

            entity.ToTable("sports");

            entity.Property(e => e.Sportid).HasColumnName("sportid");
            entity.Property(e => e.Sportname)
                .HasMaxLength(50)
                .HasColumnName("sportname");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Teamid).HasName("teams_pkey");

            entity.ToTable("teams");

            entity.Property(e => e.Teamid).HasColumnName("teamid");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Clubpresident)
                .HasMaxLength(100)
                .HasColumnName("clubpresident");
            entity.Property(e => e.Coachname)
                .HasMaxLength(100)
                .HasColumnName("coachname");
            entity.Property(e => e.Draws)
                .HasDefaultValue(0)
                .HasColumnName("draws");
            entity.Property(e => e.Goaldifference)
                .HasComputedColumnSql("(goalsscored - goalsconceded)", true)
                .HasColumnName("goaldifference");
            entity.Property(e => e.Goalsconceded)
                .HasDefaultValue(0)
                .HasColumnName("goalsconceded");
            entity.Property(e => e.Goalsscored)
                .HasDefaultValue(0)
                .HasColumnName("goalsscored");
            entity.Property(e => e.Leagueid).HasColumnName("leagueid");
            entity.Property(e => e.Leagueposition).HasColumnName("leagueposition");
            entity.Property(e => e.Losses)
                .HasDefaultValue(0)
                .HasColumnName("losses");
            entity.Property(e => e.Points)
                .HasDefaultValue(0)
                .HasColumnName("points");
            entity.Property(e => e.Teamname)
                .HasMaxLength(100)
                .HasColumnName("teamname");
            entity.Property(e => e.Wins)
                .HasDefaultValue(0)
                .HasColumnName("wins");

            entity.HasOne(d => d.League).WithMany(p => p.Teams)
                .HasForeignKey(d => d.Leagueid)
                .HasConstraintName("teams_leagueid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
