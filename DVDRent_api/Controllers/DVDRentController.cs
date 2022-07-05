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
    [RoutePrefix("api/DVDRent")]
    public class DVDRentController : ApiController
    {
        //[HttpGet]
        //public DataTable GetTrending()
        //{
        //    DataTable dtTrending = new DataTable();
        //    try
        //    {
        //        string query = "SELECT * FROM [user] where email = '" + user_class.email + "'";
        //        dtlogin = Common.ExecuteQuery("SELECT * FROM [user] where email = '" + user_class.email + "'");
        //        try
        //        {
        //            verified = BCrypt.Net.BCrypt.Verify(user_class.password, dtlogin.Rows[0]["password"].ToString());
        //            if (!verified)
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "incorrect username or password");
        //            }
        //            var message = Request.CreateResponse(HttpStatusCode.OK);
        //            return message;
        //        }
        //        catch (Exception ex)
        //        {
        //            string eror = Convert.ToString(ex);
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
        //        {
        //            Content = new StringContent(Convert.ToString(ex.Message))
        //        };
        //        throw new HttpResponseException(message);
        //    }
        //}
        [HttpGet]
        [Route("GetCustomer")]
        public JsonResult<DataTable> GetCustomer()
        {
            DataTable dtCust = new DataTable();
            try
            {
                string query = "EXEC SP_GET_CUST";
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
        [Route("GetAdmin")]
        public JsonResult<DataTable> GetAdmin()
        {
            DataTable dtCust = new DataTable();
            try
            {
                string query = "EXEC SP_GET_ADMIN";
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
        [Route("GetMovie")]
        public JsonResult<DataTable> GetMovie()
        {
            DataTable dtMovie = new DataTable();
            try
            {
                string query = "EXEC SP_GET_MOVIE_ADMIN";
                dtMovie = Common.ExecuteQuery(query);
                return Json(dtMovie);
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

        [HttpPut]
        [Route("UpdateStatusCustomer")]
        public HttpResponseMessage Put(string id, string status)
        {
            try
            {
                Common.ExecuteQuery(@"EXEC SP_CHANGE_STATUS_CUST @ID_CUST = '"+ id +"', @STATUS = '"+ status +"'");

                var message = Request.CreateResponse(HttpStatusCode.OK, "Berhasil diupdate");
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpDelete]
        [Route("DeleteAdmin")]
        public HttpResponseMessage delAdmin(string id)
        {
            try
            {
                Common.ExecuteQuery(@"EXEC [SP_DEL_ADMIN] @ID_ADM = '" + id + "'");

                var message = Request.CreateResponse(HttpStatusCode.OK, "Berhasil dihapus");
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("LoadDDLagerating")]
        public JsonResult<DataTable> LoadDDLagerating()
        {
            DataTable dtStore = new DataTable();
            try
            {
                string query = "select * from AgeRating";
                dtStore = Common.ExecuteQuery(query);
                return Json(dtStore);
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

        [Route("LoadGenre")]
        public JsonResult<DataTable> LoadGenre()
        {
            DataTable dtStore = new DataTable();
            try
            {
                string query = "select * from Genre";
                dtStore = Common.ExecuteQuery(query);
                return Json(dtStore);
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
