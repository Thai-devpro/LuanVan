using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class TtQuyengopHienvat
{
    public int MaQghv { get; set; }

    public int MaHv { get; set; }

    public int MaMtq { get; set; }

    public int? MaCd { get; set; }

    public string? Ghichu { get; set; }

    public int SoluongQg { get; set; }

    public string TrangthaiHv { get; set; } = null!;

    public DateTime? NgayQg { get; set; }

    public int? MaTv { get; set; }

    public virtual Chiendich? MaCdNavigation { get; set; }

    public virtual HienVat MaHvNavigation { get; set; } = null!;

    public virtual Manhthuongquan MaMtqNavigation { get; set; } = null!;

    public virtual Thanhvien? MaTvNavigation { get; set; }
}
