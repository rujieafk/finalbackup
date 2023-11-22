using Microsoft.AspNetCore.Mvc;
using RentHive.Models;

namespace RentHive.Controllers
{
    public class LogRecorder_Controller : Controller
    {
        [HttpPost]
        public async Task<ActionResult> LogWriter(UserDataGetter TempData)
        {
            try
            {
                var userData = HttpContext.Session.GetString("UserData");
                if (string.IsNullOrEmpty(userData))
                {
                    return RedirectToAction("Login", "UserManagement");
                }

                string url = "https://renthive.online/Admin_API/ .php";

                using (var httpClient = new HttpClient())
                {
                    // initializer
                    int AdminID = TempData.AdminID;
                    string rep_user = TempData.Reported_User;
                    string rep_post = TempData.Post_id;
                    int rep_id = TempData.Rep_id;
                    int numHolder = TempData.NumHolder;

                    //date amd time
                    string formattedCurrentDateTime = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper();
                    string origin = "Report Page";
                    string sysResponse = "Success";

                    //making to string
                    var data = new Dictionary<string, string>
                    {
                        {"adminID", AdminID.ToString()},
                        {"reportedUser", rep_user},
                        {"reportedPost", rep_post},
                        {"ReportID", rep_id.ToString()},
                        {"numholder", numHolder.ToString()},
                        {"CurrentDate", formattedCurrentDateTime},
                        {"origin", origin},
                        {"sysResponse", sysResponse}
                    };

                    var content = new FormUrlEncodedContent(data);

                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = "Something went wrong.";
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Successfully Recorded.";
                            return RedirectToAction("Reports", "Home", new { Acc_id = AdminID, ErrorMessage = ViewBag.ErrorMessage });
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "API request failed";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("Handle exceptions error");
            }

            return View();
        }
    }
}
