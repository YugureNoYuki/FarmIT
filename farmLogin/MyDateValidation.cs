using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin
{
    public class MyDate : ValidationAttribute
    {

        public MyDate()
        {
        }

        public override bool IsValid(object value)
        {
            if(value != null)
            {
                var dt = (DateTime)value;
                if (dt <= DateTime.Now)
                {
                    return true;
                }
                return false;
            }
            else return true;
            
        }
    }
}