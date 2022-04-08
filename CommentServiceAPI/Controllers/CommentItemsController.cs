#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommentApi.Models;
using CommentServiceAPI;

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

        // GET: api/CommentItems?ReportId=5
        [HttpGet("{GetType}/{ReportId}")]
        public async Task<ActionResult<IEnumerable<CommentItem>>> GetCommentforReportId(string GetType, int ReportId)
        {
            if (GetType == "REPORT")
            {
                var comments = from m in _context.CommentItems
                               select m;

                comments = comments.Where(c => c.ReportId == ReportId);
                List<CommentItem> CommentsList = await comments.ToListAsync();
                return CommentsList;
            }
            return NotFound();  
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
