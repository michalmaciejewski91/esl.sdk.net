using System;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;
using System.IO;

namespace SDK.Examples
{
    public class DesignerRedirectForApiKeyExample : SDKSample
    {
        public static void Main (string[] args)
        {
            new DesignerRedirectForApiKeyExample(Props.GetInstance()).Run();
        }

        public string GeneratedLinkToDesignerForApiKey{ get; private set; }
        private AuthenticationClient authenticationClient;
        private Stream fileStream;

        public DesignerRedirectForApiKeyExample( Props props ) : this(props.Get("api.url"), props.Get("api.key"), props.Get("webpage.url")) {
        }

        public DesignerRedirectForApiKeyExample( string apiKey, string apiUrl, string webpageUrl) : base( apiKey, apiUrl ) {
            this.authenticationClient = new AuthenticationClient(webpageUrl);
            this.fileStream = File.OpenRead(new FileInfo(Directory.GetCurrentDirectory() + "/src/document.pdf").FullName);
        }

        override public void Execute()
        {            
            DocumentPackage package = PackageBuilder.NewPackageNamed ("SignerAuthenticationTokenExample " + DateTime.Now)
                    .DescribedAs ("This is a new package")
                    .WithDocument(DocumentBuilder.NewDocumentNamed("My Document")
                                  .FromStream(fileStream, DocumentType.PDF))
                    .Build();

            PackageId packageId = eslClient.CreatePackage (package);

            string userAuthenticationToken = eslClient.AuthenticationTokenService.CreateUserAuthenticationToken();


            GeneratedLinkToDesignerForApiKey = authenticationClient.BuildRedirectToDesignerForUserAuthenticationToken(userAuthenticationToken, packageId);

            //This is an example url that can be used in an iFrame or to open a browser window with a session (created from the user authentication token) and a redirect to the designer page.
            System.Console.WriteLine("Designer redirect url: " + GeneratedLinkToDesignerForApiKey);
        }
    }
}
