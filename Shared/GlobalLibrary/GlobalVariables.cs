using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalLibrary
{
    public class GlobalVariables
    {
        public enum ProcessStatus
        {
            success = 1,
            failed = 0
        }

        public enum LoginStatus
        {
            validUser = 1,
            inValidUser = 0

        }
        public enum APIStatus
        {
            success = 1,
            failed = 0,
            exception = 2,
            pagenotfound = 4,
            internalexception = 5,
            invalidParameter = 6,
            invalidParameterValue = 7,
            SRM_RM_Approval_Required = 8,
            no_Data_Found = 9,
            faileTOGenerateToken = 10,
            validationFailed = 11,
            alreadyExist = 12,
            neftModificationRequired = 13,
            invalidIFSCCode = 14,
            invalidRequest = 15,
            smsRegistrationCode = 16,
            loanRejected = 17,
            badRequest = 18,
            invalidUser = 19,
            otherBranchValidation = 20,
            invalidCustomerID = 21,
			fullSettlementabove7=22

		}
        public enum LoginFormMode
        {
            Login = 1,
            Add_Inventory = 2,
            GL1_Verification = 3,
            GL2_Verification = 4,
            ABH_Verification = 5,
            BH_Verification = 6,
            Repledge = 7,
            Settlement = 8,
            Inventory_Release = 9,
            FormK = 10,
            CashNeftDisburse = 11,
            MaxValue = 12,
            AccountsEntry = 13,
            Report=14
        }

        public enum Durations
        {
            LoginOTP = 30,
            Token = 30,
        }
        public enum LostCustomerStatus
        {
            valid = 1,
            inValid = 0,
        }
        public static string getAPIResponseMessages(APIStatus status)
        {
            string msg = "";
            switch (status)
            {
                case APIStatus.success:
                    break;
                case APIStatus.failed:
                    break;
                case APIStatus.exception:
                    break;
                case APIStatus.pagenotfound:
                    break;
                case APIStatus.internalexception:
                    break;
                case APIStatus.invalidParameter:
                    break;
                case APIStatus.invalidParameterValue:
                    break;
                case APIStatus.SRM_RM_Approval_Required:
                    break;
                case APIStatus.no_Data_Found:
                    break;
                case APIStatus.faileTOGenerateToken:
                    break;
                default:
                    break;
            }
            return msg;
        }
        public class ResponseStatus
        {
            public ProcessStatus flag { get; set; }
            public APIStatus code { get; set; }
            public string message { get; set; }
            public string timeStamp { get; set; }
        }

        public class Constants
        {
            public static class Strings
            {
                public const string dateFormat = "dd-MMM-yyyy";
                public const string cashFormat = "#.00";
                public const string dateTimeFormat = "dd-MMM-yyyy hh:mm:ss";
                public const string nameFormat = @"^[a-zA-Z\s]+$";
                public const string numberFormat = "^[0-9]+$";
                public const string alphaNumericFormat = "^[a-zA-Z0-9]*$";
                public const string panNoFormat = @"^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$";
                public const string zeroOneFormat = "^[0-1]{0,1}$";
                public const string decimalFormat = @"^\d+.\d{0,2}$";
                public const string gl1decimalFormat = @"^\d+.\d{0,3}$";                
                public const string OutStandingApprovalMode_VERIFYFORGL = "VERIFYFORGL";
                public const string OutStandingApprovalMode_SELREGFORAPPR = "SELREGFORAPPR";
                public const string schemeNameFormat = "^[A-Z]*$";
                public const string custnameFormat = @"^[a-zA-Z\s\.]+$";
            }

            public static class Ints
            {
                public const int rangeValidatorFrom_0 = 0;
                public const int rangeValidatorTo = int.MaxValue;
                public const int rangeValidatorFrom_Neg1 = -1;
                public const int rangeValidatorFrom_1 = 1;
                public const int rangeValidatorFrom_2 = 2;
                public const int rangeValidatorFrom_4 = 4;
                public const int mobileNumberLength = 10;
                public const int pinSerialLength = 6;
                public const int customerIDLength = 10;
                public const int createdBy = 7;
                public const int reportedBy = 8;
                public const int pledgeNoLength = 16;
                public const int vehicleLoanNumberLength = 22;
                public const int otpLength = 5;
                public const int inventoryitemMaxValue = 9999;
                public const int zeroOneMaxValue = 10;
                public const int messageLength = 10;
                public const int mPin = 45;
                public const int inventoryNoLength = 16;
                public const int inventoryQutaionNoLength = 16;
                public const double rangeValidatorDouble = 0.00;
                public const int branchIdLength = 4;
                public const int branchIdMaxLength = 4;
                public const int branchIdMinLength = 1;
                public const int smsmessageLength = 160;
				public const int quotNoLength = 16;


			}
            public class InventoryValidation
            {
                public const int rangeValidatorFrom_1 = 1;
                public const int rangeValidatorFrom_2 = 2;

                public const int inventoryNo_AplphaNumeric = 16;
                public const int inventoryCharge = 5;
                public const int otherCharge = 3;
                public const int totalAmount = 6;
            }
            public class CustomerValidation
            {
                public const int customerIDLength = 10;
                public const int customerNameLength = 50;

            }
            public static class TokenStatus
            {

                public const int open = 1;
                public const int manualExpired = 0;
                public const int autoExpired = 2;
            }

            public static class LoginFlags
            {
                public const int Flag_Success = 1;
                public const int Flag_Failed = 0;
                public const int Flag_NoMobileNumber = 2;
                public const int Flag_OTP_Sent = 3;
                public const int Flag_OTP_Already_Sent = 4;
            }
        }

        public class FormMode
        {
            public enum SchemeAPI
            {
                QuarterlyInterestSalb = 1,
                slab_interest = 2,
                FestivalOfferSchemes = 3,
                GetProcessingFee = 4,
                DocumentCharges = 5,
                GetEffrate = 6
            }
        }

        public enum QuotNoType
        {
            inventoryQuotNo = 1,
            inventoryNo = 2,
            pledgeNo = 3

        }

        public enum LanguageType
        {
            local = 1,
            english = 2         

        }


        public enum QuotFormMode
        {
            gl2 = 1,
            abh = 2,
            bh = 3,
            doorstep = 4
        }
        public enum SMSAccMode
        {
           
            transactionsms = 2,
            OtpSms = 3
        }
        public enum ConfirmChoice
        {
            gL2=1,
            abh=2
        }
    
        public enum ConfirmStatus
        {
          
            no= 0,
            yes = 1,
        }

        public enum SMSStatus
        {
            callApi=0,
            received = 6,
            notreceived = 7
        }
        public enum smsSendStsatus
        {
            generateOtp = 0,
            verifyOtp = 1,
            afterVerifyOtp = 2
        }
        
        public enum CustomerOutStandingApprovalMode
        {
            VERIFYFORGL = 1,
            SELREGFORAPPR = 2
        }

        public enum CashBalanceMode
        {
            Cash = 1,
            Bank = 2,
            Swapping = 3
        }

        public enum PrintButtonValues
        {
            save = 1,
            print = 2,
            reprint = 3,
            close = 4
        }
        public enum PaymentMode
        {
            NEFT = 1,
            NEFT_Cash = 2,
            Yes_Bank = 3,
            Cheque=4
        }
        public enum PldgnoLostFocus
        {
            pledgeIsAlreadySettled = 1,
            pledgeFirmChecking = 2,
            banksettle = 3,
            oglRebate=4,
            pledgeStatus12And13=5,
            Kycstatus=6,
            wrongPledgeNumber = 7
        }

        public enum PledgeSettlementAccept
        {
            PayAmount = 1
        }
        public enum loanConfirmationAlert
        {
            loanAmount = 1,
            lostCustomer=2
        }
        public enum InventoryNoLostFocus
        {
            Pledgeissettled = 1,
            checkvalid=2
        }

        public enum InventoryReleaseConfirmMessage
        {
            msg1 = 1,
            msg2 = 2
        }
        public enum serviceCharge
        {
            msg1 = 1,
            msg2 = 2
        }

        public enum CheckStatus
        {
            Checked = 1,
            notChecked = 0
        }
        public enum RepldgnoLostFocus
        {
            pledgeIsAlreadySettled = 1,
            Messagecheck1 = 2,
            pledgeStatus12And13 = 3,
            Kycstatus = 4,
            wrongPledgeNumber = 5
        }
        public enum One2oneLostFocusMessage
        {
            masg1,
            masg2,
            masg3,
            masg4,
            masg5,
            masg6
        }
        public enum RedioStatus
        {
            True = 1,
            False = 0
        }

        public enum schemechoice
        {
            allscheme = 1,
            TRscheme = 2,
            NotTRscheme = 3
        }
    }
}
