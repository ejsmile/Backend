using ApiContracts.References.Agreement;
using Domain.Interface.Repositories.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Backend.Controllers.References
{
    [Authorize]
    public class TariffController : ApiController
    {
        private readonly ITariffRepository repository;

        public TariffController(ITariffRepository repository)
        {
            this.repository = repository;
        }

        // GET api/values
        public IEnumerable<TariffView> Get()
        {
            var items = repository.GetAll();
            return items.Select(item => new TariffView()
            {
                Id = item.Id,
                Name = item.Name,
                Category = item.Category,
                Price = item.Price,
                Deleted = item.Deleted
            });
        }

        // GET api/values/5
        public TariffView Get(int id)
        {
            try
            {
                var item = repository.Get(id);
                return new TariffView()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Category = item.Category,
                    Price = item.Price,
                    Deleted = item.Deleted
                };
            }
            catch (AggregateException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        // POST api/values/objects
        public void Post([FromBody]TariffCreateBindingModel value)
        {
            try
            {
                repository.Create(value.Id, value.Name, value.Category, value.Price, value.Deleted);
            }
            catch (AggregateException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        // PUT api/values/object
        public void Put(int id, [FromBody]TariffCreateBindingModel value)
        {
            try
            {
                repository.Update(value.Id, value.Name, value.Category, value.Price, value.Deleted);
            }
            catch (AggregateException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            try
            {
                repository.Delete(id);
            }
            catch (AggregateException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
    }
}