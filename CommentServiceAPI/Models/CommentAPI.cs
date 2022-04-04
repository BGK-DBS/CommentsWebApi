namespace CommentServiceAPI
{
    public class CommentItem
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        public string Comment { get; set; } = string.Empty;
    }
}
