namespace MindzenBackendBackOffice.Models
{
    public class UpdateFeaturedStatusRequest
    {
        public string? UserID { get; set; }
        public bool IsFeatured { get; set; }
    }

    public class FeaturedPractitionerOrderItem
    {
        public string? UserID { get; set; }
        public int FeaturedOrder { get; set; }
    }

    public class UpdateFeaturedOrderRequest
    {
        public List<FeaturedPractitionerOrderItem>? Practitioners { get; set; }
    }
}
