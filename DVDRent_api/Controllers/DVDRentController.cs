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
        [HttpGet]
        [Route("GetDetailMovie")]
        public JsonResult<DataTable> GetDetailMovie(int id)
        {
            DataTable dtMovie = new DataTable();
            try
            {
                string query = "SELECT * FROM Movie A LEFT JOIN MovieGenre B ON A.Id = B.MovieID where A.ID = "+ id +"";
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

        [HttpGet]
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

        [HttpGet]
        [Route("LoadDDLmovie")]
        public JsonResult<DataTable> LoadDDLmovie()
        {
            DataTable dtStore = new DataTable();
            try
            {
                string query = "select * from Movie";
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

        [HttpGet]
        [Route("LoadDDLstatusMovie")]
        public JsonResult<DataTable> LoadDDLstatusMovie()
        {
            DataTable dtStore = new DataTable();
            try
            {
                string query = "select * from Status";
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

        [HttpPost]
        [Route("AddMovie")]
        public HttpResponseMessage AddMovie([FromBody] AddModifyMovie mov)
        {
            try
            {
                string query = "EXEC [SP_ADD_MOVIE] '"+ mov.Title + "', '" + mov.Description + "', '" + mov.ReleaseYear + "', '" + mov.Duration + "', '" + mov.AgeRating + "', '" + mov.PictureURL + "', '" + mov.TrailerURL + "'";
                DataTable getID = Common.ExecuteQuery(query);

                foreach(var genreItem in mov.Genre)
                {
                    query = "EXEC [SP_ADD_MOVIE_GENRE] '" + getID.Rows[0][0].ToString() + "', '" + genreItem + "'";
                    Common.ExecuteNonQuery(query);
                }
                
                var message = Request.CreateResponse(HttpStatusCode.Created, mov);
                message.Headers.Location = new Uri(Request.RequestUri + mov.Title.ToString());

                return message;
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
        [Route("UpdateMovie")]
        public HttpResponseMessage UpdateMovie(int mov_id, [FromBody] AddModifyMovie mov)
        {
            try
            {
                string query = "EXEC [SP_UPDATE_MOVIE] '" + mov.Title + "', '" + mov.Description + "', '" + mov.ReleaseYear + "', '" + mov.Duration + "', '" + mov.AgeRating + "', '" + mov.PictureURL + "', '" + mov.TrailerURL + "', '"+ mov_id + "'";
                Common.ExecuteNonQuery(query);

                foreach (var genreItem in mov.TambahGenre)
                {
                    query = "EXEC [SP_UPDATE_MOVIE_GENRE] '" + mov_id + "', '" + genreItem + "', 'TAMBAH'";
                    Common.ExecuteNonQuery(query);
                }

                foreach (var genreItem in mov.KurangGenre)
                {
                    query = "EXEC [SP_UPDATE_MOVIE_GENRE] '" + mov_id + "', '" + genreItem + "', 'KURANG'";
                    Common.ExecuteNonQuery(query);
                }

                var message = Request.CreateResponse(HttpStatusCode.Created, mov);
                message.Headers.Location = new Uri(Request.RequestUri + mov.Title.ToString());

                return message;
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
        public class AddModifyMovie
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ReleaseYear { get; set; }
            public string Duration { get; set; }
            public string[] Genre { get; set; }
            public string[] TambahGenre { get; set; }
            public string[] KurangGenre { get; set; }
            public string AgeRating { get; set; }
            public string PictureURL { get; set; }
            public string TrailerURL { get; set; }
        }
    }
}
