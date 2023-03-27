using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Thanhvien
{
    public int MaTv { get; set; }

    public string? TenTv { get; set; }

    public string? GioitinhTv { get; set; }

    public string? DiachiTv { get; set; }

    public string? SdtTv { get; set; }

    public string? EmailTv { get; set; }

    public string? MatkhauTv { get; set; }

    public int? MaCv { get; set; }

    public virtual ICollection<Chiendich> Chiendiches { get; } = new List<Chiendich>();

    public virtual Chucvu? MaCvNavigation { get; set; }

    public virtual ICollection<TtQuyengopHienvat> TtQuyengopHienvats { get; } = new List<TtQuyengopHienvat>();

    public virtual ICollection<TtTraotang> TtTraotangs { get; } = new List<TtTraotang>();
}
