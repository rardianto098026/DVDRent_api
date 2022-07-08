using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DVDRent_api.Repository;
using System.Web.Http.Results;

namespace DVDRent_api.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        [HttpPost]        
        [Route("toLogin")]
        public HttpResponseMessage toLogin([FromBody] user_class user_class)
        {
            DataTable dtlogin = new DataTable();
            bool verified;
            try
            {
                //string query = "SELECT * FROM [user] where email = '" + user_class.email + "' and RoleID = "+ user_class.role +"";
                string query = "EXEC SP_LOAD_USER @ID_USER = NULL, @EMAIL = '" + user_class.email + "', @ROLE = '1'";
                dtlogin = Common.ExecuteQuery(query);
                try
                {
                    if(dtlogin.Rows.Count == 0)
                    {
                        query = "EXEC SP_LOAD_USER @ID_USER = NULL, @EMAIL = '" + user_class.email + "', @ROLE = '"+ user_class.role +"'";
                        dtlogin = Common.ExecuteQuery(query);
                    }
                    verified = BCrypt.Net.BCrypt.Verify(user_class.password, dtlogin.Rows[0]["password"].ToString());
                    if (!verified)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "incorrect username or password");
                    }

                    string firstname = dtlogin.Rows[0]["firstname"].ToString();
                    if (firstname != "")
                    {
                        var message2 = Request.CreateResponse(HttpStatusCode.OK, dtlogin.Rows[0]["firstname"]);
                        return message2;
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, dtlogin.Rows[0]["firstname1"]);
                    return message;
                }
                catch (Exception ex)
                {
                    string eror = Convert.ToString(ex);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
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
        [Route("AddEmployee")]
        public HttpResponseMessage AddEmployee([FromBody] add_employee emp)
        {
            try
            {
                emp.password = BCrypt.Net.BCrypt.HashPassword(emp.password);
                string query = "EXEC SP_ADD_EMPLOYEE @firstname = '" + emp.firstname + "', @lastname = '" + emp.lastname + "', @email = '" + emp.email + "' , @phone = '" + emp.phone + "' , @address = '" + emp.address + "' , @address2 = '" + emp.address2 + "' , @gender = '" + emp.gender + "' , @password = '" + emp.password + "' , @store = '" + emp.store + "' , @position = '" + emp.position +"', @picture_URL = '"+ emp.picture_URL +"'";
                Common.ExecuteNonQuery(query);
                var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                message.Headers.Location = new Uri(Request.RequestUri + emp.firstname.ToString());
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

        [Route("Register")]
        public HttpResponseMessage Register([FromBody] register_user regis)
        {
            DataTable dtlogin = new DataTable();
            try
            {
                string query = "select * from [user] where EMAIL = '" + regis.email + "'";
                DataTable dtcheck = Common.ExecuteQuery(query);
                if(dtcheck.Rows.Count > 0) {
                    var message2 = Request.CreateResponse(HttpStatusCode.Found);
                    return message2;
                }
                query = "EXEC SP_REGISTER_CUST @FIRSTNAME = '"+ regis.firstname + "', @LASTNAME = '"+ regis.lastname +"', @PASSWORD = '"+ regis.password +"', @EMAIL = '"+ regis.email +"'";
                Common.ExecuteNonQuery(query);
                
                var message = Request.CreateResponse(HttpStatusCode.OK);
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

        [HttpGet]
        [Route("LoadUser")]
        public JsonResult<DataTable> loadUser(string id)
        {
            DataTable dtCust = new DataTable();
            try
            {
                string query = "EXEC SP_LOAD_USER @ID_USER = '" + id + "'";
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

        [HttpGet]
        [Route("LoadDDLstore")]
        public JsonResult<DataTable> LoadDDLstore()
        {
            DataTable dtStore = new DataTable();
            try
            {
                string query = "select * from store";
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
        [Route("LoadDDLposition")]
        public JsonResult<DataTable> LoadDDLposition()
        {
            DataTable dtPosition = new DataTable();
            try
            {
                string query = "select * from position";
                dtPosition = Common.ExecuteQuery(query);
                return Json(dtPosition);
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

        public class user_class
        {
            public string email { get; set; }
            public string password { get; set; }
            public int role { get; set; }
        }

        public class register_user
        {
            public string email { get; set; }
            public string password { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
        }

        public class add_employee
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string address { get; set; }
            public string picture_URL { get; set; }            
            public string address2 { get; set; }
            public string gender { get; set; }
            public string password { get; set; }
            public string store { get; set; }
            public string position { get; set; }
        }
    }
}
