# wcf-nonce

This project extends the standard WCF using Custom Tokens to allow Nonce verification.

Based heavily upon this documentaton:
https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/custom-token

And code:
https://github.com/ssickles/archive/tree/af9d07112e16d84d233adcb338ad29b243d90a59/samples/WCFWFSamples/WCF/Extensibility/Security/CustomToken/CS

Known issues:
This implementation is missing the custom implementation needed for the WSDL/metadata to display properly.
Not really an issue, just needs more code not included here. See more:
https://social.msdn.microsoft.com/Forums/en-US/1e3a71bc-7cef-414e-80dc-711cdea370e0/error-requesting-metadata-with-sample-about-customtoken?forum=wcf
