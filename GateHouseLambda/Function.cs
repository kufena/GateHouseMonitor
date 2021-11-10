using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.S3;
using Amazon.SimpleSystemsManagement;
using System.Text.Json.Serialization;
using System.Text.Json;
using GateHouseModel;
using Amazon.SimpleSystemsManagement.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GateHouseLambda
{
    public class Function
    {
        AmazonS3Client s3Client;
        AmazonSimpleSystemsManagementClient ssmClient;
        string bucketName = null;

        public Function()
        {
            s3Client = new AmazonS3Client();
            ssmClient = new AmazonSimpleSystemsManagementClient();
        }

        public Function(AmazonS3Client cl)
        {
            s3Client = cl;
        }
        
        /// <summary>
        /// Responds to a gatehouse monitor or gatehouse things network request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Http response.</returns>
        public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest request, ILambdaContext context)
        {
            await getBucketName();

            string ip = "";
            if (request.Headers.TryGetValue("X-Forwarded-For", out ip))
            {
                Console.WriteLine($"Ip address of caller is {ip}");
            }

            string type = request.PathParameters["type"];
            Console.WriteLine($"type == {type}");
            Console.WriteLine($"path == {request.Path}");
            JsonSerializerOptions jsonopt = new JsonSerializerOptions();
            jsonopt.PropertyNameCaseInsensitive = true;
            jsonopt.Converters.Add(new JsonDateTimeConverter());
            jsonopt.Converters.Add(new JsonIPAddressConverter());

            if (String.Equals(type, "GateHouseMonitor"))
            {
                Console.WriteLine("Got a nice gate house monitor request.");
                var model = JsonSerializer.Deserialize<GateHouseMonitorModel>(request.Body, jsonopt);
                Console.WriteLine($"Got ok = {model.OK} and time/date of {model.Time.ToShortTimeString()}-{model.Time.ToShortDateString()}");

                string dtformat = model.Time.ToString("yyyy-MM-dd-HH-mm-ss");
                Console.WriteLine("Here's the format --" + dtformat + "--");
                
                MonitorJson mj = new MonitorJson
                {
                    CallerIP = ip,
                    Temperature = model.Temperature,
                    OK = model.OK,
                    Time = model.Time,
                    DomainIP = model.IPs,
                    Domain = model.Domain
                };

                var resp = await s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = bucketName,
                    ContentBody = JsonSerializer.Serialize<MonitorJson>(mj, jsonopt),
                    Key = $"GateHouseMonitor-{dtformat}"
                });

                Console.WriteLine("Our response was " + resp.HttpStatusCode);
                if (resp.HttpStatusCode != HttpStatusCode.OK)
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Body = resp.ToString()
                    };
            }

            if (String.Equals(type, "TTNMonitor"))
            {
                Console.WriteLine("Got a nice TTN monitor request.");
                Console.WriteLine(request.Body);
                
                var model = JsonSerializer.Deserialize<TTNModel>(request.Body, jsonopt);

                Console.WriteLine($"Temperature = {model.payload_fields.temperature}");
                Console.WriteLine($"Metadata = {model.metadata}");
                /*
                MonitorJson mj = new MonitorJson
                {
                    CallerIP = ip,
                    Temperature = model.Temperature,
                    OK = model.OK,
                    Time = model.Time,
                    DomainIP = model.IPs,
                    Domain = model.Domain
                };

                var resp = await s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = bucketName,
                    ContentBody = JsonSerializer.Serialize<MonitorJson>(mj, jsonopt),
                    Key = $"GateHouseMonitor-{dtformat}"
                });

                Console.WriteLine("Our response was " + resp.HttpStatusCode);
                if (resp.HttpStatusCode != HttpStatusCode.OK)
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Body = resp.ToString()
                    };
                */
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        private async Task getBucketName()
        {
            if (bucketName is null)
            {
                Console.WriteLine("Fetching bucket name from parameter store.");
                // This parameter store stuff is happening here because it is async and
                // cannot be done in the c'tor.  Lambda needs an initialise section?
                GetParameterRequest ssmRequest = new GetParameterRequest
                {
                    Name = "thegatehousewereham-s3monitorbucket"
                };

                var ssmResponse = await ssmClient.GetParameterAsync(ssmRequest);
                bucketName = ssmResponse.Parameter.Value;
            }
            else
            {
                Console.WriteLine("No need to get bucket name as it isn't null.");
            }
        }
    }
}
