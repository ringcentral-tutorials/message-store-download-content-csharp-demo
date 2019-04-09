using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RingCentral;

namespace message_store_download_content_csharp_demo
{
    public class Program
    {
        const string RINGCENTRAL_CLIENTID = "Your_App_Client_Id";
        const string RINGCENTRAL_CLIENTSECRET = "Your_App_Client_Secret";
        const bool RINGCENTRAL_PRODUCTION = false;

        const string RINGCENTRAL_USERNAME = "Your_Account_Username";
        const string RINGCENTRAL_PASSWORD = "Your_Account_Password";
        const string RINGCENTRAL_EXTENSION = "";

        static void Main(string[] args)
        {
            read_message_store_message_content().Wait();
        }
        static private async Task read_message_store_message_content()
        {
            RestClient rc = new RestClient(RINGCENTRAL_CLIENTID, RINGCENTRAL_CLIENTSECRET, RINGCENTRAL_PRODUCTION);
            await rc.Authorize(RINGCENTRAL_USERNAME, RINGCENTRAL_EXTENSION, RINGCENTRAL_PASSWORD);
            if (rc.token.access_token.Length > 0)
            {
                var parameters = new ListMessagesParameters();
                parameters.dateFrom = "2018-01-01T00:00:00.000Z";
                parameters.dateTo = "2018-12-31T23:59:59.999Z";

                var contentPath = "content/";
                System.IO.Directory.CreateDirectory(contentPath);
                // Limit API call to ~40 calls per minute to avoid exceeding API rate limit.
                long timePerApiCall = 1200;
                var resp = await rc.Restapi().Account().Extension().MessageStore().List(parameters);
                foreach (var record in resp.records)
                {
                    if (record.attachments != null)
                    {
                        foreach (var attachment in record.attachments)
                        {
                            var fileName = "";
                            var fileExt = getFileExtensionFromMimeType(attachment.contentType);
                            if (record.type == "VoiceMail")
                            {
                                if (attachment.type == "AudioRecording")
                                {
                                    fileName = String.Format("voicemail_recording_{0}{1}", record.attachments[0].id, fileExt);
                                }
                                else if (attachment.type == "AudioTranscription" &&
                                         record.vmTranscriptionStatus == "Completed")
                                {
                                    fileName = String.Format("voicemail_transcript_{0}.txt", record.attachments[0].id);
                                }
                            }
                            else if (record.type == "Fax")
                            {
                                fileName = String.Format("fax_attachment_{0}{1}", attachment.id, fileExt);
                            }
                            else if (record.type == "SMS")
                            {
                                if (attachment.type == "MmsAttachment")
                                {
                                    fileName = String.Format("mms_attachment_{0}{1}", record.attachments[0].id, fileExt);
                                }
                                else
                                {
                                    fileName = String.Format("sms_text_{0}.txt", record.attachments[0].id);
                                }
                            }
                            var start = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                            var res = await rc.Restapi().Account().Extension().MessageStore(record.id).Content(attachment.id).Get();
                            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(contentPath + fileName, FileMode.Create)))
                            {
                                writer.Write(res);
                                writer.Flush();
                                writer.Close();
                            }
                            var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                            var consumed = (end - start);
                            if (consumed < timePerApiCall)
                            {
                                Thread.Sleep((int)(timePerApiCall - consumed));
                            }

                        }
                    }
                }
            }
        }
        private static string getFileExtensionFromMimeType(string mimeType)
        {
            switch (mimeType)
            {
                case "text/html":
                    return "html";
                case "text/css":
                    return ".css";
                case "text/xml":
                    return ".xml";
                case "text/plain":
                    return ".txt";
                case "text/x-vcard":
                    return ".vcf";
                case "application/msword":
                    return ".doc";
                case "application/pdf":
                    return ".pdf";
                case "application/rtf":
                    return ".rtf";
                case "application/vnd.ms-excel":
                    return ".xls";
                case "application/vnd.ms-powerpoint":
                    return ".ppt";
                case "application/zip":
                    return ".zip";
                case "image/tiff":
                    return ".tiff";
                case "image/gif":
                    return ".gif";
                case "image/jpeg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                case "image/x-ms-bmp":
                    return ".bmp";
                case "image/svg+xml":
                    return ".svg";
                case "audio/wav":
                case "audio/x-wav":
                    return ".wav";
                case "audio/mpeg":
                    return ".mp3";
                case "audio/ogg":
                    return ".ogg";
                case "video/3gpp":
                    return ".3gp";
                case "video/mpeg":
                    return ".mpeg";
                case "video/quicktime":
                    return ".mov";
                case "video/x-flv":
                    return ".flv";
                case "video/x-ms-wmv":
                    return ".wmv";
                case "video/mp4":
                    return ".mp4";
                default:
                    return ".unknown";
            }
        }
    }
}
