using MangaDexSharp;
using System.Reflection;

var username = "";
var password = "";

Console.WriteLine("Loading...");

// login
var mangaDex = MangaDex.Create();
var loginResult = await mangaDex.User.Login(username, password);
var token = loginResult.Data.Session;

// clear pending uploads
var pendingUpload = await mangaDex.Upload.Get(token);
if (pendingUpload.ErrorOccurred is false)
    await mangaDex.Upload.Abandon(pendingUpload.Data.Id, token);

// start new upload 
const string mangaId = "c875bebb-8d03-4c8e-89dd-a3235f28df62";
const string groupId = "a52ae16d-92cf-4576-8503-60ae77e2e08a";
var uploadBeginResponse = await mangaDex.Upload.Begin(mangaId!, new[] { groupId }, token);

// file creation
var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "example.png");
using var stream = File.OpenRead(filePath);
var fileUpload = new StreamFileUpload(Path.GetFileName(filePath), stream);

// upload
var fileUploadResult = await mangaDex.Upload.Upload(uploadBeginResponse.Data.Id, token, fileUpload);
if (fileUploadResult.ErrorOccurred)
    Console.WriteLine("Error");
else
    Console.WriteLine("Success");