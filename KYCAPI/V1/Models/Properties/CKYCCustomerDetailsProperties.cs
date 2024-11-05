using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class CKYCCustomerDetailsProperties
    {
        public string CUST_ID { get; set; }
        public long NAME_PFX { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public long FAT_PFX { get; set; }
        public string FAT_F_NAME { get; set; }
        public string FAT_M_NAME { get; set; }
        public string FAT_L_NAME { get; set; }
        public long FAT_SPOUSE_FLG { get; set; }
        public long MOTHER_PFX { get; set; }
        public string MOTH_F_NAME { get; set; }
        public string MOTH_M_NAME { get; set; }
        public string MOTH_L_NAME { get; set; }
        public long JURIS_RESID { get; set; }
        public string TAX_ID_NUM { get; set; }
        public long BIRTH_COUNTRY { get; set; }
        public long ADDR_TYPE { get; set; }
        public long USER_ID { get; set; }
        public DateTime TRA_DT { get; set; }
        public long CONFIRM_BY { get; set; }
        public DateTime CONFIRM_DT { get; set; }
        public long STATUS_ID { get; set; }

    }
}
