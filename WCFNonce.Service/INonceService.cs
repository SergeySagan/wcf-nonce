using System.ServiceModel;

namespace WCFNonce.Service
{
    [ServiceContract]
    public interface INonceService
    {

        [OperationContract]
        string Echo(string value);
    }
}