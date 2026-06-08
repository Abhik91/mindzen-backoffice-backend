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

    public class PractitionerProfileRequest
    {
        public string? UserID { get; set; }
        public string? ProfessionID { get; set; }
        public string? ProfilePicPath { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PracticingSince { get; set; }
        public string? AboutMe { get; set; }
        public string? SingleSessionCharge { get; set; }
        public string? TwoSessionCharge { get; set; }
        public string? FourSessionCharge { get; set; }

        public string? AgeGroup { get; set; }
        public bool IsIndividual { get; set; }
        public bool IsCouple { get; set; }
        public bool IsFamily { get; set; }

        public List<string> Specializations { get; set; } = [];
        public List<string> Top3Specializations { get; set; } = [];
        public List<string> Languages { get; set; } = [];

        public List<DocumentDto> Documents { get; set; } = [];
    }

    public class DocumentDto
    {
        public string? DocumentType { get; set; }
        public string? DocumentPath { get; set; }
    }
}
