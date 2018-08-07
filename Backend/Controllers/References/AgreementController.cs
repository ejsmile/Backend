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
    public class AgreementController : ApiController
    {
        private readonly IAgreementRepository repository;

        public AgreementController(IAgreementRepository repository)
        {
            this.repository = repository;
        }

        // GET api/values
        public IEnumerable<AgreementView> Get()
        {
            var items = repository.GetAll();
            return items.Select(item => new AgreementView()
            {
                Id = item.Id,
                Name = item.Name,
                Number = item.Number,
                BegDate = item.BeginDate,
                EndDate = item.EndDate
            });
        }

        // GET api/values/5
        public AgreementView Get(int id)
        {
            try
            {
                var item = repository.Get(id);
                return new AgreementView()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Number = item.Number,
                    BegDate = item.BeginDate,
                    EndDate = item.EndDate
                };
            }
            catch (AggregateException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        // POST api/values/objects
        public void Post([FromBody]AgreementCreateBindingModel value)
        {
            try
            {
                repository.Create(value.Id, value.Name, value.Number, value.BegDate, value.EndDate, value.Deleted);
            }
            catch (AggregateException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        // PUT api/values/object
        public void Put(int id, [FromBody]AgreementCreateBindingModel value)
        {
            try
            {
                repository.Update(value.Id, value.Name, value.Number, value.BegDate, value.EndDate, value.Deleted);
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

        // POST api/values/objects
        [Route("api/Agreement/CalculationPeriod")]
        public AgreementCalculationPeriodView PostCalculationPeriod([FromBody]AgreementCalculationPeriodBindingModel value)
        {
            try
            {
                var item = repository.Get(value.Id);

                return new AgreementCalculationPeriodView()
                {
                    Id = value.Id,
                    Period = value.Period,
                    BeginDate = value.BeginDate,
                    EndDate = value.EndDate,
                    Consumption = item.СalculationPeriod(value.Period, value.BeginDate, value.EndDate)
                };
            }
            catch (AggregateException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}