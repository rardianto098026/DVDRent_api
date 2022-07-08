using DVDRent_api.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace DVDRent_api.Controllers
{
    [RoutePrefix("api/CustDVDRent")]
    public class CustDVDRentController : ApiController
    {
        [HttpGet]
        [Route("GetMovie")]
        public JsonResult<DataTable> GetMovie()
        {
            DataTable dtCust = new DataTable();
            try
            {
                string query = "EXEC [SP_GET_MOVIE_CUST]";
                dtCust = Common.ExecuteQuery(query);
                return Json(dtCust);
            }
            catch (Exception ex)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(Convert.ToString(ex.Message))
                };
                throw new HttpResponseException(message);
            }
        }
    }
}
