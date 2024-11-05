using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1.TokenAttribute
{
    public interface IToken
    {
        bool isValidToken(string userContextObj);
    }
}
