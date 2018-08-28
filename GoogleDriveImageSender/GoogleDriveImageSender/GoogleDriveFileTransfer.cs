using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

namespace GoogleDriveImageSender
{
    class GoogleDriveFileTransfer
    {
        string[] Scopes = { DriveService.Scope.Drive };
        string ApplicationName = "My Project";
        DriveService service;

        public GoogleDriveFileTransfer()
        {
            InitialiseAuthorization();
        }

        public void InitialiseAuthorization()
        {
            UserCredential credential;

            using (FileStream stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public string UploadFile(string path, string mimeType)
        {
            string result = "";
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = System.IO.Path.GetFileName(path),
                MimeType = mimeType
            };
            FilesResource.CreateMediaUpload request;

            try
            {
                using (FileStream uploadStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    request = service.Files.Create(fileMetadata, uploadStream, fileMetadata.MimeType);
                    request.Fields = "id";
                    request.Upload();
                }
                var file = request.ResponseBody;
                Console.WriteLine("File ID: " + file.Id);
                if (file != null)
                {
                    result = "File loaded";
                }
                else
                {
                    result = "upload error";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return result;
        }
    }
}
