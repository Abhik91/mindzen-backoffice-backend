namespace MindzenBackofficeDatabaseLibrary.DataModels
{
    public class PractitionerProfileDbModel
    {
        public string? UserID { get; set; }
        public string? ProfessionID { get; set; }
        public string? PractitionerName { get; set; }
        public string? Profession { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PracticingSince { get; set; }
        public string? ProfilePicPath { get; set; }
        public string? AboutMe { get; set; }
        public string? RciLicenseNumber { get; set; }
        public string? EducationalDegree { get; set; }

        public double SingleSessionCharge { get; set; }
        public double TwoSessionCharge { get; set; }
        public double FourSessionCharge { get; set; }

        public string? AgeGroup { get; set; }
        public bool IsIndividual { get; set; }
        public bool IsCouple { get; set; }
        public bool IsFamily { get; set; }
        public bool IsListed { get; set; }

        public List<string> Specializations { get; set; } = [];
        public List<string> Top3Specializations { get; set; } = [];
        public List<string> Languages { get; set; } = [];
        public List<PractitionerDocumentModel> Documents { get; set; } = [];
    }

    public class PractitionerDocumentModel
    {
        public string? DocumentType { get; set; }
        public string? DocumentPath { get; set; }
    }

    public class PractitionerFeatureDetailsDbModel
    {
        public string? UserID { get; set; }
        public string? FullName { get; set; }
        public string? EmailAddress { get; set; }
        public string? ProfilePicPath { get; set; }
        public string? PractitionerProfession { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PracticingSince { get; set; }
        public int ExperienceYears { get; set; }
        public int ExperienceMonths { get; set; }
        public bool IsListed { get; set; }
        public bool IsFeatured { get; set; }
        public int? FeaturedOrder { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public decimal SingleSessionCharge { get; set; }
    }

    public class PractitionerSpecializationsModel
    {
        public string? SpecializationID { get; set; }
    }

    public class PractitionerSpeakingLanguageModel
    {
        public string? LanguageID { get; set; }
    }
}
