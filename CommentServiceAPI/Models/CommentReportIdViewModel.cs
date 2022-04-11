using Microsoft.AspNetCore.Mvc.Rendering;
// Purpose - to support return a list of Comments with filtering:
//   all users or the logged in user reports only
//   All comments for a report
//   


namespace CommentServiceAPI.Models
{
    internal class CommentReportIdViewModel
    {
        public SelectList CommentsReportId { get; set; }
        public List<CommentItem> Comments { get; set; }

        public string? CreatedBy { get; set; }
        public int? ReportID { get; set; }  //BC - amended type to int from string
    }
}