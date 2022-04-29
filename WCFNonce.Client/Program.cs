using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using WCFNonce.Client.Helpers;
using WCFNonce.Common;

namespace WCFNonce.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Binding nonceBinding = ServiceHelpers.CreateNonceBinding();

            var serviceAddress = new EndpointAddress("http://localhost:63227/NonceService.svc");

            var channelFactory = new ChannelFactory<INonceService>(nonceBinding, serviceAddress);

            var myIP = "::1";

            var credentials = new NonceClientCredentials(ServiceHelpers.CreateNonceModel(myIP));

            credentials.ServiceCertificate.SetDefaultCertificate(
                  "CN=localhost", StoreLocation.LocalMachine, StoreName.My);

            channelFactory.Endpoint.Behaviors.Remove(typeof(ClientCredentials));
            channelFactory.Endpoint.Behaviors.Add(credentials);

            var client = channelFactory.CreateChannel();

            string serviceResponse = client.Echo("Test");

            ((IChannel)client).Close();
            channelFactory.Close();

            Console.WriteLine(serviceResponse);
            Console.ReadKey();
        }
    }
}