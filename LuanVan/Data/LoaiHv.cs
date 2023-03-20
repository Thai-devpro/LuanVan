using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class LoaiHv
{
    public int MaLoai { get; set; }

    public string? DienGiai { get; set; }

    public virtual ICollection<HienVat> HienVats { get; } = new List<HienVat>();
}
