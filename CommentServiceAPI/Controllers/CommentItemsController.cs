#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommentApi.Models;
using CommentServiceAPI.Models;
using CommentServiceAPI;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CommentServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentItemsController : ControllerBase
    {
        private readonly CommentContext _context;

        public CommentItemsController(CommentContext context)
        {
            _context = context;
        }

        // GET: api/CommentItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentItem>>> GetCommentItems()
        {
            return await _context.CommentItems.ToListAsync();
        }

        // Purpose - return a list of Comments:
        //   all users or the logged in user reports only
        //   All comments for a report
        //   

        // GET: api/CommentItems/FilterComments?CreationEmail?={CreationEmail}&ReportID?={reportID}
        [HttpGet("FilterComments")]
        public async Task<ActionResult<IEnumerable<CommentItem>>> GetCommentItems([FromQuery]string CreationEmail, [FromQuery]string reportID)
        {

            // Use LINQ to get list of genres.
            IQueryable<string> ReportIDQuery = from m in _context.CommentItems
                                               orderby m.CreatedBy
                                               select m.CreatedBy;

            var comments = from m in _context.CommentItems
                          select m;

            if (!string.IsNullOrEmpty(CreationEmail))
            {
                comments = comments.Where(s => s.CreatedBy == CreationEmail);
            }

 
            if (!string.IsNullOrEmpty(reportID))
            {

                if (Int32.TryParse(reportID, out int reportid))
                {
                    comments = comments.Where(x => x.ReportId == reportid);
                }
            }

            var CommentReportIdVM = new CommentReportIdViewModel
            {
                CommentsReportId = new SelectList(await ReportIDQuery.Distinct().ToListAsync()),
                Comments = await comments.ToListAsync()
            };

            return CommentReportIdVM.Comments;

        }


        // GET: api/CommentItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentItem>> GetCommentItem(int id)
        {
            var commentItem = await _context.CommentItems.FindAsync(id);

            if (commentItem == null)
            {
                return NotFound();
            }

            return commentItem;
        }


        // PUT: api/CommentItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommentItem(int id, CommentItem commentItem)
        {
            if (id != commentItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(commentItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CommentItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommentItem>> PostCommentItem(CommentItem commentItem)
        {
            _context.CommentItems.Add(commentItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommentItem", new { id = commentItem.Id }, commentItem);
        }

        // DELETE: api/CommentItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentItem(int id)
        {
            var commentItem = await _context.CommentItems.FindAsync(id);
            if (commentItem == null)
            {
                return NotFound();
            }

            _context.CommentItems.Remove(commentItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //// DELETE: api/CommentItems?ReportId=5
        //[HttpDelete("{reportID}")]
        //public async Task<IActionResult> DeleteCommentsbyReportID(string ReportID) 
        //{
        //    int reportid = ReportID
        //    var comments = from m in _context.CommentItems
        //                   select m;

        //    comments = comments.Where(c => c.ReportId == reportID);
        //    List<CommentItem> CommentsList = await comments.ToListAsync();
        //    if (CommentsList == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CommentItems.Remove(CommentsList);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool CommentItemExists(int id)
        {
            return _context.CommentItems.Any(e => e.Id == id);
        }
    }
}
