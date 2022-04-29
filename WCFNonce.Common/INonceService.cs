using System.ServiceModel;

namespace WCFNonce.Common
{
    [ServiceContract]
    public interface INonceService
    {

        [OperationContract]
        string Echo(string value);
    }
}