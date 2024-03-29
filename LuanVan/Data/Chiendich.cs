﻿using System;
using System.Collections.Generic;

namespace LuanVan.Data;

public partial class Chiendich
{
    public int MaCd { get; set; }

    public string TenCd { get; set; } = null!;

    public DateTime Ngaybatdau { get; set; }
    public string NgaybatdauFormatted
    {
        get { return Ngaybatdau.ToString("dd/MM/yyyy"); }
    }
    public DateTime Ngayketthuc { get; set; }
    public string NgayKetThucFormatted
    {
        get { return Ngayketthuc.ToString("dd/MM/yyyy"); }
    }

    public string NoidungCd { get; set; } = null!;

    public string? AnhCd { get; set; }

    public int? MaTv { get; set; }

    public int? MaNoi { get; set; }

    public virtual Noihotro? MaNoiNavigation { get; set; }

    public virtual Thanhvien? MaTvNavigation { get; set; }

    public virtual ICollection<TtQuyengopHienvat> TtQuyengopHienvats { get; } = new List<TtQuyengopHienvat>();

    public virtual ICollection<TtTraotang> TtTraotangs { get; } = new List<TtTraotang>();
}
