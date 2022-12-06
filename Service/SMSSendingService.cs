using Meeting_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Meeting_App.Service
{
    public class SMSSendingService
    {
		private string MSISDN;
		private string Password;
		
		public SMSSendingService(string msisdn, string password)
		{
			MSISDN = msisdn;
			this.Password = password;
		
		}
		public SMSSendingService()
		{

			
		}

		//public SMSResponseViewModel GetSessionId()
		//{
		//	try
		//	{
		//		string url = "https://telenorcsms.com.pk:27677/corporate_sms2/api/auth.jsp?msisdn=" + MSISDN + "&password=" + Password;
		//		var sessionId = sendRequest(url);
		//		return sessionId;
		//	}
		//	catch (Exception ex)
		//	{
		//		throw;
		//	}
		//}

		public SMSViewModel SendQuickMessage(SMSViewModel sms)
		{
			try
			{
				SMSViewModel model = new SMSViewModel();
				//model.SmssessionId = GetSessionId().Data;
				//string url = "https://telenorcsms.com.pk:27677/corporate_sms2/api/sendsms.jsp?session_id=" + model.SmssessionId + "&text=" + sms.Body + "&to=" + sms.Receiver;
				//string url = @"https://bsms.ufone.com/bsms_v8_api/sendapi-0.3.jsp?id=" + sms.Sender + "&message=" + sms.Body + "&shortcode=" + sms.Mask + "&lang=English&mobilenum=" + sms.Receiver + "&password=Hi$DU@112345&messagetype=Nontransactional";
				
				string url = @"https://connect.jazzcmt.com/sendsms_url.html?Username=03018482714&Password=Jazz@123&From=" + sms.Mask + "&To=" + sms.Receiver + "&Message=" + sms.Body + "&Identifier=123456&UniqueId=123456789&ProductId=123456789&Channel=123456789&TransactionId=123456789";
				
				if (sms.Mask != null)
				{
					url = url += "&mask=" + sms.Mask;
				}
				model.Body = sms.Body;
				model.Sender = sms.Sender;
				model.Receiver = sms.Receiver;
				model.Mask = sms.Mask;
				model.CreatedBy = sms.CreatedBy;
				model.SentDate = DateTime.UtcNow.AddHours(5);
				model.Fkid = model.Fkid;
				model.Note = sms.Note;
				var result = SendRequest(url);
				//model.MessageId = result.Data;
				//model.Status = string.IsNullOrEmpty(result.Data) ? "Error" : "Sent";
				//model.StatusResponse = result.Response;
				//SaveSMSHistory(model);
				return model;
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		public string SendRequest(string url)
		{
			string response = null;
			try
			{
				System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
				var client = new WebClient();
				Uri smsUri = new Uri(url);
				response = client.DownloadString(smsUri);
				XmlDocument xmldoc = new XmlDocument();
				//xmldoc.LoadXml(response);
				//XmlNodeList responseType = xmldoc.GetElementsByTagName("response_to_browser");
				//XmlNodeList data = xmldoc.GetElementsByTagName("response_id");
				//XmlNodeList text = xmldoc.GetElementsByTagName("response_text");
				//if (responseType.Equals("Error"))
				//{
				//    return null;
				//}
				//string responseId = data[0].InnerText;
				string responseText = response;
				return responseText.Replace("Successful:", "");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return null;
		}


		//public void SaveSMSHistory(SMSViewModel model)
		//{
		//	using (var db = new UnitOfWork<AppSmsHistory>())
		//	{
		//		AppSmsHistory his = new AppSmsHistory();
		//		his.Sender = model.Sender;
		//		his.Receiver = model.Receiver;
		//		his.Mask = model.Mask;
		//		his.SentDate = model.SentDate;
		//		his.Note = model.Note;
		//		his.SmssessionId = model.SmssessionId;
		//		his.MessageId = model.MessageId;
		//		his.Fkid = model.Fkid;
		//		his.Body = model.Body;
		//		his.CreatedBy = model.CreatedBy;
		//		his.SentDate = model.SentDate;
		//		his.Status = model.Status;
		//		his.StatusResponse = model.StatusResponse;
		//		db.Repository.Insert(his);
		//		db.Save();
		//	}
		//}
	}
	public class SMSResponseViewModel
	{
		public string Data { get; set; }
		public string Response { get; set; }
	}
}
