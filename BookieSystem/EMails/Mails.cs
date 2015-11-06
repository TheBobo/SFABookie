using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BookieSystem.EMails
{
    public class Email
    {
        public static void SendGroupEmail(string reciever, string subject, string body)
        {

            string emailSender = "bookiesf@gmail.com";
            string password = "highstrongpassword";
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSender, password)
            };


            client.Credentials = new System.Net.NetworkCredential(emailSender, password);



            MailAddress from = new MailAddress(emailSender,
               "ScaleFocus Bookie ", System.Text.Encoding.UTF8);

            MailAddress to = new MailAddress(reciever);
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

          
            client.Send(message);

        }
    }
}