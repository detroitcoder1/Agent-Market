using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AgentMarket.Models;

namespace AgentMarket.Controllers.API
{
    public class DynamicMenuDTO
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<ItemDTO> Items { get; set; }
    }

    public class ItemDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
    }

    [Authorize(Roles="SuperModerator")]
    public class DynamicMenuController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET api/DynamicMenu
        [AllowAnonymous]
        public IEnumerable<DynamicMenuDTO> GetDynamicMenuItems()
        {
            return db.DynamicMenuItems.Select(x => new DynamicMenuDTO { Id = x.Id, Title = x.Title }).AsEnumerable();
        }

        // GET api/DynamicMenu/5
        [ResponseType(typeof(DynamicMenuDTO))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetDynamicMenuItem(short id)
        {
            DynamicMenuItem dynamicmenuitem = await db.DynamicMenuItems.FindAsync(id);
            if (dynamicmenuitem == null)
            {
                return NotFound();
            }

            DynamicMenuDTO dto = new DynamicMenuDTO { Id = dynamicmenuitem.Id, Title = dynamicmenuitem.Title };
            dto.Items = new List<ItemDTO>();
            (dto.Items as List<ItemDTO>).AddRange(dynamicmenuitem.Items.Select(x => new ItemDTO { Id = x.Id, Description = x.Description, PostDate = x.PostDate, Title = x.Title, Image = x.Image, Thumbnail = x.Thumbnail, Content = x.Content }));

            return Ok(dto);
        }

        // PUT api/DynamicMenu/5
        public async Task<IHttpActionResult> PutDynamicMenuItem(short id, DynamicMenuItem dynamicmenuitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dynamicmenuitem.Id)
            {
                return BadRequest();
            }

            db.Entry(dynamicmenuitem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DynamicMenuItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/DynamicMenu
        [ResponseType(typeof(DynamicMenuItem))]
        public async Task<IHttpActionResult> PostDynamicMenuItem(DynamicMenuItem dynamicmenuitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DynamicMenuItems.Add(dynamicmenuitem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = dynamicmenuitem.Id }, dynamicmenuitem);
        }

        // DELETE api/DynamicMenu/5
        [ResponseType(typeof(DynamicMenuItem))]
        public async Task<IHttpActionResult> DeleteDynamicMenuItem(short id)
        {
            DynamicMenuItem dynamicmenuitem = await db.DynamicMenuItems.FindAsync(id);
            if (dynamicmenuitem == null)
            {
                return NotFound();
            }

            db.DynamicMenuItems.Remove(dynamicmenuitem);
            await db.SaveChangesAsync();

            return Ok(dynamicmenuitem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DynamicMenuItemExists(short id)
        {
            return db.DynamicMenuItems.Count(e => e.Id == id) > 0;
        }
    }
}