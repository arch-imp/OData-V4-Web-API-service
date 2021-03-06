﻿using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using WebAPIODataV4.Models;

namespace WebAPIODataV4.Controllers
{
    public class EventDataController : ODataController
    {
        readonly SqliteContext _sqliteContext;

        public EventDataController(SqliteContext sqliteContext)
        {
            _sqliteContext = sqliteContext;
        }

        [EnableQuery(PageSize = 20)]
        public IHttpActionResult Get()
        {
            return Ok(_sqliteContext.EventDataEntities.AsQueryable());
        }

        [EnableQuery(PageSize = 20)]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            return Ok(_sqliteContext.EventDataEntities.Find(key));
        }

        [HttpPost]
        [ODataRoute("EventData")]
        public async Task<IHttpActionResult> CreateEventData(EventData eventData)
        {
            if (eventData != null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _sqliteContext.EventDataEntities.Add(eventData);
            await _sqliteContext.SaveChangesAsync();

            return Created(eventData);
        }

        [HttpPut]
        [ODataRoute("EventData")]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, EventData eventData)
        {
            if (eventData != null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_sqliteContext.EventDataEntities.Any(t => t.EventDataId == eventData.EventDataId && t.EventDataId == key))
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            _sqliteContext.EventDataEntities.AddOrUpdate(eventData);
            await _sqliteContext.SaveChangesAsync();

            return Updated(eventData);
        }

        [HttpPut]
        [ODataRoute("EventData")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<EventData> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_sqliteContext.EventDataEntities.Any(t => t.EventDataId == key))
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            var eventData = _sqliteContext.EventDataEntities.Single(t => t.EventDataId == key);
            delta.Patch(eventData);
            await _sqliteContext.SaveChangesAsync();

            return Updated(eventData);
        }

        [HttpDelete]
        [ODataRoute("EventData")]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var entity = _sqliteContext.EventDataEntities.FirstOrDefault(t => t.EventDataId == key);
            if (entity == null)
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            _sqliteContext.EventDataEntities.Remove(entity);
            await _sqliteContext.SaveChangesAsync();

            return Content(HttpStatusCode.NoContent, "Deleted");
        }

        protected override void Dispose(bool disposing)
        {
            _sqliteContext.Dispose();
            base.Dispose(disposing);
        }
    }
}