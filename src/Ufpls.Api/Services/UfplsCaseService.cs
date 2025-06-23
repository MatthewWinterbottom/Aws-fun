using MongoDB.Driver;
using Ufpls.Domain;

namespace Ufpls.Api.Services;

public class UfplsCaseService
{
    private readonly IMongoCollection<UfplsCase> _collection;

    public UfplsCaseService(IConfiguration config)
    {
        var mongoConn = config["mongoDbConnectionString"];
        var client = new MongoClient(mongoConn);
        var db = client.GetDatabase("ufpls");
        _collection = db.GetCollection<UfplsCase>("cases");
    }

    public async Task<UfplsCase?> GetCaseByIdAsync(string id)
    {
        return await _collection.Find(x => x.CaseId == id).FirstOrDefaultAsync();
    }
}
