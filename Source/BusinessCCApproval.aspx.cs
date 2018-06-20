using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Net.Http;


public partial class BusinessCCApproval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            button.Visible = true;
            button.InnerText = "Submit";
        }


        // Add event handlers for the navigation button on each of the wizard pages 
        PrefillClick.ServerClick += new EventHandler(prefill_Click);
        button.ServerClick += new EventHandler(page1_Click);

    }

    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Define what happens when the Submit button is clicked 
    protected void button1_Click(object sender, EventArgs e)
    {
        // Call method to create envelope
        createEnvelopeResponsive();
    }

    protected void page1_Click(object sender, EventArgs e)
    {
        button.Visible = false;
        createEnvelopeResponsive();
    }

    protected void page2_Click(object sender, EventArgs e)
    {
        primarySignerSection.Visible = false;
        button.Visible = false;
    }

    // Set up your prefill values when you click anchor tag 
    protected void prefill_Click(object sender, EventArgs e)
    {
        firstName.Value = "Priya";
        lastName.Value = "Sampath";
        title.Value = "Owner";
        businessName.Value = "ACME Widgets";
    }


    public class FullnameTab
    {
        public string name { get; set; }
        public string value { get; set; }
        public string tabLabel { get; set; }
        public string font { get; set; }
        public string bold { get; set; }
        public string italic { get; set; }
        public string underline { get; set; }
        public string fontColor { get; set; }
        public string fontSize { get; set; }
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
        public string anchorString { get; set; }
        public string anchorXOffset { get; set; }
        public string anchorYOffset { get; set; }
        public string anchorUnits { get; set; }
        public string anchorIgnoreIfNotPresent { get; set; }
        public string anchorCaseSensitive { get; set; }
        public string anchorMatchWholeWord { get; set; }
        public string anchorHorizontalAlignment { get; set; }
        public string tabId { get; set; }
        public string templateLocked { get; set; }
        public string templateRequired { get; set; }
        public string conditionalParentLabel { get; set; }
        public string conditionalParentValue { get; set; }
        public string customTabId { get; set; }
        public string tabOrder { get; set; }
    }

    public class ApproveTab
    {
        public string buttonText { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string tabLabel { get; set; }
        public string font { get; set; }
        public string bold { get; set; }
        public string italic { get; set; }
        public string underline { get; set; }
        public string fontColor { get; set; }
        public string fontSize { get; set; }
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
        public string anchorString { get; set; }
        public string anchorXOffset { get; set; }
        public string anchorYOffset { get; set; }
        public string anchorUnits { get; set; }
        public string anchorIgnoreIfNotPresent { get; set; }
        public string anchorCaseSensitive { get; set; }
        public string anchorMatchWholeWord { get; set; }
        public string anchorHorizontalAlignment { get; set; }
        public string tabId { get; set; }
        public string templateLocked { get; set; }
        public string templateRequired { get; set; }
        public string conditionalParentLabel { get; set; }
        public string conditionalParentValue { get; set; }
        public string customTabId { get; set; }
        public string tabOrder { get; set; }
    }
    public class TextTab
    {
        public string isPaymentAmount { get; set; }
        public string validationPattern { get; set; }
        public string validationMessage { get; set; }
        public string shared { get; set; }
        public string requireInitialOnSharedChange { get; set; }
        public string requireAll { get; set; }
        public string value { get; set; }
        public string required { get; set; }
        public string locked { get; set; }
        public string concealValueOnDocument { get; set; }
        public string disableAutoSize { get; set; }
        public string maxLength { get; set; }
        public string tabLabel { get; set; }
        public string font { get; set; }
        public string bold { get; set; }
        public string italic { get; set; }
        public string underline { get; set; }
        public string fontColor { get; set; }
        public string fontSize { get; set; }
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }

    public class Tabs
    {
        public List<SignHereTab> signHereTabs { get; set; }
        public List<DateSignedTab> dateSignedTabs { get; set; }
        public List<TextTab> textTabs { get; set; }
        public List<CheckboxTab> checkboxTabs { get; set; }
        public List<FullnameTab> fullnameTabs { get; set; }
        public List<ListTab> listTabs { get; set; }
        public List<RadioGroupTab> radioGroupTabs { get; set; }
        public List<ApproveTab> approveTabs { get; set; }
    }

    public class PhoneAuthentication
    {
        public string recipMayProvideNumber { get; set; }
        public string recordVoicePrint { get; set; }
        public List<string> senderProvidedNumbers { get; set; }
        public string validateRecipProvidedNumber { get; set; }
    }

    public class SMSAuthentication
    {
        public List<string> senderProvidedNumbers { get; set; }
    }

    public class Signer
    {

        public string email { get; set; }
        public string name { get; set; }
        public int recipientId { get; set; }
        public string roleName { get; set; }
        public string routingOrder { get; set; }
        public string clientUserId { get; set; }
        public Tabs tabs { get; set; }
        public PhoneAuthentication phoneAuthentication { get; set; }
        public SMSAuthentication smsAuthentication { get; set; }

        public string accessCode { get; set; }
        public string addAccessCodeToEmail { get; set; }
        public List<string> customFields { get; set; }
        public string idCheckConfigurationName { get; set; }
        public string inheritEmailNotificationConfiguration { get; set; }
        public string note { get; set; }
        public string requireIdLookup { get; set; }
        public string defaultRecipient { get; set; }
        public string signInEachLocation { get; set; }

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
        public List<DisplayAnchor> displayAnchors { get; set; }
        public string source { get; set; }
    }
    public class Document
    {
        public string documentId { get; set; }
        public string name { get; set; }
        //      public string transformPdfFields { get; set; }
        public HtmlDefinition htmlDefinition { get; set; }
        //      public string documentBase64 { get; set; }

    }

    public class InlineTemplate
    {
        public string sequence { get; set; }
        public Recipients recipients { get; set; }

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

    public class RecipientEvent
    {
        public string recipientEventStatusCode { get; set; }
    }

    public class EnvelopeEvent
    {
        public string envelopeEventStatusCode { get; set; }
    }

    public class EventNotification
    {
        public List<RecipientEvent> RecipientEvents { get; set; }
        public List<EnvelopeEvent> EnvelopeEvents { get; set; }
        public string includeSenderAccountasCustomField { get; set; }
        public string includeEnvelopeVoidReason { get; set; }
        public string includeTimeZoneInformation { get; set; }
        public string includeDocuments { get; set; }
        public string url { get; set; }
    }

    protected EventNotification getConnectSetup()
    {
        EventNotification eventNotification = new EventNotification();

        // First define the envelope events 
        eventNotification.EnvelopeEvents = new List<EnvelopeEvent>();
        EnvelopeEvent event1 = new EnvelopeEvent();
        event1.envelopeEventStatusCode = "Sent";
        eventNotification.EnvelopeEvents.Add(event1);
        EnvelopeEvent event2 = new EnvelopeEvent();
        event2.envelopeEventStatusCode = "Completed";
        eventNotification.EnvelopeEvents.Add(event2);
        EnvelopeEvent event3 = new EnvelopeEvent();
        event3.envelopeEventStatusCode = "Delivered";
        eventNotification.EnvelopeEvents.Add(event3);
        EnvelopeEvent event4 = new EnvelopeEvent();
        event4.envelopeEventStatusCode = "Declined";
        eventNotification.EnvelopeEvents.Add(event4);
        EnvelopeEvent event5 = new EnvelopeEvent();
        event5.envelopeEventStatusCode = "Voided";
        eventNotification.EnvelopeEvents.Add(event5);

        // Next define the recipient events 
        eventNotification.RecipientEvents = new List<RecipientEvent>();
        RecipientEvent recipEvent1 = new RecipientEvent();
        recipEvent1.recipientEventStatusCode = "Sent";
        eventNotification.RecipientEvents.Add(recipEvent1);
        RecipientEvent recipEvent2 = new RecipientEvent();
        recipEvent2.recipientEventStatusCode = "Delivered";
        eventNotification.RecipientEvents.Add(recipEvent2);
        RecipientEvent recipEvent3 = new RecipientEvent();
        recipEvent3.recipientEventStatusCode = "Completed";
        eventNotification.RecipientEvents.Add(recipEvent3);
        RecipientEvent recipEvent4 = new RecipientEvent();
        recipEvent4.recipientEventStatusCode = "Declined";
        eventNotification.RecipientEvents.Add(recipEvent4);
        RecipientEvent recipEvent5 = new RecipientEvent();
        recipEvent5.recipientEventStatusCode = "AuthenticationFailed";
        eventNotification.RecipientEvents.Add(recipEvent5);
        RecipientEvent recipEvent6 = new RecipientEvent();
        recipEvent6.recipientEventStatusCode = "AutoResponded";
        eventNotification.RecipientEvents.Add(recipEvent6);

        eventNotification.includeDocuments = "true";
        eventNotification.includeEnvelopeVoidReason = "true";
        eventNotification.includeSenderAccountasCustomField = "true";
        eventNotification.includeTimeZoneInformation = "true";
        eventNotification.url = ConfigurationManager.AppSettings["ConnectListener"];

        return eventNotification;
    }

    public class CreateEnvelopeRequest
    {
        public string status { get; set; }
        public string emailSubject { get; set; }
        public string emailBlurb { get; set; }
        public List<CompositeTemplate> compositeTemplates { get; set; }
        public string brandId { get; set; }
        public EventNotification eventNotification { get; set; }
        public Recipients recipients { get; set; }
        public List<Document> documents { get; set; }
        public List<EnvelopeCustomField> envelopeCustomFields { get; set; }
    }





    public class CreateEnvelopeCustomField
    {
        public List<TextCustomField> textCustomFields { get; set; }
        public List<ListCustomField> listCustomFields { get; set; }
    }

    public class ErrorDetails
    {
        public string errorCode { get; set; }
        public string message { get; set; }
    }

    public class TextCustomField
    {
        public string fieldId { get; set; }
        public string name { get; set; }
        public string show { get; set; }
        public string required { get; set; }
        public string value { get; set; }
        public string configurationType { get; set; }
        public ErrorDetails errorDetails { get; set; }
    }

    public class ErrorDetails2
    {
        public string errorCode { get; set; }
        public string message { get; set; }
    }

    public class ListCustomField
    {
        public List<string> listItems { get; set; }
        public string fieldId { get; set; }
        public string name { get; set; }
        public string show { get; set; }
        public string required { get; set; }
        public string value { get; set; }
        public string configurationType { get; set; }
        public ErrorDetails2 errorDetails { get; set; }
    }


    public class CreateEnvelopeCustomFieldResponse
    {
        public List<TextCustomField> textCustomFields { get; set; }
        public List<ListCustomField> listCustomFields { get; set; }
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
        public string stampType { get; set; }
        public string name { get; set; }
        public string tabLabel { get; set; }
        public string scaleValue { get; set; }
        public string optional { get; set; }
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
    }


    public class DateSignedTab
    {
        public string name { get; set; }
        public string value { get; set; }
        public string tabLabel { get; set; }
        public string font { get; set; }
        public string bold { get; set; }
        public string italic { get; set; }
        public string underline { get; set; }
        public string fontColor { get; set; }
        public string fontSize { get; set; }
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
        public string anchorString { get; set; }
        public string anchorXOffset { get; set; }
        public string anchorYOffset { get; set; }
        public string anchorUnits { get; set; }
        public string anchorIgnoreIfNotPresent { get; set; }
        public string anchorCaseSensitive { get; set; }
        public string anchorMatchWholeWord { get; set; }
        public string anchorHorizontalAlignment { get; set; }
        public string tabId { get; set; }
        public string templateLocked { get; set; }
        public string templateRequired { get; set; }
        public string conditionalParentLabel { get; set; }
        public string conditionalParentValue { get; set; }
        public string customTabId { get; set; }
        public string tabOrder { get; set; }
    }

    public class CheckboxTab
    {
        public string name { get; set; }
        public string tabLabel { get; set; }
        public string selected { get; set; }
        public string shared { get; set; }
        public string requireInitialOnSharedChange { get; set; }
        public string required { get; set; }
        public string locked { get; set; }
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
    }

    public class ListItem
    {
        public string text { get; set; }
        public string value { get; set; }
        public string selected { get; set; }
    }


    public class ListTab
    {
        public List<ListItem> listItems { get; set; }
        public string value { get; set; }
        public int width { get; set; }
        public string shared { get; set; }
        public string requireInitialOnSharedChange { get; set; }
        public string required { get; set; }
        public string locked { get; set; }
        public string senderRequired { get; set; }
        public string requireAll { get; set; }
        public string tabLabel { get; set; }
        public string font { get; set; }
        public string bold { get; set; }
        public string italic { get; set; }
        public string underline { get; set; }
        public string fontColor { get; set; }
        public string fontSize { get; set; }
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
        public string anchorString { get; set; }
        public string anchorXOffset { get; set; }
        public string anchorYOffset { get; set; }
        public string anchorUnits { get; set; }
        public string anchorIgnoreIfNotPresent { get; set; }
        public string anchorCaseSensitive { get; set; }
        public string anchorMatchWholeWord { get; set; }
        public string anchorHorizontalAlignment { get; set; }
        public string tabId { get; set; }
        public string templateLocked { get; set; }
        public string templateRequired { get; set; }
        public string conditionalParentLabel { get; set; }
        public string conditionalParentValue { get; set; }
        public string customTabId { get; set; }
        public string tabOrder { get; set; }
    }

    public class Radio
    {
        public string pageNumber { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
        public string anchorString { get; set; }
        public string anchorXOffset { get; set; }
        public string anchorYOffset { get; set; }
        public string anchorUnits { get; set; }
        public string anchorIgnoreIfNotPresent { get; set; }
        public string anchorCaseSensitive { get; set; }
        public string anchorMatchWholeWord { get; set; }
        public string anchorHorizontalAlignment { get; set; }
        public string value { get; set; }
        public string selected { get; set; }
        public string tabId { get; set; }
        public string required { get; set; }
        public string locked { get; set; }
        public string tabOrder { get; set; }
    }

    public class RadioGroupTab
    {
        public string documentId { get; set; }
        public string recipientId { get; set; }
        public string templateLocked { get; set; }
        public string templateRequired { get; set; }
        public string conditionalParentLabel { get; set; }
        public string conditionalParentValue { get; set; }
        public string groupName { get; set; }
        public List<Radio> radios { get; set; }
        public string shared { get; set; }
        public string requireInitialOnSharedChange { get; set; }
        public string requireAll { get; set; }
    }
    public class RecipientViewRequest
    {
        public string authenticationMethod { get; set; }
        public string email { get; set; }
        public string returnUrl { get; set; }
        public string userName { get; set; }
        public string clientUserId { get; set; }
    }



    public class EnvelopeCustomField
    {
        public List<TextCustomField> textCustomFields { get; set; }
        public List<ListCustomField> listCustomFields { get; set; }
    }


    public class RecipientViewResponse
    {
        public string url { get; set; }
    }

    protected String RandomizeClientUserID()
    {
        Random random = new Random();

        return (random.Next()).ToString();
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

    protected void createEnvelopeResponsive()
    {

        // Set up the envelope
        CreateEnvelopeRequest createEnvelopeRequest = new CreateEnvelopeRequest();
        createEnvelopeRequest.emailSubject = "Business Credit Card Approval";
        createEnvelopeRequest.status = "sent";
        createEnvelopeRequest.emailBlurb = "Please review & DocuSign your business credit cards approval";

        // Define first signer 
        Signer signer = new Signer();
        signer.email = Email.Value;
        signer.name = firstName.Value + ' ' + lastName.Value;
        signer.recipientId = 1;
        signer.routingOrder = "1";
        signer.roleName = "signer";
        signer.clientUserId = RandomizeClientUserID();


        //// Add tabs for the signer
        signer.tabs = new Tabs();
        signer.tabs.dateSignedTabs = new List<DateSignedTab>();
        DateSignedTab dateSignedTab = new DateSignedTab();

        dateSignedTab.documentId = "1";
        dateSignedTab.pageNumber = "1";
        dateSignedTab.font = "arial";
        dateSignedTab.fontSize = "size12";
        dateSignedTab.fontColor = "black";
        dateSignedTab.recipientId = "1";
        dateSignedTab.tabLabel = "clientDateSigned";
        signer.tabs.dateSignedTabs.Add(dateSignedTab);


        signer.tabs.signHereTabs = new List<SignHereTab>();
        SignHereTab signHereTab = new SignHereTab();
        signHereTab.stampType = "signature";
        signHereTab.name = "SignHere";
        signHereTab.tabLabel = "clientSignature";
        signHereTab.scaleValue = "1";
        signHereTab.optional = "false";
        signHereTab.recipientId = "1";
        signer.tabs.signHereTabs.Add(signHereTab);

        signer.tabs.textTabs = new List<TextTab>();
        TextTab texttab2 = new TextTab();
        texttab2.isPaymentAmount = "false";
        texttab2.validationMessage = "";
        texttab2.validationPattern = "";
        texttab2.shared = "false";
        texttab2.requireInitialOnSharedChange = "false";
        texttab2.requireAll = "false";
        texttab2.value = firstName.Value + " " + lastName.Value + "/" + title.Value;
        texttab2.required = "true";
        texttab2.locked = "true";
        texttab2.concealValueOnDocument = "false";
        texttab2.disableAutoSize = "false";
        texttab2.maxLength = "4000";
        texttab2.tabLabel = "Approver";
        texttab2.font = "arial";
        texttab2.bold = "false";
        texttab2.italic = "false";
        texttab2.underline = "false";
        texttab2.fontColor = "black";
        texttab2.fontSize = "size12";
        texttab2.documentId = "1";
        texttab2.recipientId = "1";
        texttab2.width = "100";
        texttab2.height = "11";
        signer.tabs.textTabs.Add(texttab2);

        TextTab texttab3 = new TextTab();
        texttab3.isPaymentAmount = "false";
        texttab3.validationMessage = "";
        texttab3.validationPattern = "";
        texttab3.shared = "false";
        texttab3.requireInitialOnSharedChange = "false";
        texttab3.requireAll = "false";
        texttab3.value = businessName.Value;
        texttab3.required = "true";
        texttab3.locked = "true";
        texttab3.concealValueOnDocument = "false";
        texttab3.disableAutoSize = "false";
        texttab3.maxLength = "4000";
        texttab3.tabLabel = "BusinessName";
        texttab3.font = "arial";
        texttab3.bold = "false";
        texttab3.italic = "false";
        texttab3.underline = "false";
        texttab3.fontColor = "black";
        texttab3.fontSize = "size12";
        texttab3.documentId = "1";
        texttab3.recipientId = "1";
        texttab3.width = "60";
        texttab3.height = "11";
        signer.tabs.textTabs.Add(texttab3);




        createEnvelopeRequest.recipients = new Recipients();

        createEnvelopeRequest.recipients.signers = new List<Signer>();
        createEnvelopeRequest.recipients.signers.Add(signer);

        // Define a document 
        Document document = new Document();
        document.documentId = "1";
        document.name = "Business Credit Card Approval";
        document.htmlDefinition = new HtmlDefinition();
        // Read in the HTML file 
        document.htmlDefinition.source = File.ReadAllText(Server.MapPath("~/App_Data/") + "digbankingdemo.html");

        // Define display anchors    
        document.htmlDefinition.displayAnchors = new List<DisplayAnchor>();
        DisplayAnchor displayAnchor = new DisplayAnchor();
        displayAnchor.startAnchor = "responsive_table_start";
        displayAnchor.endAnchor = "responsive_table_end";
        displayAnchor.removeEndAnchor = true;
        displayAnchor.removeStartAnchor = true;
        displayAnchor.caseSensitive = true;
        displayAnchor.displaySettings = new DisplaySettings();
        displayAnchor.displaySettings.display = "responsive_table_single_column";
        displayAnchor.displaySettings.tableStyle = "margin-bottom: 20px;width:100%;max-width:816px;margin-left:auto;margin-right:auto;";
        displayAnchor.displaySettings.cellStyle = "text-align:left;border:solid 0px #000;margin:0px;padding:0px;";
        document.htmlDefinition.displayAnchors.Add(displayAnchor);

        createEnvelopeRequest.documents = new List<Document>();
        createEnvelopeRequest.documents.Add(document);
        createEnvelopeRequest.brandId = ConfigurationManager.AppSettings["MomentumBrandID"];



        // Set up Connect
        createEnvelopeRequest.eventNotification = getConnectSetup();


        string output = JsonConvert.SerializeObject(createEnvelopeRequest);

        // Set the URI
        HttpWebRequest request = HttpWebRequest.Create(ConfigurationManager.AppSettings["DocuSignServer"] + "/restapi/vdev/accounts/" + ConfigurationManager.AppSettings["API.AccountId"] + "/envelopes") as HttpWebRequest;

        // Set the method
        request.Method = "POST";

        // Set the authentication header
        request.Headers["X-DocuSign-Authentication"] = GetSecurityHeader();

        // Set the overall request content type aand boundary string
        request.ContentType = "application/json";
        request.Accept = "application/json";

        // Start forming the body of the request
        Stream reqStream = request.GetRequestStream();


        WriteStream(reqStream, "\n"); // requires an empty line between the header and the json body
        WriteStream(reqStream, output);




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
                    recipientViewRequest.email = Email.Value;
                    recipientViewRequest.returnUrl = url.Substring(0, url.LastIndexOf("/")) + "/ConfirmationScreen.aspx";
                    recipientViewRequest.userName = firstName.Value + " " + lastName.Value;

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
                    Session.Add("envelopeID", createEnvelopeResponse.envelopeId);

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
                    log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BusinessCCApproval));
                    logger.Info("\n----------------------------------------\n");
                    logger.Error("DocuSign Error: " + errorMess);
                    logger.Error(ex.StackTrace);
                    Response.Write(ex.Message);
                }
            }
            else
            {
                log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BusinessCCApproval));
                logger.Info("\n----------------------------------------\n");
                logger.Error("WebRequest Error: " + ex.Message);
                logger.Error(ex.StackTrace);
                Response.Write(ex.Message);
            }
        }

    }

}