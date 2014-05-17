using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using log4net;
using SWPCCBilling.Models;
using SWPCCBilling.Properties;

namespace SWPCCBilling.Infrastructure
{
    public class InvoiceMailer
    {
        private readonly ILog _log;

        public InvoiceMailer(ILog log)
        {
            _log = log;
        }

        public void Send(Invoice invoice, string emailPassword, IList<string> emailTo)
        {
            var client = new SmtpClient
            {
                Host = Settings.Default.EmailServer,
                Port = Int32.Parse(Settings.Default.EmailPort),
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

            if (Settings.Default.EmailSSL)
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Settings.Default.EmailFrom, emailPassword);
            }

            var msg = new MailMessage
            {
                From = new MailAddress(Settings.Default.EmailFrom),
                Subject = String.Format("SWPCC {0:MMMM yyyy} Invoice for {1}", invoice.Date, invoice.FamilyName)
//                Subject = String.Format("SWPCC Summer Camp 2014 Invoice for {1}", invoice.Date, invoice.FamilyName)
            };

            foreach (string to in emailTo)
                msg.To.Add(to);

            msg.IsBodyHtml = true;
            msg.Body = invoice.HTMLBody;

            client.Send(msg);

            _log.InfoFormat("Invoice {0:MMMM yyyy} sent to {1} at:", invoice.Date, invoice.FamilyName);

            foreach (string to in emailTo)
                _log.InfoFormat("    {0}", to);
        }
    }
}