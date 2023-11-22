using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentHive.Models;

namespace RentHive.Controllers
{
    public class SystemManagement : Controller
    {
        [HttpGet]
        public async Task<IActionResult> HiveUserList(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }

            string url = "https://renthive.online/Admin_API/ViewUsersList.php";
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
        [HttpGet]
        public async Task<IActionResult> HiveAdminList(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewAdminList.php";
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
                            var userObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
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
        public IActionResult HiveAdminList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HiveUserDetails(int userID, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewUsersDetails.php";
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

                            ViewBag.UserId = userID; // the selected user

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
                return View(new List<UserDataGetter>());// Returns an emtpy list 
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> HiveUserLog(int userID, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewUserLog.php";
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
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("Successfully retrieving data ");
                            //Successfully retrieving data 
                            var userObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
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
        [HttpPost]
        public IActionResult HiveUserLog()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HiveUserPayment(int userID, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url = "https://renthive.online/Admin_API/ViewUserPayment.php";
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
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "No Users found")
                        {
                            ViewBag.ErrorMessage = string.Format("No users found.");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("Successfully retrieving data ");
                            //Successfully retrieving data 
                            var userObject = JsonConvert.DeserializeObject<List<UserDataGetter>>(responseData);
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
        [HttpPost]
        public IActionResult HiveUserPayment()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> HiveUserStatus(int userID, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            string url1 = "https://renthive.online/Admin_API/ViewUserStatus.php";
            string url2 = "https://renthive.online/Admin_API/CountReport.php";

            int userId = userID;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var data1 = new Dictionary<string, string>
                    {
                        {"userId", userId.ToString()}
                    };

                    var content1 = new FormUrlEncodedContent(data1);
                    var response1 = await httpClient.PostAsync(url1, content1);

                    if (response1.IsSuccessStatusCode)
                    {
                        var responseData1 = await response1.Content.ReadAsStringAsync();

                        if (responseData1 == "Something went wrong.")
                        {
                            ViewBag.ErrorMessage = string.Format("Something went wrong.");
                        }
                        else
                        {
                            //total number of report to a specific user (getter)
                            //-------------------------------start of code----------------------------------
                            using (var httpClient2 = new HttpClient())
                            {
                                var data2 = new Dictionary<string, string>
                                {
                                    {"userId", userId.ToString()}
                                };

                                var content2 = new FormUrlEncodedContent(data2);
                                var response2 = await httpClient.PostAsync(url2, content2);

                                if (response2.IsSuccessStatusCode)
                                {
                                    var responseData2 = await response2.Content.ReadAsStringAsync();

                                    if (responseData2 == "Something went wrong.")
                                    {
                                        ViewBag.ErrorMessage = string.Format("Something went wrong.");
                                    }
                                    else
                                    {
                                        int userCount = Convert.ToInt32(responseData2);
                                        ViewBag.UserCount = 10;

                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "API request failed";
                                }
                            }
                            //-------------------------------end of code------------------------------------


                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData1);
                            ViewBag.CheckCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy"); // this is only for if condition purposes only
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
                ViewBag.ErrorMessage = string.Format("Handle exceptions error");
            }
            return View();
        }

        public IActionResult sampleView(int sample1, string sample2) //THIS CONTROLLER IS A SAMPLE VIEW ONLY
        {
            ViewBag.sam1 = sample1;
            ViewBag.sam2 = sample2;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HiveUserStatusPost1(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (TempData.Acc_Ban == "0" || TempData.Acc_Ban == null)
            {
                string url = "https://renthive.online/Admin_API/BanUser.php";
                try
                {
                    /*string userBanDate = DateTime.Now.ToString("MMMM dd, yyyy");*/
                    /*DateTime currentDate = DateTime.Now;
                    DateTime userBanEndDate = currentDate.AddDays(3);

                    string formattedCurrentDate = currentDate.ToString("MMMM dd, yyyy");
                    string formattedEndDate = userBanEndDate.ToString("MMMM dd, yyyy");*/

                    string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                    string formattedEndDate = DateTime.Now.AddDays(3).ToString("MMMM dd, yyyy");

                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "userBanDate", formattedCurrentDate },
                            { "userBanEnd", formattedEndDate },
                            { "strike", TempData.Acc_Strikes.ToString() }
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
                    string userBanDate = "NULL";

                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "userBanDate", userBanDate}
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
        [HttpPost]
        public async Task<IActionResult> WeekBanUser(UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (TempData.Acc_Ban == "0" || TempData.Acc_Ban == null)
            {
                string url = "https://renthive.online/Admin_API/WeeKBanUser.php";
                try
                {
                    /*string userBanDate = DateTime.Now.ToString("MMMM dd, yyyy");*/
                    /*DateTime currentDate = DateTime.Now;
                    DateTime userBanEndDate = currentDate.AddDays(3);

                    string formattedCurrentDate = currentDate.ToString("MMMM dd, yyyy");
                    string formattedEndDate = userBanEndDate.ToString("MMMM dd, yyyy");*/

                    string formattedCurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                    string formattedEndDate = DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy");

                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.userId.ToString() },
                            { "userBanDate", formattedCurrentDate },
                            { "userBanEnd", formattedEndDate },
                            { "strike", TempData.Acc_Strikes.ToString() }
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

        [HttpPost]
        public async Task<IActionResult> HiveUserStatusPost2(int userID, int status, UserDataGetter TempData)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }
            if (status == 1)
            {
                string url = "https://renthive.online/Admin_API/StatusTO_0.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", userID.ToString() },
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
                                return RedirectToAction("HiveUserStatus", new
                                {
                                    userID = userID, //selected user

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
                string url = "https://renthive.online/Admin_API/StatusTO_1.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", userID.ToString() },
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
                                return RedirectToAction("HiveUserStatus", new
                                {
                                    userID = userID, //selected user

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
    }
}