using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Manhthuongquan
{
    public int MaMtq { get; set; }

    public string HotenMtq { get; set; } = null!;

    public string GioitinhMtq { get; set; } = null!;

    public string? DonviTochucMtq { get; set; }

    public int? SdtMtq { get; set; }

    public string? DiachiMtq { get; set; }

    public string? MatkhauMtq { get; set; }

    public string? EmailMtq { get; set; }

    public virtual ICollection<Noihotro> Noihotros { get; } = new List<Noihotro>();

    public virtual ICollection<TtQuyengopHienvat> TtQuyengopHienvats { get; } = new List<TtQuyengopHienvat>();
}
