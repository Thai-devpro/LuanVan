using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class TaikhoanMtq
{
    public int MaMtq { get; set; }

    public string? MatkhauMtq { get; set; }

    public string? Taikhoan { get; set; }

    public virtual Manhthuongquan MaMtqNavigation { get; set; } = null!;
}
