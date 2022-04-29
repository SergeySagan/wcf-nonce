using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using WCFNonce.Common;

namespace WCFNonce.Service.Helpers
{
    public class NonceServiceHostFactory : ServiceHostFactoryBase
    {
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            return new NonceServiceHost(baseAddresses);
        }

        class NonceServiceHost : ServiceHost
        {
            public NonceServiceHost(params Uri[] addresses): base(typeof(NonceService), addresses)
            {
            }

            override protected void InitializeRuntime()
            {
                var serviceCredentials = new NonceServiceCredentials();

                serviceCredentials.ServiceCertificate.SetCertificate("CN=localhost", 
                                                                     StoreLocation.LocalMachine, 
                                                                     StoreName.My);

                this.Description.Behaviors.Remove((typeof(ServiceCredentials)));
                this.Description.Behaviors.Add(serviceCredentials);

                this.AddServiceEndpoint(typeof(INonceService), ServiceHelpers.CreateNonceBinding(), string.Empty);

                base.InitializeRuntime();
            }
        }
    }
}