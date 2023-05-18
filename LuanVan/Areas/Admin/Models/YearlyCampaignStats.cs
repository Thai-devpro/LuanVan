namespace LuanVan.Areas.Admin.Models
{
    public class YearlyCampaignStats
    {
        public int Year { get; set; }
        public int[] MonthlyStats { get; set; } = new int[12];
    }
}
