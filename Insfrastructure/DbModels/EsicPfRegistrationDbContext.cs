using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Insfrastructure.DbModels;

public partial class EsicPfRegistrationDbContext : DbContext
{
    public EsicPfRegistrationDbContext()
    {
    }

    public EsicPfRegistrationDbContext(DbContextOptions<EsicPfRegistrationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DispenseryDetail> DispenseryDetails { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<EmpAddress> EmpAddresses { get; set; }

    public virtual DbSet<EmpBankDetail> EmpBankDetails { get; set; }

    public virtual DbSet<EmployeeRegistration> EmployeeRegistrations { get; set; }

    public virtual DbSet<EmploymentDetail> EmploymentDetails { get; set; }

    public virtual DbSet<FamilyParticular> FamilyParticulars { get; set; }

    public virtual DbSet<NomineeDetail> NomineeDetails { get; set; }

    public virtual DbSet<PfRegistration> PfRegistrations { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.;Database=ESIC-PF-RegistrationDB;User Id=sa;Password=aparna@321;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DispenseryDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dispense__3214EC07A8801F51");

            entity.Property(e => e.DispensaryNameForFamily).HasMaxLength(100);
            entity.Property(e => e.DispensaryNameForIp)
                .HasMaxLength(100)
                .HasColumnName("DispensaryNameForIP");
            entity.Property(e => e.DispensaryOrImpormEudforFamily)
                .HasMaxLength(100)
                .HasColumnName("DispensaryOrIMPOrmEUDForFamily");
            entity.Property(e => e.DispensaryOrImpormEudforIp)
                .HasMaxLength(100)
                .HasColumnName("DispensaryOrIMPOrmEUDForIP");
            entity.Property(e => e.DistrictIdForIp).HasColumnName("DistrictIdForIP");
            entity.Property(e => e.StateIdForIp).HasColumnName("StateIdForIP");

            entity.HasOne(d => d.Employee).WithMany(p => p.DispenseryDetails)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dispensary_InsuredPerson");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC07D1E3015F");

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.State).WithMany(p => p.Districts)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Districts_States");
        });

        modelBuilder.Entity<EmpAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmpAddre__3214EC07714DA4D7");

            entity.ToTable("EmpAddress");

            entity.Property(e => e.PmtAddress).HasMaxLength(500);
            entity.Property(e => e.PmtEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PmtMobile)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PstAddress).HasMaxLength(500);
            entity.Property(e => e.PstEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PstMobile)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmpAddresses)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMP_InsuredPerson");

            entity.HasOne(d => d.PmtDistrict).WithMany(p => p.EmpAddressPmtDistricts)
                .HasForeignKey(d => d.PmtDistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMP_PmtDistrict");

            entity.HasOne(d => d.PmtState).WithMany(p => p.EmpAddressPmtStates)
                .HasForeignKey(d => d.PmtStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMP_PmtState");

            entity.HasOne(d => d.PstDistrict).WithMany(p => p.EmpAddressPstDistricts)
                .HasForeignKey(d => d.PstDistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMP_PstDistrict");

            entity.HasOne(d => d.PstState).WithMany(p => p.EmpAddressPstStates)
                .HasForeignKey(d => d.PstStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMP_PstState");
        });

        modelBuilder.Entity<EmpBankDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmpBankD__3214EC07BDC47700");

            entity.Property(e => e.AccountNumber).HasMaxLength(100);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.BranchName).HasMaxLength(100);
            entity.Property(e => e.Ifsc)
                .HasMaxLength(100)
                .HasColumnName("IFSC");
            entity.Property(e => e.Micr)
                .HasMaxLength(100)
                .HasColumnName("MICR");
            entity.Property(e => e.TypeofAccount).HasMaxLength(20);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmpBankDetails)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bank_InsuredPerson");
        });

        modelBuilder.Entity<EmployeeRegistration>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11F497F920");

            entity.ToTable("EmployeeRegistration");

            entity.Property(e => e.AadhaarNo).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.FatherOrHusband).HasMaxLength(10);
            entity.Property(e => e.FatherOrHusbandName).HasMaxLength(200);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.IpNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IP_Number");
            entity.Property(e => e.IsEsicavailable).HasColumnName("IsESICAvailable");
            entity.Property(e => e.IsEsicdisabled).HasColumnName("IsESICDisabled");
            entity.Property(e => e.MaritalStatus).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PanNo).HasMaxLength(50);
            entity.Property(e => e.TypeOfDisability).HasMaxLength(200);
        });

        modelBuilder.Entity<EmploymentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employme__3214EC07CFD56679");

            entity.Property(e => e.DojofCurrentEmployer)
                .HasColumnType("datetime")
                .HasColumnName("DOJofCurrentEmployer");
            entity.Property(e => e.EmployerAddress).HasMaxLength(200);
            entity.Property(e => e.EmployerCode).HasMaxLength(100);
            entity.Property(e => e.EmployerName).HasMaxLength(100);
            entity.Property(e => e.HasPreviousEmployer)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.PreviousInsuarenceNo).HasMaxLength(100);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmploymentDetails)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employment_InsuredPerson");
        });

        modelBuilder.Entity<FamilyParticular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FamilyPa__3214EC077CF57DFC");

            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Relationship).HasMaxLength(50);
            entity.Property(e => e.ResidingWith).HasMaxLength(5);

            entity.HasOne(d => d.Employee).WithMany(p => p.FamilyParticulars)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Family_InsuredPerson");
        });

        modelBuilder.Entity<NomineeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NomineeD__3214EC07856F7A98");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.IsFamilyMember)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Relationship).HasMaxLength(50);

            entity.HasOne(d => d.District).WithMany(p => p.NomineeDetails)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nominee_District");

            entity.HasOne(d => d.Employee).WithMany(p => p.NomineeDetails)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nominee_InsuredPerson");

            entity.HasOne(d => d.State).WithMany(p => p.NomineeDetails)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nominee_State");
        });

        modelBuilder.Entity<PfRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PF_Regis__3214EC074AE4C155");

            entity.ToTable("PF_Registration");

            entity.Property(e => e.AadhaarDob)
                .HasColumnType("datetime")
                .HasColumnName("Aadhaar_Dob");
            entity.Property(e => e.AadhaarFullName)
                .HasMaxLength(200)
                .HasColumnName("Aadhaar_FullName");
            entity.Property(e => e.AadhaarGender)
                .HasMaxLength(20)
                .HasColumnName("Aadhaar_Gender");
            entity.Property(e => e.AadhaarNo).HasMaxLength(20);
            entity.Property(e => e.Doj)
                .HasColumnType("datetime")
                .HasColumnName("DOJ");
            entity.Property(e => e.Emailid).HasMaxLength(100);
            entity.Property(e => e.FatherOrHusband).HasMaxLength(10);
            entity.Property(e => e.FatherOrHusbandname).HasMaxLength(100);
            entity.Property(e => e.MaritalStatus).HasMaxLength(50);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.PanDob)
                .HasColumnType("datetime")
                .HasColumnName("Pan_Dob");
            entity.Property(e => e.PanFullName)
                .HasMaxLength(200)
                .HasColumnName("Pan_FullName");
            entity.Property(e => e.PanGender)
                .HasMaxLength(20)
                .HasColumnName("Pan_Gender");
            entity.Property(e => e.PanNo).HasMaxLength(20);
            entity.Property(e => e.Qualification).HasMaxLength(100);
            entity.Property(e => e.Uan)
                .HasMaxLength(12)
                .HasColumnName("UAN");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__States__3214EC073BE29561");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0710FEFBBB");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
