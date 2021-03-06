using System;
using System.Collections.Generic;
namespace OneBlink.SDK.Model
{
    public class FormSubmissionEventConfigration
    {
        public string url
        {
            get; set;
        }
        public string recordTitle
        {
            get; set;
        }
        public FormSubmissionEventTrimUriOption container
        {
            get; set;
        }
        public string username
        {
            get; set;
        }
        public string password
        {
            get; set;
        }
        public FormSubmissionEventTrimUriOption recordType
        {
            get; set;
        }
        public FormSubmissionEventTrimUriOption actionDefinition
        {
            get; set;
        }
        public FormSubmissionEventTrimUriOption location
        {
            get; set;
        }
        public string secret
        {
            get; set;
        }
        public string email
        {
            get; set;
        }
        public string emailSubjectLine
        {
            get; set;
        }
        public string pdfFileName
        {
            get; set;
        }
        public string apiId
        {
            get; set;
        }
        public string apiEnvironment
        {
            get; set;
        }
        public string apiEnvironmentRoute
        {
            get; set;
        }
        public Guid elementId
        {
            get; set;
        }
        public string contentTypeName
        {
            get; set;
        }
        public Guid gatewayId
        {
            get; set;
        }
        public Guid environmentId
        {
            get; set;
        }
        public string crn2
        {
            get; set;
        }
        public string crn3
        {
            get; set;
        }
        public bool includeSubmissionIdInPdf
        {
            get; set;
        }

        public List<string> encryptedElementIds
        {
            get; set;
        }
        public List<string> excludedElementIds
        {
            get; set;
        }
    }
}