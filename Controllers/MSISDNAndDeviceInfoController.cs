using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Utility;
using System.Net.Http.Headers;
using System.Text;
using MySqlX.XDevAPI.Common;

namespace SQMS.Controllers
{
    [AuthorizationFilter]
    public class MSISDNAndDeviceInfoController : Controller
    {
        private readonly BLLMSISDNAndDeviceInfo dbManager = new BLLMSISDNAndDeviceInfo();
        private readonly IHttpContextAccessor _session;

        public MSISDNAndDeviceInfoController(IHttpContextAccessor session)
        {
            _session = session;
        }
        public byte[] GetGenericJsonData<T>(T obj)
        {
            try
            {
                string result = "";
                var content2 = JsonConvert.SerializeObject(obj);
                result = content2.ToString();
                byte[] bytedata = Encoding.ASCII.GetBytes(result);
                return bytedata;
            }
            catch (Exception ex)
            {   
                string message = ex.Message;
                byte[] error = Encoding.ASCII.GetBytes(message);
                return error;
            }
        }

        [HttpGet]
        [ActionName("dfp-info")]
        public IActionResult GetNotification(string msisdn)
        {
            VMDeviceLoanEligiblility loanEligiblility = new VMDeviceLoanEligiblility();
            try
            {
                loanEligiblility = dbManager.GetDeviceLoanInfoByMSISDN(msisdn);

            }
            catch (Exception ex)
            {
                return Ok(new { data = loanEligiblility });
            }
            return Ok(new { data = loanEligiblility });
        }


        [HttpGet]
        [ActionName("handset-category")]
        public async Task<IActionResult> GetHandsetCategory(string msisdn)
        {
            VMHandsetLog handsetLog = new VMHandsetLog();
            string handsetCategory = "Not Found";

            try
            {
                string baseURL = ApplicationSetting.ADMAPI + "api/";
                //string baseURL = "http://10.13.49.116/api/";
                handsetLog.msisdn = msisdn;
                using (var client = new HttpClient())
                {
                    var requestObject = new
                    {
                        msisdn = msisdn
                    };

                    byte[] req_data = GetGenericJsonData(requestObject);

                    handsetLog.request_data = req_data;

                    //Passing service base url
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    handsetLog.request_date = DateTime.Now;
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync($"subscribers/{msisdn}?show=device_model,device_latest_supported_data_technology,device_latest_supported_generation&username=sqmsbss&password=sqmsbss");

                    handsetLog.response_date = DateTime.Now;

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        var responseData = GetGenericJsonData(EmpResponse);
                        handsetLog.response_data = responseData;

                        JObject responseResult = JsonConvert.DeserializeObject<JObject>(EmpResponse);

                        if (responseResult["subscriber"] != null)
                        {
                            //Deserializing the response recieved from web api and storing into the Employee list
                            handsetLog.api_status = 1;
                            handsetLog.message = "API Status OK";
                            if (responseResult["subscriber"]["device"] != null)
                            {
                                handsetCategory = responseResult["subscriber"]["device"]["latest_supported_generation"].ToString() + "/" + responseResult["subscriber"]["device"]["latest_supported_data_technology"].ToString();
                            }
                        }
                    }
                    {
                        handsetLog.api_status = 0;
                        handsetLog.message = Res.StatusCode.ToString();
                    }

                    dbManager.InsertHandsetLog(handsetLog);
                    //returning the employee list to view
                    //return Json(new { data = handsetCategory }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                handsetLog.response_data = null;
                handsetLog.api_status = 3;
                handsetLog.message = ex.Message;
                dbManager.InsertHandsetLog(handsetLog);
            }
            return Ok(new { data = handsetCategory });
        }
    }
}
