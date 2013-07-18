﻿// Copyright [2011] [PagSeguro Internet Ltda.]
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Net;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;

namespace CreatePayment
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Substitute the parameters below with your credentials
            //AccountCredentials credentials = new AccountCredentials("your@email.com", "your_token_here");
            AccountCredentials credentials = PagSeguroConfiguration.Credentials;

            try
            {

                // Instantiate a new payment request
                PaymentRequest payment = new PaymentRequest();

                // Sets the currency
                payment.Currency = Currency.Brl;

                // Add an item for this payment request
                payment.Items.Add(new Item("0001", "Notebook Prata", 1, 2430.00m));
                
                
                // Add another item for this payment request
                payment.Items.Add(new Item("0002", "Notebook Rosa", 2, 150.99m));

                // Sets a reference code for this payment request, it is useful to identify this payment in future notifications.
                payment.Reference = "REF1234";

                // Sets shipping information for this payment request
                payment.Shipping = new Shipping();
                payment.Shipping.ShippingType = ShippingType.Sedex;

                //Passando valor para ShippingCost
                //payment.Shipping.Cost = 123.00m;

                payment.Shipping.Address = new Address(
                    "BRA", 
                    "SP", 
                    "Sao Paulo", 
                    "Jardim Paulistano", 
                    "01452002", 
                    "Av. Brig. Faria Lima", 
                    "1384", 
                    "5o andar"
                );
                
                // Sets your customer information.
                payment.Sender = new Sender(
                    "Joao Comprador", 
                    "comprador@uol.com.br", 
                    new Phone("11", "56273440")
                );

                // Sets the url used by PagSeguro for redirect user after ends checkout process
                payment.RedirectUri = new Uri("http://www.lojamodelo.com.br");
                
                // Add checkout metadata information
                payment.AddMetaData(MetaDataItemKeys.GetItemKeyByDescription("CPF do passageiro"), "123.456.789-09", 1);
                payment.AddMetaData("PASSENGER_PASSPORT", "23456", 1);

                // Another way to set checkout parameters
                payment.AddParameter("senderBirthday", "07/05/1980");
                payment.AddIndexedParameter("itemColor", "verde", 1);
                payment.AddIndexedParameter("itemId", "0003", 3);
                payment.AddIndexedParameter("itemDescription", "Mouse", 3);
                payment.AddIndexedParameter("itemQuantity", "1", 3);
                payment.AddIndexedParameter("itemAmount", "200.00", 3);

                SenderDocument senderCPF = new SenderDocument(Documents.GetDocumentByType("CPF"), "12345678909"); 
                payment.Sender.Documents.Add(senderCPF);

                Uri paymentRedirectUri = payment.Register(credentials);

                Console.WriteLine("URL do pagamento : " + paymentRedirectUri);
                Console.ReadKey();
            }
            catch (PagSeguroServiceException exception)
            {
                if (exception.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized: please verify if the credentials used in the web service call are correct.\n");
                }
                Console.ReadKey();
            }
        }
    }
}
