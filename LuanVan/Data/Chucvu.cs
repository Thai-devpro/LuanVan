using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Chucvu
{
    public int MaCv { get; set; }

    public string? TenCv { get; set; }

    public virtual ICollection<Quyen> Quyens { get; } = new List<Quyen>();

    public virtual ICollection<Thanhvien> Thanhviens { get; } = new List<Thanhvien>();
}
