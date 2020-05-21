using System;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    [Serializable()]
    public class CustomException : Exception
    {
        public CustomException() : base() { }
        public CustomException(string message) : base(message) { }
        public CustomException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable()]
    public class EnvioEmailException : Exception
    {
        public EnvioEmailException() : base() { }
        public EnvioEmailException(string message) : base(message) { }
        public EnvioEmailException(string message, Exception inner) : base(message, inner) { }
    }
}
