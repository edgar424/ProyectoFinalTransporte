﻿using Cibertec.Models;
using Cibertec.UnitOfWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Cibertec.WebApi.Controllers
{
    [RoutePrefix("customer")]
    public class CustomerController : BaseController
    {
        public CustomerController(IUnitOfWork unit, ILog log) : base(unit,log)
        {
            _log.Info($"{typeof(CustomerController)} in execution");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("error")]
        public IHttpActionResult CreateError()
        {
            throw new System.Exception("This is an unhandled error.");
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (id == "" || id == null) return BadRequest();
            return Ok(_unit.Customers.GetById(id));
        }
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(Customers customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = _unit.Customers.Insert(customer);
            return Ok(new { id = id });
        }

        [Route("")]
        [HttpPut]
        public IHttpActionResult Put(Customers customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_unit.Customers.Update(customer)) return BadRequest(ModelState);
            return Ok(new { status = true });
        }
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            if (id == "" || id == null) return BadRequest();
            //try {
            var result = _unit.Customers.Delete(id);
            //}
            //catch
            //{
            //}
            return Ok(new { delete = true });
        }
        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetList()
        {
            return Ok(_unit.Customers.GetList());
        }
        /*Servicio a ser consumido por el proyecto MVC:*/
        //[HttpGet]
        //[Route("list/{startRecord}/{endRecord}")]
        //public IHttpActionResult GetListPagination(int startRecord, int endRecord)
        //{
        //    return Ok(_unit.Customers.PagedList(startRecord, endRecord));
        //}

        [HttpGet]
        [Route("list/{page:int}/{rows:int}")]
        public IHttpActionResult GetList(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return Ok(new List<Customers>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;


            return Ok(_unit.Customers.PagedList(startRecord, endRecord));
        }
    }
}