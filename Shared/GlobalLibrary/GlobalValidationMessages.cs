using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalLibrary
{
    public class GlobalValidationMessages
    {
            public static class Required
            {

            public const string firmID = "firm id is required";
            public const string branchID = "branch id is required";
            public const string imps = "Imps required";
            public const string scheschemeValue = "Schescheme value is required";
            public const string ConfirmStatus = "Confirm status is required";
            public const string SMSStatus = "SMS status is required";
            public const string ConfirmChoice = "Confirm choice is required";
            public const string revenueStamp = "Revenue stamp number is required";
            public const string purchaseDt = "Purchase date is required";

            public const string otherFirm = "Other firm id is required";
            public const string bankCheckStatus = "Bank check status is required";
            public const string otherBranchCheck = "Other bank check status is required";
            public const string checkCash = "Cash Check status is required";
            public const string checkEzeTap = "Eze tap check status is required";
            

            public const string settlementAmount = "Settlement amount is required";
            public const string pledgestatus = "Pledge status is required";
            public const string bankAmount = "Bank amount is required";
            public const string otherFirmsettlementAmount = "Other firmsettlement amount is required";
            public const string balance = "Balance is required";
            public const string interest = "Interest is required";
            public const string interestWaiver = "Interest waiver is required";
            public const string postage = "Postage waiver is required";
            public const string rebate = "Postage waiver is required";
            public const string appraisalAmount = "Appraisal amount is required";
            public const string grandTotal = "Grand total is required";
            public const string payAmount = "pay amount is required";

            public const string pledgeData = "Pledge data is required";

            public const string transId = "trans id is required";
            public const string userID = "user id is required";
            public const string password = "password id is required";
            public const string abh_bh_userID = "ABH/BH user id is required";
            public const string abh_bh_password = "ABH/BH password id is required";
            public const string RRN = "RRN is required";
            public const string UUID = "UUID is required";
            public const string custName = "Customer name is required";

            public const string createdBy = "Created by is required";
            public const string reportedBy = "Reported by is required";
            public const string kycAddressMatch = "KYC Address match is required";
            public const string kycCrclrComp = "KYC Circular is required";
            public const string exposureAmount = "Exposure Amount is required";

            public const string fatHusName = "Father/Husband is required";
            public const string houseName = "House name is required";
            public const string location = "location is required";
            public const string countryID = "country id is required";
            public const string pinsrl = "Pin serial is required";
            public const string occupation = "occupation is required";
            public const string mobileNo = "Mobile number is required";
            public const string confirmMobileNo = "Confirm mobile number is required";
            public const string dob = "DOB is required";
            public const string gender = "gender is required";
            public const string mediaType = "mediaType is required";
            public const string media = "media is required";
            public const string religion = "religion is required";
            public const string caste = "caste is required";
            public const string loanPurpose = "loan purpose is required";
            public const string citizen = "citizen is required";
            public const string nationality = "nation is required";
            public const string residentialStatus = "resident is required";
            public const string marital = "marital is required";
            public const string relationIdentity = "relation identity is required";
            public const string relation = "relation  is required";
            public const string preName = "pre name is required";
            public const string address_Flg = "address Flg is required";
            public const string qualification = "qualification is required";
            public const string firstGL = "first gold loan is required";
            public const string custType = "customer type is required";
            public const string custStatus = "customer status is required";
            public const string isActive = "is active flag is required";
            public const string customerCategory = "customer category is required";
            public const string kycOf = "kyc of flag is required";
            public const string kycType = "kyc type is required";
            public const string kycIDType = "kyc id type is required";
            public const string kycID = "kyc id type is required";
            public const string pep = "pep is required";
            public const string income = "income is required";
            public const string prefLang = "preferred language is required";
            public const string language = "preferred language is required";
            public const string accountNo = "account number is required";
            public const string neftPhoto = " neft photo is required";
            public const string accountType = "account type is required";
            public const string IFSCCode = "IFSC code is required";
            public const string custID = "Customer id is required";
            public const string status = "Status is required";
            public const string mPin = "MPin is required";
            public const string branchName = "branch name is required";
            public const string PBCustomerName = "PB customer name is required";
            public const string moduleId = "Module id is required";
            public const string optionId = "Option id is required";
            public const string identityId = "Identity id is required";
            public const string panNo = "Pan no is required";
            public const string panPhoto = "Pan photo is required";
            public const string pinCode = "Pin code is required";
            public const string stateId = "State id is required";
            public const string parameterId = "Parameter id is required";
            public const string postOfficeId = "Postoffice id  is required";
            public const string districtId = "District id  is required";
            public const string empCode = "Employee code  is required";
            public const string userName = "User name is required";
            public const string passWord = "Password is required";
            public const string otherBranchPassWord = "Password is required";

            public const string siganture = "Signature is required";

            public const string signature = "Signature is required";
            public const string schemeValue = "Scheme value is required";
            
            public const string empName = "Employee name  is required";
            public const string mediaId = "Media id is required";
            public const string type = "Type is required";
            public const string formMode = "Form mode is required";
            public const string custPhoto = "Customer photo is required";
            public const string kycPhoto = "Customer photo is required";
            public const string code = "Code is required";
            public const string searchValue = "Search value is required";
            public const string otp = "OTP is required";
            public const string checkOtp = "Check OTP is required";
            public const string token = "Token is required";
            public const string fatHus_Flag = "father husband modified or not flag is required";
            public const string house_Flag = "house modified or not flag is required";
            public const string locality_Flag = "locality modified or not flag is required";
            public const string pinSerial_Flag = "pin serial modified or not flag is required";
            public const string cust_Status_Flag = "cust status modified or not flag is required";
            public const string occupation_Flag = "occupation modified or not flag is required";
            public const string phone1_Flag = "phone1 modified or not flag is required";
            public const string phone = "phone number is required";
            public const string phone2_Flag = "phone2 modified or not flag is required";
            public const string loyaltiCardno_Flag = "loyalti card number modified or not flag is required";
            public const string kyc_Type_Flag = "kyc type modified or not flag is required";
            public const string key_Id_Flag = "key id modified or not flag is required";
            public const string kyc_Issue_Place_Flag = "kyc issue place modified or not flag is required";
            public const string descr_Flag = "descr modified or not flag is required";
            public const string street_Flag = "street modified or not flag is required";
            public const string cust_Name_Flag = "cust name modified or not flag is required";
            public const string email_Flag = "email modified or not flag is required";
            public const string accountTypeName = "account type name is required";
            public const string schemes = "scheme is required";
            public const string items = "Add any item to continue";
            public const string itemsImage = "Items image is required";
            public const string doorStep = "Door step is required";
            public const string act_weight = "actual weight is required";
            public const string net_weight = "net weight is required";
            public const string new_items_wt = "new items weight is required";
            public const string purity = "purity is required";
            public const string stone_weight = "stone weight is required";
            public const string backDateGl1 = "back date Gl1 is required";
            public const string schemeBrandID = "scheme brand id is required";
            public const string new_net_weight = "net weight is required";
            public const string optFlag = "optFlag is required";
            public const string itemID = "itemID is required";
            public const string pledgeNo = "pledge number is required";
            public const string idNumber = "Identity number number is required";
            public const string idType = "Id type is required";
            public const string smsaccountType = "Sms account type is required";
            public const string comments = "comment is required";
            public const string inventoryNo = "Inventory no is required";
            public const string inventoryQuotationNo = "InventoryQuotationNo no is required";            
            public const string barCode = "barCode is required";
            public const string parameter = "parameter is required";
            public const string fromDate = "fromdate is required";
            public const string toDate = "todate is required";
            public const string typeId = "typeId is required";
            public const string pledgeValue = "pledge value is required";
            public const string itemId = "Item id is required";
            public const string itemCount = "Item count is required";
            public const string purityId = "Purity id is required";
            public const string itemType = "Item type is required";
            public const string newItem = "New item is required";
            public const string quotNoType = "Quot no type is required";
            public const string netWgt = "Net weight is required";
            public const string grossWgt = "Gross weight is required";
            public const string documentPhoto = "pledge documents photo is required";
            public const string smsStatus = "Sms status is required";
            public const string invStatus = "Inventory status is required";
            public const string transID = "Transaction id is required";
            public const string kdmGross = "kdm gross is required"; 
            public const string chType = "ch type is required";
            public const string lUpdate = "update is required";
            public const string aprLevel = "apr level is required";
            public const string param2 = "param2 is required";
            public const string param3 = "param3 is required";
            public const string amount = "amount is required";
            public const string barCodeCheckStatus = "Barcode check status is required";
            
            public const string quotationNo = "quotationNo is required";
            public const string inputData = "input data is required";
            public const string flagVal = "flagVal is required";
            public const string serialno = "serialno is required";
            public const string Message = "Message is required";
            public const string approximateValue = "Approximate value is required";
            public const string itemDetails = "ItemDetails value is required";
            public const string schemeDetails = "SchemeDetails value is required";
            public const string jewelBill = "JewelBill value is required";
            public const string insuranceStatus = "Insurance Status value is required";
            public const string neftFlag = "NeftFlag value is required";

            public const string neftId = "Neft id value is required";       
            public const string chequeFlag = "Cheque flag value is required";
            public const string bankDtl = "Bank Detail value is required";
            public const string cnFlag = "CN flag value is required";
            public const string yesFlag = "YES flag value is required";       
            public const string counter = "Counter value is required";                
            public const string weighingBalance = "Weighing balance value is required";
            public const string reason = "reason value is required";
            public const string inventoryCharge = "inventory charge is required";
            public const string otherCharge = "Other charge is required";
            public const string totAmount = "Total amount is required";
            public const string formKStatus = "FormK status is required";
            public const string formKCharge = "FormK charge is required";
            public const string smsCode = "SMS code is required";
            public const string ipAddress = "ipAddress is required";
            public const string pledgedata = "pledgedata is required";
            public const string counterType = "counter type is required";
            public const string formkPhoto = "formk photo is required";
            public const string formkPhotoStatus = "formk photo status is required";
            
            public const string key = "key is required";
            public const string interestPayDate = "interest pay date is required";
            public const string formK = "formK is required";
            public const string smsSendStsatus = "Sms send status is required";
            
            public const string kycStatus = "KYC Status is required";
            public const string neftStatus = "NEFT Status is required";
            public const string irregularStatus = "Irregular Status is required";
            public const string scanStatus = "Scan Status is required";

            public const string pawnNumber = "pawnNumber is required";
            public const string cashID = " Cash ID is required";

            public const string oldBarCode = " oldBarCode is required";

            public const string txnid = "Transaction ID  is required";
            public const string autncode = " Authorised Code is required";
            public const string goldloanstatus = " Goldloanstatus is required";
            public const string remarks = "Remarks required";
            public const string insertedby = " Insertedby is required";
            public const string paymentstatus = " Paymentstatusis required";
            public const string servicecharge = " servicecharge is required";
            public const string rezetapamount = " Rrezetap amount is required";
            public const string custreceipturl = " custreceipturl is required";
            public const string sglOrOgl = "SGL or OGL is required";

            public const string printButtonValue = "Print button value is required";

            public const string paymentMode = "payment mode is required";
            public const string balanceReceive = "balance receive is required";

            public const string minimumValue = "minimum value is required";
            public const string disbursementValue = "disbursement value is required";
            public const string maturityValue = "maturity value is required";
            public const string settlementValue = "settlement value is required";

            public const string pwantktbutton = "Pawnticket button value is required";

            public const string loginCounter = "login counter is required";
            public const string pagenumber = "page number is required";
            public const string pledgePrinterIP = "Pledge Printer IP is required";
            public const string releasePrinterIP = "Release Printer IP is required";
            public const string pledgePrinterName = "Pledge Printer Name is required";
            public const string releasePrinterName = "Release Printer Name is required";
            public const string pledgePrinterValue = "Pledge Value  is required";
            public const string releasePrinterValue = "Release Value  is required";


        }
        public static class Invalid
        {
            public const string imps = "Invalid Imps";
            public const string comments = "Invalid Comments";
            public const string firmID = "please use valid firm id";
            public const string confchoice = "please use valid confirmation choice ";
            public const string ConfStat = "please use valid confirmation status ";
            public const string smsStat = "please use valid sms status ";
            public const string revenueStamp = "please use valid revenue stamp number";
            public const string branchID = "please use valid branch id";
            public const string transId = "please use valid trans id";
            public const string userID = "please login to get valid user id";
            public const string RRN = "RRN is required";
            public const string UUID = "UUID is required";
            public const string custName = "Invalid customer name, use alphabets only";
            public const string createdBy = "Invalid user name";
            public const string createdByMax = "The user length should be 7 digits.";
            public const string fatHusName = "Invalid father/husband name, use alphabets only";
            public const string houseName = "Invalid house name, use alphabets only";
            public const string countryID = "please use valid country id";
            public const string pinsrl = "Pin serial must be numeric";
            public const string pinsrl_lenth = "Invalid pin serial";
            public const string occupation = "please use valid occupation";
            public const string phoneno = "Phone must be numeric";
            public const string mobileNo = "Mobile number must be numeric";
            public const string mobileNoLength = "Invalid mobile number";
            public const string vehicleLoanNumberLength = "Invalid vehicle loan number length";
            public const string confirmMobileNo = "Confirm mobile number must be numeric";
            public const string confirmMobileNoLength = "Invalid confirm mobile number";
            public const string otpLength = "Invalid otp length";


            public const string email = "Invalid email address";
            public const string dob = "DOB is invalid";
            public const string gender = "please use valid gender id";
            public const string mediaType = "please use valid media type";
            public const string media = "please use valid media";
            public const string moduleID = "please use valid module id";
            public const string religion = "please use valid religion";
            public const string caste = "please use valid caste";
            public const string exServiceStatus = "please use valid ex status";
            public const string loanPurpose = "please use valid loan purpose";
            public const string citizen = "please use valid citizen";
            public const string nationality = "please use valid nation";
            public const string residentialStatus = "please use valid resident";
            public const string marital = "please use valid marital";
            public const string relationIdentity = "please use valid relation identity";
            public const string preName = "please use valid pre name";
            public const string address_Flg = "please use valid address Flg";
            public const string qualification = "please use valid qualification";
            public const string firstGL = "please use valid first gold loan flag";
            public const string custType = "please use valid customer type";
            public const string custStatus = "please use valid customer status";
            public const string isActive = "please use valid customer status";
            public const string customerCategory = "please use valid customer ategory";
            public const string kycOf = "please use valid kyc of flag";
            public const string kycType = "please use valid kyc type";
            public const string kycIDType = "please use valid kyc id type";
            public const string kycID = "please use valid kyc id type";
            public const string pep = "please use valid pep";
            public const string income = "please use valid income";
            public const string prefLang = "please use valid preferred language";
            public const string language = "please use valid preferred language";
            public const string accountNo = "please use valid account number";
            public const string neftPhoto = "please use valid neft photo";
            public const string accountType = "please use valid account type";
            public const string IFSCCode = "please use valid IFSC code";
            public const string custID = "please use valid customer id";
            public const string status = "please enter valid status";
            public const string goldloan_status = "please enter valid gold loan status";
            public const string statusChar = "please enter valid status character";
            public const string statusCharMin = "The Status min  length should be 1 Character.";
            public const string statusCharMax = "The Status max  length should be 2 Characters.";
            public const string mPin = "please enter valid MPin";
            public const string mPinMax = "The Mpin max length should be 40 digits.";

            public const string mobPhone = "The mobile number length should be 10 digits.";
            public const string custIdlength = "The customer id length should be 10 digits.";
            public const string custIdMinlength = "The customer id min length should be 10 digits.";
            public const string custIdMaxlength = "The customer id max length should be 10 digits.";
            public const string custNameMaxlength = "The customer Name max length should be 40 Letters.";
            public const string remarks = "Invalid Remarks";


            public const string branchName = "please use valid branch name";
            public const string PBCustomerName = "please use valid PB customer name";
            public const string moduleId = "please use valid module id";
            public const string optionId = "please use valid option id";
            public const string identityId = "please use valid identity id";
            public const string panNo = "please use valid pan no";
            public const string panPhoto = "please use valid  pan photo";
            public const string pinCode = "please use valid  Pin code";
            public const string stateId = "please use valid state id";
            public const string parameterId = "please use valid parameter id";
            public const string postOfficeId = "please use valid postoffice id";
            public const string districtId = "please use valid district id";
            public const string empCode = "please use valid employee code";
            public const string userName = "please use valid user name";
            public const string otherBranchUserName = "please use valid other branch user name";
            
            public const string empCodeMin = "The emp Code min length should be 5 Letters.";
            public const string empCodeMax = "The emp Code max length should be 5 Letters.";

            public const string empName = "please use valid employee name";
            public const string mediaId = "please use valid media id";
            public const string type = "Please use valid type";
            public const string code = "Please use valid code";
            public const string landDtl = "Please use valid landDtl id";
            public const string vehicleloan = "Please use valid vehicle loan";
            public const string otp = "Please use valid OTP";
            public const string checkOtp = "Please use valid check OTP";
            public const string fatHus_Flag = " Invalid father / husband modified flag";
            public const string house_Flag = " Invalid house modified flag";
            public const string locality_Flag = " Invalid locality modified flag";
            public const string pinSerial_Flag = " Invalid pin serial modified flag";
            public const string cust_Status_Flag = " Invalid cust status modified flag";
            public const string occupation_Flag = " Invalid occupation modified flag";
            public const string phone1_Flag = " Invalid phone1 modified flag";
            public const string phone2_Flag = " Invalid phone2 modified flag";
            public const string loyaltiCardno_Flag = " Invalid loyalti cardno modified flag";
            public const string kyc_Type_Flag = " Invalid kyc type modified flag";
            public const string key_Id_Flag = " Invalid key id modified flag";
            public const string kyc_Issue_Place_Flag = " Invalid kyc issue place modified flag";
            public const string descr_Flag = " Invalid descr modified flag";
            public const string street_Flag = " Invalid street modified flag";
            public const string cust_Name_Flag = " Invalid cust name modified flag";
            public const string email_Flag = " Invalid email modified flag";
            public const string accountTypeName = "please use valid account type name";
            public const string formMode = "please use valid form mode";
            public const string doorStep = "Please use valid door step";

            public const string itemId = "Please use valid item id";
            public const string itemCount = "Please use valid item count";
            public const string purityId = "Please use valid purity id";
            public const string itemType = "Please use valid item type";
            public const string newItem = "Please use valid new item";
            public const string quotNoType = "Please use valid quot no type";
            public const string smsaccountType = "Please use valid SMS Account type";
            public const string newItemZeroOne = "New item value should be  0 or 1";
            
            public const string grossWgt = "Please use valid gorss weight";
            public const string aprLevel = "Please use valid aprove level";
            public const string stonewgt = "Please use valid stone weight";
            public const string netWgt = "Please use valid net weight";
            public const string netWgtShortScheme = "Please use valid net wgt short scheme";
            public const string validDecimal = "Valid Decimal number with maximum 2 decimal places";
            public const string gl1decimalFormat = "Valid Decimal number with maximum 3 decimal places";
            
            public const string act_weight = "Please use valid actual weight";
            public const string net_weight = "Please use valid net weight";
            public const string new_items_wt = "Please use valid new items weight";
            public const string purity = "Please use valid purity";
            public const string stone_weight = "Please use valid stone weight";
            public const string backDateGl1 = "Please use valid back date Gl1";
            public const string schemeBrandID = "Please use valid scheme brand id";
            public const string new_net_weight = "Please use valid net weight";
            public const string optFlag = "Please use valid optFlag";
            public const string itemID = "Please use valid itemID";
            public const string pledgeNo = "Please use valid pledgeNo";

            public const string barCode = "Please use valid barCode";
            public const string parameter = "Please use valid parameter";
            public const string fromDate = "Please use valid fromDate";
            public const string toDate = "Please use valid todate";
            public const string typeId = "Please use valid typeId";
            public const string pledgeValue = "Please use valid pledge value format";
            public const string pledgeNoLength = "The pledge no length should be 16 digits.";
            public const string pledgeNoMinLength = "The pledge no min length should be 16 digits.";
            public const string pledgeNoMaxLength = "The pledge no max length should be 16 digits.";

            public const string quotNo = "Please use valid quotation no";
            public const string quotNoLength = "The quotation no length should be 16 digits.";
            public const string quotNoMinLength = "The quotation no min length should be 16 digits.";
            public const string quotNoMaxLength = "The quotation no max length should be 16 digits.";

            public const string inventoryNo = "Please use valid inventory no";

            public const string inventoryNoLength = "The inventory no length should be 16 digits.";
            public const string inventoryCharge = "Please use valid inventory no";  
            public const string inventoryChargeMax = "The inventoryCharge max length should be 5 digits.";
            public const string inventoryNoMinLength = "The inventory no min length should be 16 digits.";
            public const string inventoryNoMaxLength = "The inventory no max length should be 16 digits.";
            public const string transID = "please use valid transaction id";
           

            public const string quotationNo = "please use valid quotationNo";
            public const string serialno = "please use valid serial No";
            public const string flagVal = "please use valid  value flag";

            public const string Message = "please use valid Message";
            public const string MessageLength = "The Message max length should be 160 digits.";
           
            public const string approximateValue = "Please use valid approximate value";
            public const string inventoryQuotationNo = "Please use valid inventoryQuotationNo";
            public const string invStatus = "Please use valid invStatus";
            public const string insuranceStatus = "Please use valid insurance Status";
            public const string kdmGross = "Please use valid kdmGross";

            public const string neftFlag = "Please use valid NeftFlag value is required";

            public const string neftId = "Please use valid  neftId";
            public const string chequeFlag = "Please use valid  chequeFlag";
            public const string bankDtl = "Please use valid  bankDtl";
            public const string cnFlag = "Please use valid  CNFlag ";
            public const string yesFlag = "Please use valid  YESFlag";
            public const string counter = "Please use valid  Counter";
            public const string weighingBalance = "Please use valid  WeighingBalance";
            public const string reason = "Please use valid reason ";
            public const string otherCharge = "Please use valid Charge ";
            public const string otherChargeMax = "The other Charge max length should be 3 digits.";

            public const string totAmount = "Please use valid total amount ";
            public const string totAmountMax = "The total amount  max length should be 6 digits.";
            public const string formKStatus = "Please use valid formKStatus";
            public const string formKStatusMax = "The form status max length should be 1 digit.";
            public const string formKStatusMin = "The form status min length should be 1 digit.";

            public const string formKCharge = "Please use valid  FormKCharge";
            public const string formKChargeMax = "The form charge max length should be 3 digits.";
            public const string smsCode = "Please use valid smsCode";
            public const string smsStatus = "Please use valid smsStatus";
            public const string ipAddress = " Please use valid ipAddress";
            public const string counterType = " Please use valid counter type";
            public const string amount = " Please use valid amount";
            public const string barCodeCheckStatus = " Please use valid bar code check status";

            
            public const string charge = " Please use valid charge";
            public const string otherCharges = " Please use valid other charges";
            public const string formkCharge = " Please use valid formk charges";

            public const string total = " Please use valid total";
            public const string schemes = " Please use valid schemes";
            public const string key = "Please use valid key";
            public const string keyMinMax = "The key min length should be 1 digit.";

            public const string kycStatus = "Invalid KYC Status";
            public const string kycStatusMin = "The KYC status min length should be 1 digit.";
            public const string kycStatusMax = "The KYC status min length should be 1 digit.";

            public const string remarksMin = "The Remarks min length should be 1 Letter";
            public const string remarksMax = "The Remarks max length should be 500 Letters";

            public const string neftStatus = "Invalid NEFT Status";
            public const string neftStatusMin = "The Neft status min length should be 1 digit.";
            public const string neftStatusMax = "The Neft status max length should be 1 digit.";

            public const string irregularStatus = "Invalid Irregular Status";
            public const string irregularStatusMin = "The irregular status min length should be 1 digit.";
            public const string irregularStatusMax = "The irregular status max length should be 1 digit.";

            public const string scanStatus = "Invalid Scan Status";
            public const string scanStatusMin = "The Scan status min length should be 1 digit.";
            public const string scanStatusMax = "The Scan status max length should be 1 digit.";

            public const string sglOrOgl = "Invalid SGL or OGL";
            public const string sglOrOglLength = "The SGL or OGL max length should be 1 Letter.";

            public const string servicecharge = "Invalid service charge";
            public const string rezetapamount = "Invalid rezetap amount";

            public const string reportedBy = "Invalid Reported By";
            public const string reportedByMin = "The Reported By  min length should be 1 Letter.";
            public const string reportedByMax = "The Reported By  max length should be 8 Letter.";

            public const string exposureAmt = "Invalid Exposure Amount";
            public const string kycCircular = "Invalide KYC Circular";
            public const string kycAddressMatch = "Invalid KYC Address";
            public const string balanceReceive = "Invalid balance receive format";

            
            public const string branchIdMinLength = "The branch id min length should be 1 digits.";
            public const string branchIdMaxLength = "The branch id max length should be 4 digits.";

            public const string minimumValue = "Please use valid minimum value";
            public const string disbursementValue = "Please use valid disbursement value";
            public const string maturityValue = "Please use valid maturity value";
            public const string settlementValue = "Please use valid settlement value";
            public const string jewelBill = "Please use valid jewel bill value";

            public const string pwantktbutton = "Please use valid pawnticket button value";
            public const string pagenumber = "Please use valid pagenumber";
        }

        public static class InventoryItemconsts
        {
            public const int item_id = 4;
            public const int item_count = 4;
            public const int purity_id = 4;
            public const int itemType = 4;
            public const int pinSerialLength = 6;
            public const int customerIDLength = 14;
            public const int pledgeNoLength = 16;
            public const int vehicleLoanNumberLength = 22;
            public const int otpLength = 5;
            public const int transID = 8;

        }
    }
}
