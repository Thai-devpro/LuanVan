using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LuanVan.Data;

public partial class NienluancosoContext : DbContext
{
    public NienluancosoContext()
    {
    }

    public NienluancosoContext(DbContextOptions<NienluancosoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chiendich> Chiendiches { get; set; }

    public virtual DbSet<Chucnang> Chucnangs { get; set; }

    public virtual DbSet<Chucvu> Chucvus { get; set; }

    public virtual DbSet<HienVat> HienVats { get; set; }

    public virtual DbSet<LoaiHv> LoaiHvs { get; set; }

    public virtual DbSet<Manhthuongquan> Manhthuongquans { get; set; }

    public virtual DbSet<Noihotro> Noihotros { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<TaikhoanAdmin> TaikhoanAdmins { get; set; }

    public virtual DbSet<Thanhvien> Thanhviens { get; set; }

    public virtual DbSet<TtQuyengopHienvat> TtQuyengopHienvats { get; set; }

    public virtual DbSet<TtTraotang> TtTraotangs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAMQUOCTHAI\\SQLEXPRESS;Database=NIENLUANCOSO;Integrated Security=True;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chiendich>(entity =>
        {
            entity.HasKey(e => e.MaCd);

            entity.ToTable("CHIENDICH");

            entity.Property(e => e.MaCd).HasColumnName("MA_CD");
            entity.Property(e => e.AnhCd)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("ANH_CD");
            entity.Property(e => e.MaNoi).HasColumnName("MA_NOI");
            entity.Property(e => e.MaTv).HasColumnName("MA_TV");
            entity.Property(e => e.Ngaybatdau)
                .HasColumnType("datetime")
                .HasColumnName("NGAYBATDAU");
            entity.Property(e => e.Ngayketthuc)
                .HasColumnType("datetime")
                .HasColumnName("NGAYKETTHUC");
            entity.Property(e => e.NoidungCd)
                .HasMaxLength(500)
                .IsFixedLength()
                .HasColumnName("NOIDUNG_CD");
            entity.Property(e => e.TenCd)
                .HasMaxLength(50)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1253_CI_AI")
                .HasColumnName("TEN_CD");

            entity.HasOne(d => d.MaNoiNavigation).WithMany(p => p.Chiendiches)
                .HasForeignKey(d => d.MaNoi)
                .HasConstraintName("FK_CHIENDICH_NOIHOTRO");

            entity.HasOne(d => d.MaTvNavigation).WithMany(p => p.Chiendiches)
                .HasForeignKey(d => d.MaTv)
                .HasConstraintName("FK_CHIENDICH_THANHVIEN");
        });

        modelBuilder.Entity<Chucnang>(entity =>
        {
            entity.HasKey(e => e.MaCn);

            entity.ToTable("CHUCNANG");

            entity.Property(e => e.MaCn).HasColumnName("MA_CN");
            entity.Property(e => e.TenCn)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("TEN_CN");
        });

        modelBuilder.Entity<Chucvu>(entity =>
        {
            entity.HasKey(e => e.MaCv);

            entity.ToTable("CHUCVU");

            entity.Property(e => e.MaCv).HasColumnName("MA_CV");
            entity.Property(e => e.TenCv)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("TEN_CV");
        });

        modelBuilder.Entity<HienVat>(entity =>
        {
            entity.HasKey(e => e.MaHv);

            entity.ToTable("HIEN_VAT");

            entity.Property(e => e.MaHv).HasColumnName("MA_HV");
            entity.Property(e => e.Donvitinh)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("DONVITINH");
            entity.Property(e => e.Gia).HasColumnName("GIA");
            entity.Property(e => e.MaLoai).HasColumnName("MA_LOAI");
            entity.Property(e => e.Soluongcon).HasColumnName("SOLUONGCON");
            entity.Property(e => e.TenHv)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("TEN_HV");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.HienVats)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HIEN_VAT_LOAI_HV");
        });

        modelBuilder.Entity<LoaiHv>(entity =>
        {
            entity.HasKey(e => e.MaLoai);

            entity.ToTable("LOAI_HV");

            entity.Property(e => e.MaLoai).HasColumnName("MA_LOAI");
            entity.Property(e => e.DienGiai)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("DIEN_GIAI");
        });

        modelBuilder.Entity<Manhthuongquan>(entity =>
        {
            entity.HasKey(e => e.MaMtq);

            entity.ToTable("MANHTHUONGQUAN");

            entity.Property(e => e.MaMtq).HasColumnName("MA_MTQ");
            entity.Property(e => e.DiachiMtq)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("DIACHI_MTQ");
            entity.Property(e => e.DonviTochucMtq)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("DONVI/TOCHUC_MTQ");
            entity.Property(e => e.GioitinhMtq)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GIOITINH_MTQ");
            entity.Property(e => e.HotenMtq)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("HOTEN_MTQ");
            entity.Property(e => e.MatkhauMtq)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MATKHAU_MTQ");
            entity.Property(e => e.SdtMtq).HasColumnName("SDT_MTQ");
        });

        modelBuilder.Entity<Noihotro>(entity =>
        {
            entity.HasKey(e => e.Manoi);

            entity.ToTable("NOIHOTRO");

            entity.Property(e => e.Manoi).HasColumnName("MANOI");
            entity.Property(e => e.AnhNth)
                .HasMaxLength(255)
                .HasColumnName("ANH_NTH");
            entity.Property(e => e.Canhotro)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("CANHOTRO");
            entity.Property(e => e.Diachi)
                .HasMaxLength(500)
                .IsFixedLength()
                .HasColumnName("DIACHI");
            entity.Property(e => e.MaMtq).HasColumnName("MA__MTQ");
            entity.Property(e => e.Tinhtrang)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("TINHTRANG");
            entity.Property(e => e.TrangthaiNht)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("TRANGTHAI_NHT");

            entity.HasOne(d => d.MaMtqNavigation).WithMany(p => p.Noihotros)
                .HasForeignKey(d => d.MaMtq)
                .HasConstraintName("FK_NOIHOTRO_MANHTHUONGQUAN");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("QUYEN");

            entity.Property(e => e.MaCn).HasColumnName("MA_CN");
            entity.Property(e => e.MaCv).HasColumnName("MA_CV");

            entity.HasOne(d => d.MaCnNavigation).WithMany()
                .HasForeignKey(d => d.MaCn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QUYEN_CHUCNANG");

            entity.HasOne(d => d.MaCvNavigation).WithMany()
                .HasForeignKey(d => d.MaCv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QUYEN_CHUCVU");
        });

        modelBuilder.Entity<TaikhoanAdmin>(entity =>
        {
            entity.HasKey(e => e.Taikhoan);

            entity.ToTable("TAIKHOAN_ADMIN");

            entity.Property(e => e.Taikhoan)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("TAIKHOAN");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MATKHAU");
        });

        modelBuilder.Entity<Thanhvien>(entity =>
        {
            entity.HasKey(e => e.MaTv);

            entity.ToTable("THANHVIEN");

            entity.Property(e => e.MaTv).HasColumnName("MA_TV");
            entity.Property(e => e.DiachiTv)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("DIACHI_TV");
            entity.Property(e => e.EmailTv)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("EMAIL_TV");
            entity.Property(e => e.GioitinhTv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("GIOITINH_TV");
            entity.Property(e => e.MaCv).HasColumnName("MA_CV");
            entity.Property(e => e.MatkhauTv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MATKHAU_TV");
            entity.Property(e => e.SdtTv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("SDT_TV");
            entity.Property(e => e.TenTv)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("TEN_TV");

            entity.HasOne(d => d.MaCvNavigation).WithMany(p => p.Thanhviens)
                .HasForeignKey(d => d.MaCv)
                .HasConstraintName("FK_THANHVIEN_CHUCVU");
        });

        modelBuilder.Entity<TtQuyengopHienvat>(entity =>
        {
            entity.HasKey(e => e.MaQghv);

            entity.ToTable("TT_QUYENGOP_HIENVAT");

            entity.Property(e => e.MaQghv).HasColumnName("MA_QGHV");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("GHICHU");
            entity.Property(e => e.MaCd).HasColumnName("MA_CD");
            entity.Property(e => e.MaHv).HasColumnName("MA_HV");
            entity.Property(e => e.MaMtq).HasColumnName("MA_MTQ");
            entity.Property(e => e.MaTv).HasColumnName("MA_TV");
            entity.Property(e => e.NgayQg)
                .HasColumnType("date")
                .HasColumnName("Ngay_QG");
            entity.Property(e => e.SoluongQg).HasColumnName("SOLUONG_QG");
            entity.Property(e => e.TrangthaiHv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("TRANGTHAI_HV");

            entity.HasOne(d => d.MaCdNavigation).WithMany(p => p.TtQuyengopHienvats)
                .HasForeignKey(d => d.MaCd)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TT_QUYENGOP_HIENVAT_CHIENDICH");

            entity.HasOne(d => d.MaHvNavigation).WithMany(p => p.TtQuyengopHienvats)
                .HasForeignKey(d => d.MaHv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TT_QUYENGOP_HIENVAT_HIEN_VAT");

            entity.HasOne(d => d.MaMtqNavigation).WithMany(p => p.TtQuyengopHienvats)
                .HasForeignKey(d => d.MaMtq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TT_QUYENGOP_HIENVAT_MANHTHUONGQUAN");

            entity.HasOne(d => d.MaTvNavigation).WithMany(p => p.TtQuyengopHienvats)
                .HasForeignKey(d => d.MaTv)
                .HasConstraintName("FK_TT_QUYENGOP_HIENVAT_THANHVIEN");
        });

        modelBuilder.Entity<TtTraotang>(entity =>
        {
            entity.HasKey(e => e.MaTt);

            entity.ToTable("TT_TRAOTANG");

            entity.Property(e => e.MaTt).HasColumnName("MA_TT");
            entity.Property(e => e.AnhTt)
                .HasMaxLength(255)
                .HasColumnName("ANH_TT");
            entity.Property(e => e.MaCd).HasColumnName("MA_CD");
            entity.Property(e => e.MaHv).HasColumnName("MA_HV");
            entity.Property(e => e.MaTv).HasColumnName("MA_TV");
            entity.Property(e => e.Manoi).HasColumnName("MANOI");
            entity.Property(e => e.Ngaytang)
                .HasColumnType("date")
                .HasColumnName("NGAYTANG");
            entity.Property(e => e.SoluongTt).HasColumnName("SOLUONG_TT");

            entity.HasOne(d => d.MaCdNavigation).WithMany(p => p.TtTraotangs)
                .HasForeignKey(d => d.MaCd)
                .HasConstraintName("FK_TT_TRAOTANG_CHIENDICH");

            entity.HasOne(d => d.MaHvNavigation).WithMany(p => p.TtTraotangs)
                .HasForeignKey(d => d.MaHv)
                .HasConstraintName("FK_TT_TRAOTANG_HIEN_VAT");

            entity.HasOne(d => d.MaTvNavigation).WithMany(p => p.TtTraotangs)
                .HasForeignKey(d => d.MaTv)
                .HasConstraintName("FK_TT_TRAOTANG_THANHVIEN");

            entity.HasOne(d => d.ManoiNavigation).WithMany(p => p.TtTraotangs)
                .HasForeignKey(d => d.Manoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TT_TRAOTANG_NOIHOTRO1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
