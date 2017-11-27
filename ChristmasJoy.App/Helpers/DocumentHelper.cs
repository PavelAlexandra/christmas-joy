using System;
using ChristmasJoy.App.Models;
using Microsoft.Azure.Documents.Client;
namespace ChristmasJoy.App.Helpers
{
  public interface IDocumentHelper {
    DocumentClient GetDocumentClient(IAppConfiguration configuration);
  }
  public class DocumentHelper: IDocumentHelper
  {
    public DocumentClient GetDocumentClient(IAppConfiguration configuration)
    {
      string endpointUrl = configuration.DocumentDBEndpointUrl;
      string primaryKey = configuration.DocumentDBKey;
      DocumentClient client = new DocumentClient(new Uri(endpointUrl), primaryKey);
      return client;
    }
  }
}
