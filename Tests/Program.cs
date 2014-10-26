using System;
using System.Collections.Generic;
using CoinvoyAPI;


namespace Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			var cv = new Coinvoy();

			Console.WriteLine("Testing Coinvoy c# client");

            //NEW INVOICE
            Console.WriteLine("Creating a new ESCROW INVOICE.");
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("email", "alpdeniz@gmail.com");
            options.Add("provider", "Kodaman");
            options.Add("description", "Ode, kendini ozel hisset");
            options.Add("escrow", "true");

            var invoiceResultObj = cv.invoice("0.001", "1JZbrknYYEEySTJSrsrECg4vpmitZ1s8wb", "BTC", options);

            Console.WriteLine("Invoice ID is: "+invoiceResultObj.id+" and invoice url is: "+invoiceResultObj.url+" and the key is: " + invoiceResultObj.key);
            if (invoiceResultObj.success)
            {
                Console.WriteLine("Create invoice: OK");
            }

            //FREE AN ESCROW
            Console.WriteLine("Freeing the escrow. ID: 4VrQ0Bq4vw");
            var escrowResultObj = cv.freeEscrow("3DXVLSJMOFAXF2ZJIGIFN27WHPZDXSDQY6ZCYL2VYN4HYHNE4OXA73YLKNYSY6B46CQDLDCM63QVG===");
            Console.WriteLine("Free escrow result is: " + escrowResultObj.success);
            if (escrowResultObj.success)
            {
                Console.WriteLine("Free Escrow: OK");
            }

            //INVOICE INFORMATION
            Console.WriteLine("Getting invoice information: " + invoiceResultObj.id);
            var invoiceObj = cv.getInvoice(invoiceResultObj.id);
            Console.WriteLine("Invoice payment address is: " + invoiceObj.address);
            if (invoiceObj.success)
            {
                Console.WriteLine("Get invoice: OK");
            }

            //INVOICE STATUS
            Console.WriteLine("Getting invoice status: "+invoiceResultObj.id);
            var statusObj = cv.getStatus(invoiceResultObj.id);

            Console.WriteLine("Status: " + statusObj.status);
            if (statusObj.success)
            {
                Console.WriteLine("Get status: OK");
            }

            //CREATE BUTTON
            Console.WriteLine("Creating a new BUTTON.");
            options = new Dictionary<string, string>();
            options.Add("email", "alpdeniz@gmail.com");
            options.Add("provider", "Kodaman");
            options.Add("description", "Ode, kendini ozel hisset");

            var buttonResultObj = cv.button("0.001", "1JZbrknYYEEySTJSrsrECg4vpmitZ1s8wb", "BTC", options);

            Console.WriteLine("Invoice hash is: " + buttonResultObj.hash + " and invoice url is: " + buttonResultObj.url);
            if (buttonResultObj.success)
            {
                Console.WriteLine("Create button: OK");
            }

            //CREATE DONATION BUTTON
            Console.WriteLine("Creating a new DONATION button.");
            options = new Dictionary<string, string>();
            options.Add("email", "alpdeniz@gmail.com");
            options.Add("provider", "Kodaman");
            options.Add("description", "Ode, kendini ozel hisset");

            var donationResultObj = cv.donation("1JZbrknYYEEySTJSrsrECg4vpmitZ1s8wb", options);

            Console.WriteLine("Invoice hash is: " + donationResultObj.hash + " and invoice url is: " + donationResultObj.url);
            if (donationResultObj.success)
            {
                Console.WriteLine("Create donation button: OK");
            }
			

			Console.ReadLine();
		}
	}
}
