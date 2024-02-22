namespace SeminarHub.Data
{
    public static class DataConstants
    {
        public static class SeminarConstants
        {
            //Topic
            public const int TopicMaxLength = 100;
            public const int TopicMinLength = 3;

            //Lecturer
            public const int LecturerMaxLength = 60;
            public const int LecturerMinLength = 5;

            //Details
            public const int DetailsMaxLength = 500;
            public const int DetailsMinLength = 10;

            //Duration
            public const int DurationMaxLength = 180;
            public const int DurationMinLength = 30;
        }
        public static class CategoryConstants
        {
            //Name
            public const int NameMaxLength = 50;
            public const int NameMinLength = 3;
        }

        public const string DataFormat = "dd/MM/yyyy HH:mm";
    }
}
