using Azure.Storage.Blobs;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;

class Program
{

    static void Main(string[] args)
    {
        
        string strTag                                                    = args[0];
        string strConnectionString                                       = args[1];
        string strContainer                                              = args[2];
        string strProject                                                = args[3];
        string strKey                                                    = args[4];
        string strEndPt                                                  = args[5];

        Console.WriteLine("Open computer vision");
        CustomVisionTrainingClient trainingApi                          = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(strKey))
        {
            Endpoint                                                    = strEndPt
        };

        Console.WriteLine("Find or create project: " + strProject);
        Project p                                                       = null;
        foreach(Project vp in trainingApi.GetProjects())
        {
            if(vp.Name == strProject)
            {
                p                                                       = vp;
                break;
            }
        }

        if (p == null)
            p                                                           = trainingApi.CreateProject(strProject);

        Console.WriteLine("Find or create tag: " + strTag);
        var vTags                                                       = trainingApi.GetTags(p.Id);

        Tag t                                                           = null;

        foreach (Tag vt in vTags)
        {
            if (vt.Name == strTag)
            {
                t                                                       = vt;
                break;
            }
        }

        if (t == null)
            t                                                           = trainingApi.CreateTag(p.Id, strTag);

        Console.WriteLine("Train model with storage account container (" + strContainer + ")");
        BlobServiceClient blobServiceClient                             = new BlobServiceClient(strConnectionString);
        BlobContainerClient containerClient                             = blobServiceClient.GetBlobContainerClient(strContainer);
        var blobs                                                       = containerClient.GetBlobs();
        List<Guid> lstGuid                                              = new List<Guid>();
        lstGuid.Add(t.Id);

        try
        {
            foreach (var item in blobs)
            {
                Console.WriteLine("Train (" + strTag + ") with: " + item.Name);
                var blobClient                                          = containerClient.GetBlobClient(item.Name);
                trainingApi.CreateImagesFromData(p.Id, blobClient.OpenRead(), lstGuid);
            }
        }
        catch (Exception e)
        {
            //  Block of code to handle errors
            Console.WriteLine("Error", e);

        }

        Console.WriteLine("Done!");
    }
}