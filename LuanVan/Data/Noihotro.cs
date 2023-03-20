using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Noihotro
{
    public int Manoi { get; set; }

    public int MaMtq { get; set; }

    public string Diachi { get; set; } = null!;

    public string Tinhtrang { get; set; } = null!;

    public string Canhotro { get; set; } = null!;

    public string? TrangthaiNht { get; set; }

    public int? MaTv { get; set; }

    public string? AnhNth { get; set; }

    public virtual ICollection<Chiendich> Chiendiches { get; } = new List<Chiendich>();

    public virtual Manhthuongquan MaMtqNavigation { get; set; } = null!;

    public virtual Thanhvien? MaTvNavigation { get; set; }

    public virtual ICollection<TtTraotang> TtTraotangs { get; } = new List<TtTraotang>();
}
