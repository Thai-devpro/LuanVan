using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Chucnang
{
    public int MaCn { get; set; }

    public string? TenCn { get; set; }

    public virtual ICollection<Quyen> Quyens { get; } = new List<Quyen>();
}
