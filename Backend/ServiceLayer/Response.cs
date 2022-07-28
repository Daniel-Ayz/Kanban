using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        public string ErrorMessage { get; set; }
        public object ReturnValue { get; set; }


        public Response()
        {
        }
      
        public Response(string message, object obj = null)
        {
            this.ErrorMessage = message;
            this.ReturnValue = obj;
        }

        public override bool Equals(object obj)
        {
            if(obj is Response)
            {
                Response response = (Response)obj;
                bool messageEquals = false;
                if(response.ErrorMessage != null && this.ErrorMessage != null)
                    messageEquals= response.ErrorMessage == this.ErrorMessage;
                else 
                    messageEquals=(response.ErrorMessage ==null && this.ErrorMessage==null);
                bool returnValueEquals = false;
                if(response.ReturnValue != null && this.ReturnValue != null)
                    returnValueEquals = response.ReturnValue==(this.ReturnValue);
                else
                    returnValueEquals = (response.ReturnValue == null && this.ReturnValue == null);
                return messageEquals && returnValueEquals;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ErrorMessage, ReturnValue);
        }

        public override string ToString()
        {
            string message = string.Empty;
            if(ErrorMessage != null)
                message+="ErrorMessage: "+ErrorMessage;
            if (ReturnValue != null)
                message += " ReturnValue: " + ReturnValue;
            return message;
        }
    }
}

