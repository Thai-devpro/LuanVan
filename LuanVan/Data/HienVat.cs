using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuanVan.Data;

public partial class HienVat
{
    public int MaLoai { get; set; }

    public int MaHv { get; set; }
    [Required( ErrorMessage = "Nhập tên hiện vật")]
    public string? TenHv { get; set; }
    [Required(ErrorMessage = "Nhập đơn vị tính của hiện vật")]
    public string? Donvitinh { get; set; }
    [Required(ErrorMessage = "Nhập số lượng còn của hiện vật")]
    public int? Soluongcon { get; set; }
    [Required(ErrorMessage = "Nhập giá của hiện vật")]
    public int Gia { get; set; }

    public virtual LoaiHv MaLoaiNavigation { get; set; } = null!;

    public virtual ICollection<TtQuyengopHienvat> TtQuyengopHienvats { get; } = new List<TtQuyengopHienvat>();

    public virtual ICollection<TtTraotang> TtTraotangs { get; } = new List<TtTraotang>();
}
