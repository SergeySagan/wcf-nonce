using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WCFNonce.Common;

namespace WCFNonce.Service
{
    // Learn more: 
    // https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/custom-token
    // Metadata with a custom security token not working:
    // https://social.msdn.microsoft.com/Forums/en-US/1e3a71bc-7cef-414e-80dc-711cdea370e0/error-requesting-metadata-with-sample-about-customtoken?forum=wcf

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class NonceService : INonceService
    {
        public string Echo(string value)
        {
            if (!ServiceHelpers.IsNonceValid())
                throw new UnauthorizedAccessException();

            return string.Format("You entered: {0}", value);
        }
    }
}