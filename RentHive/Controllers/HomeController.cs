using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentHive.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace RentHive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult WelcomePage()
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                /*// User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");*/
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url1 = "https://renthive.online/Admin_API/HomeViewer.php";
            string url2 = "https://renthive.online/Admin_API/CountUser.php";
            string url3 = "https://renthive.online/Admin_API/CountAdmin.php";
            try
            {
                var userObject = JsonConvert.DeserializeObject<UserDataGetter>(userData);
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                    {
                        {"userId", userObject.Acc_id.ToString()}
                    };

                    var content = new FormUrlEncodedContent(data);

                    var response1 = await httpClient.PostAsync(url1, content);

                    if (response1.IsSuccessStatusCode)
                    {
                        var responseData1 = await response1.Content.ReadAsStringAsync();

                        if (responseData1 == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = string.Format("Something went wrong.");
                        }
                        else
                        {
                            //total number of user (getter)
                            //-------------------------------start of code----------------------------------
                            using (var client = new HttpClient())
                            {
                                HttpResponseMessage response2 = await client.GetAsync(url2);

                                if (response2.IsSuccessStatusCode)
                                {
                                    var responseData2 = await response2.Content.ReadAsStringAsync();

                                    if (responseData2 == "No Users found.")
                                    {
                                        ViewBag.ErrorMessage = "No users found.";
                                    }
                                    else
                                    {
                                        int userCount = Convert.ToInt32(responseData2);
                                        ViewBag.UserCount = userCount;
                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "API request failed";
                                }
                            }
                            //-------------------------------end of code------------------------------------

                            //total number of admin (getter)
                            //-------------------------------start of code----------------------------------
                            using (var client = new HttpClient())
                            {
                                HttpResponseMessage response3 = await client.GetAsync(url3);

                                if (response3.IsSuccessStatusCode)
                                {
                                    var responseData3 = await response3.Content.ReadAsStringAsync();

                                    if (responseData3 == "No Users found.")
                                    {
                                        ViewBag.ErrorMessage = "No users found.";
                                    }
                                    else
                                    {
                                        int userCount = Convert.ToInt32(responseData3);
                                        ViewBag.AdminCount = userCount;
                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "API request failed";
                                }
                            }
                            //-------------------------------end of code------------------------------------


                            var newuserObject1 = JsonConvert.DeserializeObject<UserDataGetter>(responseData1);
                            return View(newuserObject1);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("Handle exceptions error");
            }
            return View();
        }
        //----------------------------------------start of code-----------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Reports(UserDataGetter TempData, string ErrorMessage)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewReports.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = ErrorMessage;
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        //----------------------------------------end of code-----------------------------------------------


        //----------------------------------------start of code-----------------------------------------------
        [HttpPost]
        public async Task<ActionResult> ReportChecker(UserDataGetter TempData)
        {
            try
            {
                var userData = HttpContext.Session.GetString("UserData");
                if (string.IsNullOrEmpty(userData))
                {
                    return RedirectToAction("Login", "UserManagement");
                }

                string url = "https://renthive.online/Admin_API/ReportChecker.php";

                using (var httpClient = new HttpClient())
                {
                    int AdminID = TempData.AdminID;
                    string rep_user = TempData.Reported_User;
                    string rep_post = TempData.Post_id;
                    int rep_id = TempData.Rep_id;
                    int numHolder = TempData.NumHolder;

                    string formattedCurrentDateTime = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss") + DateTime.Now.ToString(" tt").ToUpper() ;
                    string origin = "Report Page";
                    string sysResponse= "Success";

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
                            return RedirectToAction("Reports","Home", new { Acc_id = AdminID, ErrorMessage = ViewBag.ErrorMessage });
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
        //----------------------------------------end of code-----------------------------------------------

        //----------------------------------------start of code-----------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SortedList(UserDataGetter TempData)
        {
            try
            {
                var userData = HttpContext.Session.GetString("UserData");
                if (string.IsNullOrEmpty(userData))
                {
                    return RedirectToAction("Login", "UserManagement");
                }

                string url = "https://renthive.online/Admin_API/SortedList.php";

                using (var httpClient = new HttpClient())
                {
                    int numHolder = TempData.NumHolder;

                    var data = new Dictionary<string, string>
                    {
                        {"numholder", numHolder.ToString()}
                    };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        //----------------------------------------end of code-----------------------------------------------


        //----------------------------------------start of code-----------------------------------------------
        public async Task<IActionResult> UserVerification(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/VerificationList.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_Id = TempData.Acc_id;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        //----------------------------------------end of code-----------------------------------------------

        //----------------------------------------start of code-----------------------------------------------
        public async Task<IActionResult> UserVerification_Details(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/UserVerification_Details.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string verID = TempData.Ver_id;
                    var data = new Dictionary<string, string>
                    {
                        {"verid", verID}
                    };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = string.Format("Something went wrong.");
                        }
                        else
                        {
                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);
                            //admin info
                            ViewBag.AdminID = TempData.AdminID;
                            return View(userObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No user record found");
                ViewBag.AdminID = TempData.AdminID;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        //----------------------------------------end of code-----------------------------------------------



        //----------------------------------------start of code-----------------------------------------------
        public async Task<IActionResult> BanAccounts(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewBannedAccounts.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        public async Task<IActionResult> BanAccountInfo(int userID, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewUserStatus.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    int userId = userID;
                    var data = new Dictionary<string, string>
                    {
                        {"userId", userId.ToString()}
                    };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = string.Format("Something went wrong.");
                        }
                        else
                        {
                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);
                            //admin info
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;
                            return View(userObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No user record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        public async Task<IActionResult> SortedDate(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/SortDate_ASC.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        public async Task<IActionResult> SortedID(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/SortID.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> banChanger(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (TempData.Acc_Ban == "0")
            {
                string url = "https://renthive.online/Admin_API/BanUser.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "setTimeBan" , TempData.setTimeBan.ToString() },
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("BanAccountInfo", new
                                {
                                    userID = TempData.userId, //selected user

                                    //admin info
                                    Acc_id = TempData.Acc_id,
                                    Acc_FirstName = TempData.Acc_FirstName,
                                    Acc_MiddleName = TempData.Acc_MiddleName,
                                    Acc_LastName = TempData.Acc_LastName,
                                    Acc_DisplayName = TempData.Acc_DisplayName,
                                    Acc_UserType = TempData.Acc_UserType
                                });
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("Handle exceptions error");
                }
            }
            else
            {
                string url = "https://renthive.online/Admin_API/UnbanUser.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "banduration" , TempData.setTimeBan },
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("BanAccountInfo", new
                                {
                                    userID = TempData.userId, //selected user

                                    //admin info
                                    Acc_id = TempData.Acc_id,
                                    Acc_FirstName = TempData.Acc_FirstName,
                                    Acc_MiddleName = TempData.Acc_MiddleName,
                                    Acc_LastName = TempData.Acc_LastName,
                                    Acc_DisplayName = TempData.Acc_DisplayName,
                                    Acc_UserType = TempData.Acc_UserType
                                });
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("Handle exceptions error");
                }
            }
            return RedirectToAction("HiveUserStatus", new
            {
                userID = TempData.userId, //selected user

                //admin info
                Acc_id = TempData.Acc_id,
                Acc_FirstName = TempData.Acc_FirstName,
                Acc_MiddleName = TempData.Acc_MiddleName,
                Acc_LastName = TempData.Acc_LastName,
                Acc_DisplayName = TempData.Acc_DisplayName,
                Acc_UserType = TempData.Acc_UserType
            });
        }
        //----------------------------------------end of code-----------------------------------------------


        //----------------------------------------start of code-----------------------------------------------
        public async Task<IActionResult> DeactPosts(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewBannedPosts.php";
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            //Successfully retrieving data 
                            var newuserObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;
                            return View(newuserObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        public async Task<IActionResult> DeactPostInfo(int userID, int postID, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/PostDetailsGetter.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            {"userId", userID.ToString() },
                            {"postId", postID.ToString() }
                        };

                    var content = new FormUrlEncodedContent(data);
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = string.Format("Something went wrong.");
                        }
                        else
                        {

                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);

                            ViewBag.UserId = userID; // the selected user
                            ViewBag.postID = postID;

                            //Admin info
                            ViewBag.Acc_Id = TempData.Acc_id;
                            ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                            ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                            ViewBag.Acc_LastName = TempData.Acc_LastName;
                            ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                            ViewBag.Acc_UserType = TempData.Acc_UserType;

                            return View(userObject);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = string.Format("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = string.Format("No user record found");
                ViewBag.Acc_Id = TempData.Acc_id;
                ViewBag.Acc_FirstName = TempData.Acc_FirstName;
                ViewBag.Acc_MiddleName = TempData.Acc_MiddleName;
                ViewBag.Acc_LastName = TempData.Acc_LastName;
                ViewBag.Acc_DisplayName = TempData.Acc_DisplayName;
                ViewBag.Acc_UserType = TempData.Acc_UserType;
                return View();
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ptBanChanger(int userID, int postID, int status, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (status == 1)
            {
                string url = "https://renthive.online/Admin_API/PostStatusTO_0.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", userID.ToString() },
                            { "postId", postID.ToString() }
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                //int userID, int postID, UserDataGetter TempData
                                return RedirectToAction("DeactPostInfo", new
                                {
                                    userID = userID, //selected user
                                    postID = postID,

                                    //admin info
                                    Acc_id = TempData.Acc_id,
                                    Acc_FirstName = TempData.Acc_FirstName,
                                    Acc_MiddleName = TempData.Acc_MiddleName,
                                    Acc_LastName = TempData.Acc_LastName,
                                    Acc_DisplayName = TempData.Acc_DisplayName,
                                    Acc_UserType = TempData.Acc_UserType
                                });

                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("Handle exceptions error");
                }
            }
            else
            {
                string url = "https://renthive.online/Admin_API/PostStatusTO_1.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", userID.ToString() },
                            { "postId", postID.ToString() }
                        };
                        var content = new FormUrlEncodedContent(data);

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "Something went wrong.")
                            {
                                ViewBag.ErrorMessage = string.Format("Something went wrong.");
                            }
                            else
                            {
                                // Update successful.
                                return RedirectToAction("DeactPostInfo", new
                                {
                                    userID = userID, //selected 
                                    postID = postID,

                                    //admin info
                                    Acc_id = TempData.Acc_id,
                                    Acc_FirstName = TempData.Acc_FirstName,
                                    Acc_MiddleName = TempData.Acc_MiddleName,
                                    Acc_LastName = TempData.Acc_LastName,
                                    Acc_DisplayName = TempData.Acc_DisplayName,
                                    Acc_UserType = TempData.Acc_UserType
                                });

                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("API request failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = string.Format("Handle exceptions error");
                }
            }
            return View();
        }
        //----------------------------------------end of code-----------------------------------------------
        public IActionResult TotalUser()
        {
            return View();
        }

        public IActionResult TotalAdmin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}