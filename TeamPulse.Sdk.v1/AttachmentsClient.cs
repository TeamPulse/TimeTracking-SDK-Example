using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Telerik.TeamPulse.Sdk.Common;
using Telerik.TeamPulse.Sdk.Models;

namespace Telerik.TeamPulse.Sdk
{
    public class AttachmentsClient : ApiClientBase
    {
        public AttachmentsClient(string siteUrl, string refreshToken, string username, string password) : 
            base(siteUrl, refreshToken, username, password)
        {
        }

        public AttachmentsClient(string siteUrl, string accessToken) :
            base(siteUrl, accessToken)
        {
        }

        /// <summary>
        /// Create new attachment
        /// </summary>
        /// <param name="attachment">The attachment to be created</param>
        /// <param name="type">The type of the new attachment. Possible values are TeamPulse, FeedbackPortal. If not set TeamPulse is used</param>
        /// <returns>The created attachment</returns>
        public Attachment Create(int workItemId, AttachmentFileInfo file, string type = "TeamPulse")
        {
            string apiUrl = string.Format(ApiUrls.Attachments.Post, workItemId);
            string apiUrlQueryArgs = string.Format("{0}?type={1}", apiUrl, type);
            string postUrl = this.apiClient.CreateUrl(apiUrlQueryArgs);

            string responseContent = this.PostFile(postUrl, file);

            var attachment = SerializationHelper.DeserializeFromJson<Attachment>(responseContent);
            return attachment;
        }

        /// <summary>
        /// Get all attachments for specific workitem
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <returns>Array of attachment for the specified workitem</returns>
        public ApiCollection<Attachment> GetByWorkItem(int workItemId)
        {
            return this.Get<ApiCollection<Attachment>>(string.Format(ApiUrls.Attachments.GetForWorkItem, workItemId));
        }

        /// <summary>
        /// Get all attachments for specific workitem with filter and sort
        /// </summary>
        /// <param name="workItemId">The ID of the workitem</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested attachments</returns>
        public ApiCollection<Attachment> GetByWorkItem(int workItemId, string oDataOptions)
        {
            return this.Get<ApiCollection<Attachment>>(string.Format("{0}?{1}", 
                string.Format(ApiUrls.Attachments.GetForWorkItem, workItemId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get all attachments for specific comment
        /// </summary>
        /// <param name="commentId">The ID of the comment</param>
        /// <returns>The requested attachments</returns>
        public ApiCollection<Attachment> GetByComment(int commentId)
        {
            return this.Get<ApiCollection<Attachment>>(string.Format(ApiUrls.Attachments.GetForComment, commentId));
        }

        /// <summary>
        /// Get all attachments for specific workitem with filter and sort
        /// </summary>
        /// <param name="commentId">The ID of the comment</param>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>The requested attachments</returns>
        public ApiCollection<Attachment> GetByComment(int commentId, string oDataOptions)
        {
            return this.Get<ApiCollection<Attachment>>(string.Format("{0}?{1}",
                string.Format(ApiUrls.Attachments.GetForComment, commentId), oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Get a attachment by ID
        /// </summary>       
        /// <param name="attachmentId">The ID of the attachment</param>
        /// <returns></returns>
        public Attachment Get(int attachmentId)
        {
            return this.Get<Attachment>(string.Format(ApiUrls.Attachments.GetOne, attachmentId));
        }

        /// <summary>
        /// Get all attachments
        /// </summary>
        /// <param></param>
        /// <returns>Array of all attachments</returns>
        public ApiCollection<Attachment> GetAll()
        {
            return this.Get<ApiCollection<Attachment>>(ApiUrls.Attachments.GetMany);
        }

        /// <summary>
        /// Get all attachments with filter and sort
        /// </summary>
        /// <param name="oDataOptions">oData formatted query string</param>
        /// <returns>Array of all attachments</returns>
        public ApiCollection<Attachment> GetAll(string oDataOptions)
        {
            return this.Get<ApiCollection<Attachment>>(string.Format("{0}?{1}", ApiUrls.Attachments.GetMany, oDataOptions.TrimStart('?')));
        }

        /// <summary>
        /// Update an attachment
        /// </summary>
        /// <param name="attachment">The attachment with the updated fields</param>
        /// <returns>The updated attachment</returns>
        public Attachment Update(Attachment attachment)
        {
            return this.Put<Attachment>(string.Format(ApiUrls.Attachments.Put, attachment.id), SerializationHelper.SerializeToJson(attachment));
        }

        /// <summary>
        /// Partial update of a attachment
        /// </summary>
        /// <param name="attachmentId">The ID of the attachment</param>
        /// <param name="attachmentValues">The fields which has to be updated</param>
        /// <returns>The updated attachment</returns>
        public Attachment Update(int attachmentId, Dictionary<string, object> attachmentValues)
        {
            return this.Put<Attachment>(string.Format(ApiUrls.Attachments.Put, attachmentId), SerializationHelper.SerializeToJson(attachmentValues));
        }
        
        /// <summary>
        /// Delete a attachment
        /// </summary>        
        /// <param name="attachmentId">The ID of the attachment to be deleted</param>
        /// <returns></returns>
        public void Delete(int attachmentId)
        {
            this.Delete(string.Format(ApiUrls.Attachments.Delete, attachmentId));
        }

        #region Post file

        private string PostFile(string postUrl, AttachmentFileInfo file)
        {
            string formDataBoundary = String.Format("----------{0}", Guid.NewGuid());
            byte[] formData = GetMultipartFormData(formDataBoundary, file);

            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + formDataBoundary; ;            
            request.ContentLength = formData.Length;
            request.Headers.Add("Authorization", "WRAP access_token=" + this.apiClient.GetAccessToken());
            request.Accept = "application/json";

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }
            
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());
            string responseContent = sr.ReadToEnd();
            return responseContent;
        }

        private static byte[] GetMultipartFormData(string boundary, AttachmentFileInfo attFile)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            //formDataStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
            
            // Add just the first part of this param, since we will write the file data directly to the Stream
            StringBuilder header = new StringBuilder();

            header.AppendLine("--" + boundary);
            header.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "files", attFile.Name).AppendLine();
            header.AppendFormat("Content-Type: {0}", attFile.ContentType).AppendLine();
            header.AppendLine();

            string headerString = header.ToString();
            formDataStream.Write(Encoding.UTF8.GetBytes(headerString), 0, Encoding.UTF8.GetByteCount(headerString));

            // Write the file data directly to the Stream, rather than serializing it to a string.
            formDataStream.Write(attFile.Data, 0, attFile.Data.Length);

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }              

        #endregion
    }
}
