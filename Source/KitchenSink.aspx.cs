using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Net.Http;


public partial class KitchenSink : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            primarySignerSection.Visible = true;
            jointSignerSection.Visible = false;

            button.Visible = true;
            button.InnerText = "Submit";
        }

        // Add event handlers for the navigation button on each of the wizard pages 
        PrefillClick.ServerClick += new EventHandler(prefill_Click);
        button.ServerClick += new EventHandler(button_Click);
    }

    protected void prefill_Click(object sender, EventArgs e)
    {
        firstname.Value = "Duncan";
        lastname.Value = "Heinz";
    }

    protected void button_Click(object sender, EventArgs e)
    {
        primarySignerSection.Visible = false;
        button.Visible = false;
        createEnvelope();

    }

    

    protected String RandomizeClientUserID()
    {
        Random random = new Random();

        return (random.Next()).ToString();
    }

    public class TextTab
    {
        public string tabLabel { get; set; }
        public string value { get; set; }
    }

    public class Tabs
    {
        public List<TextTab> textTabs { get; set; }
        public List<SignHereTab> signHereTabs { get; set; }
        public List<DateSignedTab> dateSignedTabs { get; set; }
    }

    public class Signer
    {

        public string email { get; set; }
        public string name { get; set; }
        public int recipientId { get; set; }
        public string roleName { get; set; }
        public Tabs tabs { get; set; }
        public string routingOrder { get; set; }
        public string clientUserId { get; set; }


    }

    public class Recipients
    {
        public List<Signer> signers { get; set; }
    }

    public class DisplaySettings
    {
        public string display { get; set; }
        public string tableStyle { get; set; }
        public string cellStyle { get; set; }
        public string inlineOuterStyle { get; set; }
        public string displayLabel { get; set; }
        public string labelWhenOpened { get; set; }
        public bool? hideLabelWhenOpened { get; set; }
        public bool? scrollToTopWhenOpened { get; set; }
    }

    public class DisplayAnchor
    {
        public string startAnchor { get; set; }
        public string endAnchor { get; set; }
        public bool removeStartAnchor { get; set; }
        public bool removeEndAnchor { get; set; }
        public bool caseSensitive { get; set; }
        public DisplaySettings displaySettings { get; set; }
    }

    public class HtmlDefinition
    {
        public int maxScreenWidth { get; set; }
        public string source { get; set; }
        public string removeEmptyTags { get; set; }
        public string headerLabel { get; set; }
        public string displayAnchorPrefix { get; set; }
        public List<DisplayAnchor> displayAnchors { get; set; }
    }

    public class Document
    {
        public string documentId { get; set; }
        public string name { get; set; }
        public string transformPdfFields { get; set; }
        public string fileExtension { get; set; }
        public HtmlDefinition htmlDefinition { get; set; }


    }

    public class InlineTemplate
    {
        public string sequence { get; set; }
        public Recipients recipients { get; set; }
        public List<Document> documents { get; set; }
    }

    public class ServerTemplate
    {
        public string sequence { get; set; }
        public string templateId { get; set; }
    }

    public class CompositeTemplate
    {
        public string compositeTemplateId { get; set; }
        public List<InlineTemplate> inlineTemplates { get; set; }
        public List<ServerTemplate> serverTemplates { get; set; }
        public Document document { get; set; }
    }

    public class CreateEnvelopeRequest
    {
        public string brandId { get; set; }

        public string status { get; set; }
        public string emailSubject { get; set; }
        public string emailBlurb { get; set; }
        public List<CompositeTemplate> compositeTemplates { get; set; }
    }
    public class CreateEnvelopeResponse
    {
        public string envelopeId { get; set; }
        public string uri { get; set; }
        public string statusDateTime { get; set; }
        public string status { get; set; }
    }

    public class SignHereTab
    {
        public string tabId { get; set; }
        public string name { get; set; }
        public string pageNumber { get; set; }
        public string documentId { get; set; }
        public string yPosition { get; set; }
        public string xPosition { get; set; }

        public string anchorString { get; set; }
        public string anchorXOffset { get; set; }
        public string anchorYOffset { get; set; }
        public string anchorIgnoreIfNotPresent { get; set; }
        public string anchorUnits { get; set; }
    }

    public class DateSignedTab
    {
        public string tabId { get; set; }
        public string name { get; set; }
        public string pageNumber { get; set; }
        public string documentId { get; set; }
        public string yPosition { get; set; }
        public string xPosition { get; set; }
        public string anchorString { get; set; }
        public string anchorXOffset { get; set; }
        public string anchorYOffset { get; set; }
        public string anchorIgnoreIfNotPresent { get; set; }
        public string anchorUnits { get; set; }

    }


    private static void WriteStream(Stream reqStream, string str)
    {
        byte[] reqBytes = UTF8Encoding.UTF8.GetBytes(str);
        reqStream.Write(reqBytes, 0, reqBytes.Length);
    }


    private String GetSecurityHeader()
    {
        String str = "";
        str = "<DocuSignCredentials>" + "<Username>" + ConfigurationManager.AppSettings["API.Email"] + "</Username>" +
            "<Password>" + ConfigurationManager.AppSettings["API.Password"] + "</Password>" +
            "<IntegratorKey>" + ConfigurationManager.AppSettings["API.IntegratorKey"] + "</IntegratorKey>" +
            "</DocuSignCredentials>";
        return str;
    }

    public class RecipientViewRequest
    {
        public string authenticationMethod { get; set; }
        public string email { get; set; }
        public string returnUrl { get; set; }
        public string userName { get; set; }
        public string clientUserId { get; set; }
    }


    public class RecipientViewResponse
    {
        public string url { get; set; }
    }

    protected void createEnvelope()
    {


        // Set up the envelope
        CreateEnvelopeRequest createEnvelopeRequest = new CreateEnvelopeRequest();
        createEnvelopeRequest.emailSubject = "Kitchen Sink Example";
        createEnvelopeRequest.status = "sent";
        createEnvelopeRequest.emailBlurb = "Example of how different smart anchors work";

        // Define first signer 
        Signer signer = new Signer();
        signer.email = email.Value;
        signer.name = firstname.Value + " " + lastname.Value;
        signer.recipientId = 1;
        signer.routingOrder = "1";
        signer.roleName = "Signer1";
        signer.clientUserId = RandomizeClientUserID();  // First signer is embedded 

        // Add tabs for the signer
        signer.tabs = new Tabs();
        signer.tabs.signHereTabs = new List<SignHereTab>();
        SignHereTab signHereTab = new SignHereTab();
        signHereTab.documentId = "1";
        signHereTab.tabId = "1";
        signHereTab.anchorString = "~?";
        signHereTab.anchorIgnoreIfNotPresent = "true";
        signHereTab.name = "PrimarySignerSignature";
        signer.tabs.signHereTabs.Add(signHereTab);

    
        signer.tabs.dateSignedTabs = new List<DateSignedTab>();
        DateSignedTab dateSignedTab = new DateSignedTab();
        dateSignedTab.documentId = "1";
        dateSignedTab.tabId = "2";
        dateSignedTab.anchorString = "~!";
        dateSignedTab.anchorIgnoreIfNotPresent = "true";
        dateSignedTab.name = "PrimarySignerDateSigned";
        signer.tabs.dateSignedTabs.Add(dateSignedTab);

        // Define a document 
        Document document = new Document();
        document.documentId = "1";
        document.name = "Sample Form";
        document.transformPdfFields = "true";
        document.fileExtension = "doc";
        document.htmlDefinition = new HtmlDefinition();
        document.htmlDefinition.source = "document";
        document.htmlDefinition.displayAnchorPrefix = "";
        document.htmlDefinition.maxScreenWidth = 0;
        document.htmlDefinition.removeEmptyTags = "table,tr,p";
        document.htmlDefinition.headerLabel = "<h1 style='color:#E20074;text-align:center;font-family:Tele-Grotesk, Arial, Helvetica;'>Examples of Smart Sections</h1><h2 style = 'text-align:center;font-family:Tele-Grotesk, Arial, Helvetica;'> Simply scroll down to review.</ h2><div style = 'margin-top:20px;margin-bottom:20px;border-bottom:solid 2px #dedede;'></ div >";

        // Define the display anchors 
        document.htmlDefinition.displayAnchors = new List<DisplayAnchor>();
        DisplayAnchor displayAnchor = new DisplayAnchor();
        displayAnchor.startAnchor = "$Docu$printonlyS$";
        displayAnchor.endAnchor = "$Docu$printonlyE$";
        displayAnchor.removeEndAnchor = true;
        displayAnchor.removeStartAnchor = true;
        displayAnchor.caseSensitive = true;
        displayAnchor.displaySettings = new DisplaySettings();
        displayAnchor.displaySettings.display = "print_only";
        displayAnchor.displaySettings.tableStyle = "";
        displayAnchor.displaySettings.cellStyle = "";
        displayAnchor.displaySettings.labelWhenOpened = "";
        displayAnchor.displaySettings.scrollToTopWhenOpened = true;
        displayAnchor.displaySettings.hideLabelWhenOpened = true;
        displayAnchor.displaySettings.displayLabel = "";
        document.htmlDefinition.displayAnchors.Add(displayAnchor);

        DisplayAnchor displayAnchor2 = new DisplayAnchor();
        displayAnchor2.startAnchor = "$tmo$tila$";
        displayAnchor2.endAnchor = "$tmo$tila$";
        displayAnchor2.removeEndAnchor = true;
        displayAnchor2.removeStartAnchor = true;
        displayAnchor2.caseSensitive = true;
        displayAnchor2.displaySettings = new DisplaySettings();
        displayAnchor2.displaySettings.display = "responsive_table_single_column";
        displayAnchor2.displaySettings.tableStyle = "margin-bottom:20px;";
        displayAnchor2.displaySettings.cellStyle = "text-align:center;border:solid 3px #333;margin:5px;padding:5px;background-color:#eaeaea;~text-align:center;border:solid 3px #333;margin:5px;padding:5px;background-color:#eaeaea;~text-align:center;border:solid 1px #999;margin:5px;padding:5px;background-color:#eaeaea;";
        displayAnchor2.displaySettings.labelWhenOpened = "";
        displayAnchor2.displaySettings.scrollToTopWhenOpened = true;
        displayAnchor2.displaySettings.hideLabelWhenOpened = true;
        displayAnchor2.displaySettings.displayLabel = "";
        document.htmlDefinition.displayAnchors.Add(displayAnchor2);

        DisplayAnchor displayAnchor5 = new DisplayAnchor();
        displayAnchor5.startAnchor = "$Docu$collapsedS$";
        displayAnchor5.endAnchor = "$Docu$collapsedE$";
        displayAnchor5.removeEndAnchor = true;
        displayAnchor5.removeStartAnchor = true;
        displayAnchor5.caseSensitive = true;
        displayAnchor5.displaySettings = new DisplaySettings();
        displayAnchor5.displaySettings.display = "collapsed";
        displayAnchor5.displaySettings.tableStyle = "";
        displayAnchor5.displaySettings.cellStyle = "";
        displayAnchor5.displaySettings.labelWhenOpened = "";
        displayAnchor5.displaySettings.scrollToTopWhenOpened = true;
        displayAnchor5.displaySettings.hideLabelWhenOpened = false;
        displayAnchor5.displaySettings.displayLabel = @"<div style='display:flex;flex-flow:row wrap;align-items:center;min-height:40px;border-top:solid 1px #dedede;font-size:24px;color:#9d2624'><div style = 'flex:none;width:90%;padding:5px;'>Declaration Of Independence</div><div style='width:10%;text-align:right;padding:5px;'><a><i class='icon icon-caret-large-down' style='color:#000;'></i></a></div></div>";
        document.htmlDefinition.displayAnchors.Add(displayAnchor5);

        DisplayAnchor displayAnchor3 = new DisplayAnchor();
        displayAnchor3.startAnchor = "$Docu$collapsibleS$";
        displayAnchor3.endAnchor = "$Docu$collapsibleE$";
        displayAnchor3.removeEndAnchor = true;
        displayAnchor3.removeStartAnchor = true;
        displayAnchor3.caseSensitive = true;
        displayAnchor3.displaySettings = new DisplaySettings();
        displayAnchor3.displaySettings.display = "collapsible";
        displayAnchor3.displaySettings.tableStyle = "";
        displayAnchor3.displaySettings.cellStyle = "";
        displayAnchor3.displaySettings.labelWhenOpened = "";
        displayAnchor3.displaySettings.scrollToTopWhenOpened = true;
        displayAnchor3.displaySettings.hideLabelWhenOpened = false;
        displayAnchor3.displaySettings.displayLabel = @"<div style='display:flex;flex-flow:row wrap;align-items:center;min-height:40px;border-top:solid 1px #dedede;font-size:24px;color:#9d2624'><div style = 'flex:none;width:90%;padding:5px;'>Declaration Of Independence</div><div style='width:10%;text-align:right;padding:5px;'><a><i class='icon icon-caret-large-down' style='color:#000;'></i></a></div></div>";
        document.htmlDefinition.displayAnchors.Add(displayAnchor3);

        DisplayAnchor displayAnchor4 = new DisplayAnchor();
        displayAnchor4.startAnchor = "$Docu$inlineS$";
        displayAnchor4.endAnchor = "$Docu$inlineE$";
        displayAnchor4.removeEndAnchor = true;
        displayAnchor4.removeStartAnchor = true;
        displayAnchor4.caseSensitive = true;
        displayAnchor4.displaySettings = new DisplaySettings();
        displayAnchor4.displaySettings.display = "inline";
        displayAnchor4.displaySettings.labelWhenOpened = "";
        displayAnchor4.displaySettings.scrollToTopWhenOpened = true;
        displayAnchor4.displaySettings.hideLabelWhenOpened = true;
        displayAnchor4.displaySettings.displayLabel = "Highlight the following section";
        displayAnchor4.displaySettings.inlineOuterStyle = "background-color:#ff0; padding:10px;";
        document.htmlDefinition.displayAnchors.Add(displayAnchor4);

        // Define an inline template
        InlineTemplate inline1 = new InlineTemplate();
        inline1.sequence = "2";
        inline1.recipients = new Recipients();
        inline1.recipients.signers = new List<Signer>();
        inline1.recipients.signers.Add(signer);


        // Add the inline template to a CompositeTemplate 
        CompositeTemplate compositeTemplate1 = new CompositeTemplate();
        compositeTemplate1.inlineTemplates = new List<InlineTemplate>();
        compositeTemplate1.inlineTemplates.Add(inline1);
        compositeTemplate1.document = document;

        // Add compositeTemplate to the envelope 
        createEnvelopeRequest.compositeTemplates = new List<CompositeTemplate>();
        createEnvelopeRequest.compositeTemplates.Add(compositeTemplate1);
        createEnvelopeRequest.brandId = ConfigurationManager.AppSettings["MomentumBrandID"];


        string output = JsonConvert.SerializeObject(createEnvelopeRequest);

        // Specify a unique boundary string that doesn't appear in the json or document bytes.
        string Boundary = "MY_BOUNDARY";

        // Set the URI
        HttpWebRequest request = HttpWebRequest.Create(ConfigurationManager.AppSettings["DocuSignServer"] + "/restapi/vdev/accounts/" + ConfigurationManager.AppSettings["API.AccountId"] + "/envelopes") as HttpWebRequest;

        // Set the method
        request.Method = "POST";

        // Set the authentication header
        request.Headers["X-DocuSign-Authentication"] = GetSecurityHeader();

        // Set the overall request content type aand boundary string
        request.ContentType = "multipart/form-data; boundary=" + Boundary;
        request.Accept = "application/json";

        // Start forming the body of the request
        Stream reqStream = request.GetRequestStream();

        // write boundary marker between parts
        WriteStream(reqStream, "\n--" + Boundary + "\n");

        // write out the json envelope definition part
        WriteStream(reqStream, "Content-Type: application/json\n");
        WriteStream(reqStream, "Content-Disposition: form-data\n");
        WriteStream(reqStream, "\n"); // requires an empty line between the header and the json body
        WriteStream(reqStream, output);

        // write out the form bytes for the first form
        WriteStream(reqStream, "\n--" + Boundary + "\n");
        WriteStream(reqStream, "Content-Type: application/pdf\n");
        WriteStream(reqStream, "Content-Disposition: file; filename=\"Sample_Form\"; documentId=1\n");
        WriteStream(reqStream, "\n");
        if (File.Exists(Server.MapPath("~/App_Data/kitchensink.pdf")))
        {
            // Read the file contents and write them to the request stream
            byte[] buf = new byte[4096];
            int len;
            // read contents of document into the request stream
            FileStream fileStream = File.OpenRead(Server.MapPath("~/App_Data/kitchensink.pdf"));
            while ((len = fileStream.Read(buf, 0, 4096)) > 0)
            {
                reqStream.Write(buf, 0, len);
            }
            fileStream.Close();
        }


        // wrte the end boundary marker - ensure that it is on its own line
        WriteStream(reqStream, "\n--" + Boundary + "--");
        WriteStream(reqStream, "\n");
        try
        {
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.Created)
            {
                byte[] responseBytes = new byte[response.ContentLength];
                using (var reader = new System.IO.BinaryReader(response.GetResponseStream()))
                {
                    reader.Read(responseBytes, 0, responseBytes.Length);
                }
                string responseText = Encoding.UTF8.GetString(responseBytes);
                CreateEnvelopeResponse createEnvelopeResponse = new CreateEnvelopeResponse();

                createEnvelopeResponse = JsonConvert.DeserializeObject<CreateEnvelopeResponse>(responseText);
                if (createEnvelopeResponse.status.Equals("sent"))
                {


                    // Now that we have created the envelope, get the recipient token for the first signer
                    String url = Request.Url.AbsoluteUri;
                    RecipientViewRequest recipientViewRequest = new RecipientViewRequest();
                    recipientViewRequest.authenticationMethod = "email";
                    recipientViewRequest.clientUserId = signer.clientUserId;
                    recipientViewRequest.email = email.Value;
                    recipientViewRequest.returnUrl = url.Substring(0, url.LastIndexOf("/")) + "/ConfirmationScreen.aspx";
                    recipientViewRequest.userName = firstname.Value + " " + lastname.Value;

                    HttpWebRequest request2 = HttpWebRequest.Create(ConfigurationManager.AppSettings["DocuSignServer"] + "/restapi/v2/accounts/" + ConfigurationManager.AppSettings["API.TemplatesAccountID"] + "/envelopes/" + createEnvelopeResponse.envelopeId + "/views/recipient") as HttpWebRequest;
                    request2.Method = "POST";

                    // Set the authenticationheader
                    request2.Headers["X-DocuSign-Authentication"] = GetSecurityHeader();

                    request2.Accept = "application/json";
                    request2.ContentType = "application/json";

                    Stream reqStream2 = request2.GetRequestStream();

                    WriteStream(reqStream2, JsonConvert.SerializeObject(recipientViewRequest));
                    HttpWebResponse response2 = request2.GetResponse() as HttpWebResponse;

                    responseBytes = new byte[response2.ContentLength];
                    using (var reader = new System.IO.BinaryReader(response2.GetResponseStream()))
                    {
                        reader.Read(responseBytes, 0, responseBytes.Length);
                    }
                    string response2Text = Encoding.UTF8.GetString(responseBytes);

                    RecipientViewResponse recipientViewResponse = new RecipientViewResponse();
                    recipientViewResponse = JsonConvert.DeserializeObject<RecipientViewResponse>(response2Text);
                    Response.Redirect(recipientViewResponse.url);

                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), UTF8Encoding.UTF8))
                {
                    string errorMess = reader.ReadToEnd();
                    log4net.ILog logger = log4net.LogManager.GetLogger(typeof(KitchenSink));
                    logger.Info("\n----------------------------------------\n");
                    logger.Error("DocuSign Error: " + errorMess);
                    logger.Error(ex.StackTrace);
                    Response.Write(ex.Message);
                }
            }
            else
            {
                log4net.ILog logger = log4net.LogManager.GetLogger(typeof(KitchenSink));
                logger.Info("\n----------------------------------------\n");
                logger.Error("WebRequest Error: " + ex.Message);
                logger.Error(ex.StackTrace);
                Response.Write(ex.Message);
            }
        }

    }
}