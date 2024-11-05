using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary
{
    public interface IEmployee
    {
        bool CheckUser(int userName, string password);
        bool CheckUser(int userName, string password, ref int access_id, ref int role_id, ref int emp_branch, ref int passwd_flag);
        byte[] getEdata(string userName, string password);

        string jsDecrypt(string userName, string password);
    }
}
