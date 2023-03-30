using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class TtTraotang
{
    public int MaTt { get; set; }

    public int Manoi { get; set; }

    public int? MaHv { get; set; }

    public int? MaCd { get; set; }

    public int SoluongTt { get; set; }

    public DateTime Ngaytang { get; set; }

    public string? AnhTt { get; set; }

    public int? MaTv { get; set; }

    public virtual Chiendich? MaCdNavigation { get; set; }

    public virtual HienVat? MaHvNavigation { get; set; }

    public virtual Thanhvien? MaTvNavigation { get; set; }

    public virtual Noihotro ManoiNavigation { get; set; } = null!;
}
