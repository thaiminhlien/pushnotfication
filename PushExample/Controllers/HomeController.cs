using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;
using WebPush;

namespace WebApplication11.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }
        public void action(WebApplication11.Models.Entities.ClientInformation e,string message)
        {
            //var pushEndpoint = @"https://fcm.googleapis.com/fcm/send/elpgb-hTYfE:APA91bEbOObMfgrR2nc-kjfhm1iQscT6hcqGFDXn_IGYFMs5n5KCGIclzMAC_WZDbA34om0Ub9sqyhR1i_3Y0BQPhiZp1CnDWjTr74exx8wJp8KBFbMWhlzNWQJ3zA3LkM8rm2ZkQCjS";
            //var p256dh = @"BASOQRL625ssxHi4QiPRy-Tu9dih25IcVECTGfWqIIROwpq3xsWuwN3bpX3zWF_rv-jSCG08rkv4M2uAhYUnJSQ=";
            //var auth = @"Y6tPjfsSmb8aUPdNaJzBXA==";

            var subject = @"mailto:tmlienit@example.com";
            var publicKey = @"BD8r6N3kP4649YLBtRfaLn4rMPxSyMVUsE7sbE3acIlgk51SximXV2SlT8_TWpU5DYKBuFD8ZeZQyi0yS85Q4FY";
            var privateKey = @"z4wKrFkzEj-XQPpqPCZwaYsLQwctAzJHkbi2UykduLg";

            var subscription = new PushSubscription(e.pushEndpoint, e.p256dh, e.auth);
            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
            //var gcmAPIKey = @"[your key here]";

            var webPushClient = new WebPushClient();
            try
            {
                webPushClient.SendNotification(subscription, message, vapidDetails);
                //webPushClient.SendNotification(subscription, "payload", gcmAPIKey);
            }
            catch (WebPushException exception)
            {
                Console.WriteLine("Http STATUS code" + exception.StatusCode);
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult push(string message)
        {
            using (var ent = new WebApplication11.Models.Entities.sampleEntities())
            {
                var all = ent.ClientInformations;
                foreach (var i in all)
                {
                    action(i, message);
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult insert(string jsonData)
        {
            var id = -1;
            var model = JsonConvert.DeserializeObject<subscription>(jsonData);
            using (var ent = new WebApplication11.Models.Entities.sampleEntities())
            {
                var exist = ent.ClientInformations.Where(u => u.pushEndpoint.Equals(model.endpoint)).FirstOrDefault();
                if(exist == null)
                {
                    var newmodel = new Models.Entities.ClientInformation
                    {
                        auth = model.keys.auth,
                        p256dh = model.keys.p256dh,
                        pushEndpoint = model.endpoint
                    };

                    ent.ClientInformations.Add(newmodel);
                    ent.SaveChanges();
                    id = newmodel.Id;

                }
                else
                {
                    id = exist.Id;
                    exist.p256dh = model.keys.p256dh;
                    exist.auth = model.keys.auth;
                    ent.SaveChanges();
                }
            }
            return Json(new { result = "ok" , id  = id }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult remove(int id)
        {
          
            using (var ent = new WebApplication11.Models.Entities.sampleEntities())
            {
                var exist = ent.ClientInformations.Find(id);
                if(exist!=null)
                {
                    ent.ClientInformations.Remove(exist);
                    ent.SaveChanges();
                }
               
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}