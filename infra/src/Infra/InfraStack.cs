using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SQS;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Constructs;
using System.Collections.Generic;

namespace Infra;

public class InfraStack : Stack
{
    internal InfraStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // SQS queue
        var queue = new Queue(this, "UfplsQueue", new QueueProps
        {
            QueueName = "ufpls-queue"
        });

        // Lambda function (from local zip package)
        var lambdaFn = new Function(this, "UfplsLambda", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            Handler = "Ufpls.Lambda::Ufpls.Lambda.Function::FunctionHandler",
            Code = Code.FromAsset("../../../src/Ufpls.Lambda/output"), // assumes packaged zip folder
            Environment = new Dictionary<string, string>
            {
                { "MONGO_CONNECTION_STRING", "<insert-real-connection-string-or-use-Secrets>" }
            },
            MemorySize = 256,
            Timeout = Duration.Seconds(30)
        });

        // Allow Lambda to be triggered by SQS
        queue.GrantConsumeMessages(lambdaFn);

        // Set up event source mapping between SQS and Lambda
        new Amazon.CDK.AWS.Lambda.EventSources.SqsEventSource(queue);

        // Optional: IAM policy for Mongo access or Secrets Manager if needed
    }
}
