using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using log4net;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class InvoiceMailer
    {
        private readonly ILog _log;
		private readonly Settings _settings;

        public InvoiceMailer(ILog log, SettingsStore settingsStore)
        {
            _log = log;
			_settings = settingsStore.Load();
        }

        public void Send(Invoice invoice, string emailPassword, IList<string> emailTo)
        {
            var client = new SmtpClient
            {
                Host = _settings.EmailServer,
				Port = _settings.EmailPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

			if (_settings.EmailSSL)
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(_settings.EmailFrom, emailPassword);
            }

            var msg = new MailMessage
            {
				From = new MailAddress(_settings.EmailFrom),
                Subject = String.Format("SWPCC {0:MMMM yyyy} Invoice for {1}", invoice.Date, invoice.FamilyName)
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