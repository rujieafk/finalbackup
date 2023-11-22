namespace RentHive.Models
{
    public class UserDataGetter
    {
        
        //global use
        public int AdminID { get; set; } // this is a temporary holder
        public int Acc_id { get; set; }
        public int Rental_id { get; set; }
        public string Post_id { get; set; }
        public string SortedList { get; set; }
        public int NumHolder { get; set; } // this is a temporary holder

        //account table
        //-------------start---------------------------
        public string Acc_FirstName { get; set; }
        public string Acc_LastName { get; set; }
        public string Acc_MiddleName { get; set; }
        public string Acc_DisplayName { get; set; }
        public string Acc_Birthdate { get; set; }
        public string Acc_PhoneNum { get; set; }
        public string Acc_Address { get; set; }
        public string Acc_Email { get; set; }
        public string Acc_Password { get; set; }
        public string Acc_UserType { get; set; }
        public int Acc_Active { get; set; }
        public string Acc_Ban { get; set; }
        public string Acc_BanDate { get; set; }
        public string Acc_BanEndDate { get; set; }
        public int Acc_Strikes { get; set; }

        public string userId { get; set; } // Selected User
        public string setTimeBan { get; set; }
        //-------------end---------------------------


        //picture table-------------------------------
        //------------------start--------------------------
        public string Images { get; set; }
        //------------------start--------------------------

        //rental table-------------------------------
        //------------------start--------------------------
        public string Rental_Location { get; set; }
        public string Rental_Category { get; set; }
        public string Rental_Amount { get; set; }
        //------------------end--------------------------

        //post table
        //------------------start--------------------------
        public string Post_Title { get; set; }
        public string Post_RentPeriod { get; set; }
        public string Post_Term { get; set; }
        public string Post_Calendar { get; set; }
        public string Post_Status { get; set; }
        public string Post_Price { get; set; }
        public int Post_Active { get; set; }
        public string Post_BanDate { get; set; }
        public string Post_AdvDeposit { get; set; }
        public string Post_DatePosted { get; set; }
        //------------------end--------------------------

        //transaction table
        //------------------start--------------------------
        public string About { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentMode { get; set; }
        public string AdvDeposit { get; set; }
        public string Delivery { get; set; }
        public string TransInfo { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        //------------------end--------------------------

        //userlog table
        //------------------start--------------------------
        public int ul_id { get; set; }
        public string ul_Timestamp { get; set; }
        public string ul_Origin { get; set; }
        public string ul_Action { get; set; }
        public string ul_SysResponse { get; set; }
        //------------------end--------------------------

        //report table
        //------------------start--------------------------
        public int Rep_id { get; set; }
        public string Rep_Topic { get; set; }
        public string Rep_Message { get; set; }
        public string Rep_Approve { get; set; }
        public string Rep_Date { get; set; }
        public string Reported_User { get; set; }
        //------------------end--------------------------

        //verification table
        //------------------start--------------------------
        public string Ver_id { get; set; }
        public string Ver_Status { get; set; }
        public string Ver_DateSent { get; set; }
        //------------------end--------------------------

    }
}
