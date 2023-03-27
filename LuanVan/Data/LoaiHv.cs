using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuanVan.Data;

public partial class LoaiHv
{
    public int MaLoai { get; set; }
    [Required( ErrorMessage = "Vui lòng nhập diễn giải")]
    public string? DienGiai { get; set; }

    public virtual ICollection<HienVat> HienVats { get; } = new List<HienVat>();
}
