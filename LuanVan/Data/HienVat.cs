using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class HienVat
{
    public int MaLoai { get; set; }

    public int MaHv { get; set; }

    public string? TenHv { get; set; }

    public string? Donvitinh { get; set; }

    public int? Soluongcon { get; set; }

    public int Gia { get; set; }

    public virtual LoaiHv MaLoaiNavigation { get; set; } = null!;

    public virtual ICollection<TtQuyengopHienvat> TtQuyengopHienvats { get; } = new List<TtQuyengopHienvat>();

    public virtual ICollection<TtTraotang> TtTraotangs { get; } = new List<TtTraotang>();
}
