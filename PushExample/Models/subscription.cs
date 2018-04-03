using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PushExample.Models
{
    public class subscription
    {
        public string endpoint { get; set; }
        public keys keys { get; set; }
    }
    public class keys
    {
        public string p256dh { get; set; }
        public string auth { get; set; }
    }
}