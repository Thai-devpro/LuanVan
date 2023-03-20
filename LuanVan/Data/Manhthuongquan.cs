using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Manhthuongquan
{
    public int MaMtq { get; set; }

    public string HotenMtq { get; set; } = null!;

    public DateTime NgaysinhMtq { get; set; }

    public string GioitinhMtq { get; set; } = null!;

    public string? DonviTochucMtq { get; set; }

    public int SdtMtq { get; set; }

    public string? DiachiMtq { get; set; }

    public virtual ICollection<Noihotro> Noihotros { get; } = new List<Noihotro>();

    public virtual TaikhoanMtq? TaikhoanMtq { get; set; }

    public virtual ICollection<TtQuyengopHienvat> TtQuyengopHienvats { get; } = new List<TtQuyengopHienvat>();
}
