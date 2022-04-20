namespace CommentServiceAPI
{
    public class CommentItem
    {
        public int Id { get; set; }

        public string CreatedBy { get; set; }  // changed to string due to issues with GUID

        public string CommentText { get; set; } = string.Empty;

        public int ReportId { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
