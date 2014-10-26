using System;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace CoinvoyAPI
{
	// Documentation is at https://coinvoy.net/developers
    // Methods:
    //
    // getInvoice           - Get invoice information
    // getStatus            - Get invoice status
    // invoice              - Create new invoice
    // button               - Create new button for client-side use
    // donation             - Create new donation button without any defined amount
    // freeEscrow           - Release a confirmed escrow payment and forward it to seller
    // validateNotification - Securely validate an incoming IPN (payment notification)

	
	public class Coinvoy
    {
		//private string URL_BASE = "http://178.62.254.129/api/";
        private string URL_BASE = "https://coinvoy.net/api/";
        private HttpClient client;
		

        public Coinvoy()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(URL_BASE);
        }

		// Invoice Information
		public InvoiceInfo getInvoice(string invoiceID)
		{

            string url = URL_BASE + "invoice/" + invoiceID;
            var result = client.GetAsync(url).Result;
            HttpContent response = result.Content;

            string res = response.ReadAsStringAsync().Result;
            dynamic obj = JsonConvert.DeserializeObject<InvoiceJson>(res);

            return new InvoiceInfo(obj);
		}

		// INVOICE STATUS
		public StatusInfo getStatus(string invoiceID)
		{
            string url = URL_BASE + "status/" + invoiceID;
            var result = client.GetAsync(url).Result;
            HttpContent response = result.Content;

            string res = response.ReadAsStringAsync().Result;
            dynamic obj = JsonConvert.DeserializeObject<StatusJson>(res);

            return new StatusInfo(obj);
		}

        // CREATE NEW INVOICE
        public InvoiceResult invoice(string amount, string address, string currency, Dictionary<string, string> parameters)
        {

            parameters.Add("amount", amount);
            parameters.Add("address", address);
            parameters.Add("currency", currency);
            var content = new FormUrlEncodedContent(parameters);

            var result = client.PostAsync("newInvoice", content).Result;
            HttpContent response = result.Content;

            string res = response.ReadAsStringAsync().Result;
            dynamic obj = JsonConvert.DeserializeObject<InvoiceResultJson>(res);

            return new InvoiceResult(obj);
        }

        public ButtonResult button(string amount, string address, string currency, Dictionary<string, string> parameters)
        {

            parameters.Add("amount", amount);
            parameters.Add("address", address);
            parameters.Add("currency", currency);
            var content = new FormUrlEncodedContent(parameters);

            var result = client.PostAsync("getButton", content).Result;
            HttpContent response = result.Content;

            string res = response.ReadAsStringAsync().Result;
            dynamic obj = JsonConvert.DeserializeObject<ButtonResultJson>(res);

            return new ButtonResult(obj);
        }

        public ButtonResult donation(string address, Dictionary<string, string> parameters)
        {

            parameters.Add("address", address);
            var content = new FormUrlEncodedContent(parameters);

            var result = client.PostAsync("getDonation", content).Result;
            HttpContent response = result.Content;

            string res = response.ReadAsStringAsync().Result;
            dynamic obj = JsonConvert.DeserializeObject<ButtonResultJson>(res);

            return new ButtonResult(obj);
        }

        public EscrowResult freeEscrow(string key)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("key", key);
            var content = new FormUrlEncodedContent(parameters);

            var result = client.PostAsync("freeEscrow", content).Result;
            HttpContent response = result.Content;

            string res = response.ReadAsStringAsync().Result;
            dynamic obj = JsonConvert.DeserializeObject<EscrowResultJson>(res);

            return new EscrowResult(obj);
        }

        public bool validateNotification(string invoiceID, string hash, string orderID , string address)
        {
            var encoding = new ASCIIEncoding();
            var text = encoding.GetBytes(orderID + ":" + invoiceID);
            var key = encoding.GetBytes(address);
            var sha256 = new HMACSHA256(key);
            var signature = BitConverter.ToString(sha256.ComputeHash(text));

            if(hash == signature)
            {
                return true;
            }
            return false;
        }

	}

}
