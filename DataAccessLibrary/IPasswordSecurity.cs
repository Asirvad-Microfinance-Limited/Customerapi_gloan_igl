using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary
{
    public interface IPasswordSecurity
    {
        string Encrypt(string Text);
        string Decrypt(string EncryptedText);
    }
}
