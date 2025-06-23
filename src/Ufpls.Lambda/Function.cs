using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Ufpls.Checker;
using Ufpls.Domain;
using MongoDB.Driver;
using System.Text.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Ufpls.Lambda;

public class Function
{
    private readonly EligibilityEvaluator _evaluator;
    private readonly IMongoCollection<UfplsCase> _collection;

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        // Fetch MongoDB connection string from env var (Secrets Manager later)
        var mongoConn = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
        var client = new MongoClient(mongoConn);
        var db = client.GetDatabase("ufpls");
        _collection = db.GetCollection<UfplsCase>("cases");

        _evaluator = new EligibilityEvaluator(new IUfplsEligibilityRule[]
        {
            new FundValueRule(),
            new DeceasedRule(),
            new CrystallisationRule()
        });
    }


    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processed message {message.Body}");

        var ufplsCase = JsonSerializer.Deserialize<UfplsCase>(message.Body)!;
        var (isEligible, results) = _evaluator.Evaluate(ufplsCase);

        ufplsCase.Status = isEligible ? UfplsCaseStatus.Eligible : UfplsCaseStatus.Ineligible;
        ufplsCase.EligibilityChecks = results;

        // Check if document exists first
        var existingDoc = await _collection.Find(x => x.CaseId == ufplsCase.CaseId).FirstOrDefaultAsync();
        
        if (existingDoc != null)
        {
            // Update existing - keep the existing Id
            ufplsCase.Id = existingDoc.Id;
            await _collection.ReplaceOneAsync(x => x.CaseId == ufplsCase.CaseId, ufplsCase);
        }
        else
        {
            // New document - don't set Id, let MongoDB generate it
            ufplsCase.Id = null; // or just leave it as is
            await _collection.InsertOneAsync(ufplsCase);
        }
    }
}