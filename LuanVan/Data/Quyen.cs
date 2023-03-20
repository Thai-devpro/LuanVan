using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Quyen
{
    public int MaCv { get; set; }

    public int MaCn { get; set; }

    public virtual Chucnang MaCnNavigation { get; set; } = null!;

    public virtual Chucvu MaCvNavigation { get; set; } = null!;
}
