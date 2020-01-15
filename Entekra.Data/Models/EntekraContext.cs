using Microsoft.EntityFrameworkCore;

namespace Entekra.Data.Models
{
    public class EntekraContext : DbContext
    {
        public EntekraContext(DbContextOptions<EntekraContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Checklist> Checklist { get; set; }
        public virtual DbSet<ExtensionsDataList> ExtensionsDataList { get; set; }
        public virtual DbSet<Issue> Issue { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Expirated24HChecklist> Expirated24HChecklist { get; set; }
        public virtual DbSet<ReportFormStausesNonConformanceReport> ReportFormStausesNonConformanceReport { get; set; }
        public virtual DbSet<ReportFormStausesChangeRequest> ReportFormStausesChangeRequest { get; set; }
        public virtual DbSet<ReportTimeExpiredFormsChangeRequest> ReportTimeExpiredFormsChangeRequest { get; set; }
        public virtual DbSet<ReportTimeExpiredFormsNonConformanceReport> ReportTimeExpiredFormsNonConformanceReport { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Checklist>(entity =>
            {
                entity.HasKey(e => e.ChecklistId)
                    .HasName("PK_ChecklistId")
                    .IsClustered(false);

                //entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                //entity.HasOne(d => d.Project)
                //    .WithMany(p => p.Checklist)
                //    .HasPrincipalKey(p => p.ProjectId)
                //    .HasForeignKey(d => d.ChecklistId)
                //    .HasConstraintName("FK_Checklists_ProjectId");
            });

            modelBuilder.Entity<Expirated24HChecklist>(entity =>
            {
                entity.HasKey(e => e.Expirated24HChecklistId)
                    .HasName("PK_Expirated24HChecklist")
                    .IsClustered(false);
            });

            modelBuilder.Entity<ExtensionsDataList>(entity =>
            {
                entity.HasKey(e => e.ExtensionsDataListId)
                    .HasName("PK_ExtensionsDataListId")
                    .IsClustered(false);

                //entity.HasOne(d => d.CheckList)
                //    .WithMany(p => p.ExtensionsDataList)
                //    .HasPrincipalKey(p => p.ChecklistId)
                //    .HasForeignKey(d => d.ExtensionsDataListId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_ExtensionsDataLists_CheckListId");


            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => e.IssueId)
                    .HasName("PK_IssuesId")
                    .IsClustered(false);

                //entity.HasIndex(e => e.Id)
                //    .HasName("UK_Issues_Id")
                //    .IsUnique();

                //entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                //entity.HasOne(d => d.Project)
                //    .WithMany(p => p.Issues)
                //    .HasPrincipalKey(p => p.ProjectId)
                //    .HasForeignKey(d => d.IssueId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Issues_ProjectId");

                //entity.HasOne(d => d.ExtensionsDataList)
                //    .WithMany(p => p.AttachedIssues)
                //    .HasPrincipalKey(p => p.ExtensionsDataListId)
                //    .HasForeignKey(d => d.IssueId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Issues_ExtensionsDataListId");

            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("PK_ProjectsId")
                    .IsClustered(false);

                entity.HasIndex(e => e.ProjectId)
                    .HasName("UK_Projects_ProjectId")
                    .IsUnique();

            });

            
            modelBuilder.Entity<ReportFormStausesChangeRequest>((pc =>
            {
                pc.HasNoKey();
                pc.ToView("View_ReportFormStauses_ChangeRequest");
            }));

            modelBuilder.Entity<ReportFormStausesNonConformanceReport>((pc =>
            {
                pc.HasNoKey();
                pc.ToView("View_ReportFormStauses_NonConformanceReport");
            }));

            modelBuilder.Entity<ReportTimeExpiredFormsChangeRequest>((pc =>
            {
                pc.HasNoKey();
                pc.ToView("View_ReportTimeExpiredForms_ChangeRequest");
            }));

            modelBuilder.Entity<ReportTimeExpiredFormsNonConformanceReport>((pc =>
            {
                pc.HasNoKey();
                pc.ToView("View_ReportTimeExpiredForms_NonConformanceReport");
            }));
        }
    }
}
