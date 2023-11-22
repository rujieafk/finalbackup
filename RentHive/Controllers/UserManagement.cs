using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentHive.Models;


namespace RentHive.Controllers
{
    public class UserManagement : Controller
    {
        //------------------------------------------------------------------------
        [HttpGet]
        public IActionResult SignUp(int TempuserId)
        {
            ViewBag.Acc_id = TempuserId;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(int TempuserId, UserDataGetter TempData)
        {
            // Replace with your PHP API URL hosted on 000webhost
            string url = "https://renthive.online/Admin_API/Signup.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string Firstname = TempData.Acc_FirstName;
                    string Lastname = TempData.Acc_LastName;
                    string Middlename = TempData.Acc_MiddleName;
                    string DisplayName = TempData.Acc_DisplayName;
                    string Birthdate = TempData.Acc_Birthdate;
                    string PhoneNum = TempData.Acc_PhoneNum;
                    string Address = TempData.Acc_Address;
                    string Username = TempData.Acc_Email;
                    string Password = TempData.Acc_Password;

                    // Create a dictionary with username and password
                    var data = new Dictionary<string, string>
                    {
                        {"firstname", Firstname},
                        {"lastname", Lastname},
                        {"middlename", Middlename},
                        {"displayname", DisplayName},
                        {"birthdate", Birthdate},
                        {"phonenum", PhoneNum},
                        {"address", Address},
                        {"username", Username},
                        {"password", Password},
                    };

                    // Serialize the credentials as JSON and send them in the request body.
                    var content = new FormUrlEncodedContent(data);

                    // Make a POST request to the PHP API
                    var response = await httpClient.PostAsync(url, content);


                    // Ensure a successful response
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (responseData == "User Registration failed")
                        {
                            ViewBag.ErrorMessage = string.Format("Email is already taken");
                            return View();
                        }
                        else
                        {
                            //User Registration successful
                            ViewBag.ErrorMessage = string.Format("User Registration successful.");
                            ViewBag.Acc_id = TempuserId;
                            return View();
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
        //--------------------------------------------------------------------------

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(UserDataGetter TempData)
        {
            // Replace with your PHP API URL hosted on 000webhost
            string url = "https://renthive.online/Admin_API/Login.php";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string Username = TempData.Acc_Email;
                    string Password = TempData.Acc_Password;

                    // Create a dictionary with username and password
                    var data = new Dictionary<string, string>
                    {
                        {"username", Username},
                        {"password", Password}
                    };

                    // Serialize the credentials as JSON and send them in the request body.
                    var content = new FormUrlEncodedContent(data);

                    // Make a POST request to the PHP API
                    var response = await httpClient.PostAsync(url, content);

                    // Ensure a successful response
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();

                        // Check if login was successful
                        if (responseData == "failed to login.")
                        {
                            ViewBag.ErrorMessage = string.Format("Login failed");
                        }
                        else
                        {
                            // Deserialize the JSON response into a dynamic object
                            var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);

                            

                            /*return RedirectToAction("Index", "Home", new
                            {
                                Acc_id = userObject.Acc_id
                            }) ;*/

                            // Store user information in a session variable
                            HttpContext.Session.SetString("UserData", JsonConvert.SerializeObject(userObject));
                            return RedirectToAction("Index", "Home");
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
                ViewBag.ErrorMessage = string.Format("Please enter correct admin credentials.");
            }
            return View();
        }
        //---------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Profile(int TempuserId)
        {
            var userData = HttpContext.Session.GetString("UserData");
            if (string.IsNullOrEmpty(userData))
            {
                // User is not logged in, redirect to login or handle as needed
                return RedirectToAction("Login", "UserManagement");
            }

            string url = "https://renthive.online/Admin_API/UpdateGet.php";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    int userId = TempuserId;

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

        [HttpPost]
        public async Task<IActionResult> Profile(UserDataGetter TempData, [FromForm] IFormFile file)
        {

            if (TempData.NumHolder == 1)
            {
                if (file != null && file.Length > 0)
                {
                    // Assuming you have a method to convert the file to base64
                    string base64Image = ConvertFileToBase64(file);

                    // You can then do something with the base64 image, like save it to the database or use it in your application

                    // For demonstration purposes, let's just return a view with the base64 image
                    /*ViewBag.Base64Image = base64Image;*/
                    string url = "https://renthive.online/Admin_API/ImageUpload.php";
                    try
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var data = new Dictionary<string, string>
                            {
                                { "userId", TempData.Acc_id.ToString() },
                                { "image", base64Image }
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
                                    /*var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);
                                    return View(userObject);*/
                                    return RedirectToAction("Profile", new { TempuserId = TempData.Acc_id });

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
            }

            if (TempData.NumHolder == 2)
            {
                string url = "https://renthive.online/Admin_API/UpdatePost.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var data = new Dictionary<string, string>
                        {
                            { "userId", TempData.Acc_id.ToString() },
                            { "firstname", TempData.Acc_FirstName },
                            { "lastname", TempData.Acc_LastName },
                            { "middlename", TempData.Acc_MiddleName },
                            { "displayname", TempData.Acc_DisplayName },
                            { "birthdate", TempData.Acc_Birthdate },
                            { "phonenum", TempData.Acc_PhoneNum },
                            { "address", TempData.Acc_Address },
                            { "email", TempData.Acc_Email },
                            { "password", TempData.Acc_Password }
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
                                /*var userObject = JsonConvert.DeserializeObject<UserDataGetter>(responseData);
                                return View(userObject);*/
                                return RedirectToAction("Profile", new { TempuserId = TempData.Acc_id });

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
            return RedirectToAction("Profile", new { TempuserId = TempData.Acc_id});
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        private string ConvertFileToBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------

        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> DeleteAccount(int TempuserId, string confirmation)
        {
            if (confirmation == "yes")
            {
                string url = "https://renthive.online/Admin_API/DeleteAccount.php";
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        int _TempuserId = TempuserId;

                        // Create a dictionary with username and password
                        var data = new Dictionary<string, string>
                    {
                        {"userId", _TempuserId.ToString()}
                    };

                        // Serialize the credentials as JSON and send them in the request body.
                        var content = new FormUrlEncodedContent(data);

                        // Make a POST request to the PHP API
                        var response = await httpClient.PostAsync(url, content);

                        // Ensure a successful response
                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the response content
                            var responseData = await response.Content.ReadAsStringAsync();

                            if (responseData == "User deleted successfully.")
                            {
                                ViewBag.ErrorMessage = string.Format("User deleted successful");
                            }
                            else
                            {
                                ViewBag.ErrorMessage = string.Format("User deleted failed");
                                return RedirectToAction("Login", "UserManagement");
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
            return RedirectToAction("Profile", new { TempuserId = TempuserId });
        }
        public IActionResult Logout()
        {
            // Remove the "UserData" from the session
            HttpContext.Session.Remove("UserData");

            // Redirect to the login or home page after logout
            return RedirectToAction("Login", "UserManagement"); // Replace "Login" with your actual login page
        }


    }
}