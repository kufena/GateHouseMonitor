using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.S3;
using System.Text.Json.Serialization;
using System.Text.Json;
using GateHouseModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GateHouseLambda
{
    public class Function
    {
        AmazonS3Client s3Client;

        public Function()
        {
            s3Client = new AmazonS3Client();
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

            string type = request.PathParameters["type"];
            Console.WriteLine($"type == {type}");
            Console.WriteLine($"path == {request.Path}");
            JsonSerializerOptions jsonopt = new JsonSerializerOptions();
            jsonopt.PropertyNameCaseInsensitive = true;

            if (String.Equals(type,"GateHouseMonitor"))
            {
                Console.WriteLine("Got a nice gate house monitor request.");
                var model = JsonSerializer.Deserialize<GateHouseMonitorModel>(request.Body, jsonopt );
                Console.WriteLine($"Got ok = {model.OK} and time/date of {model.Time.ToShortTimeString()}-{model.Time.ToShortDateString()}");

                string dtformat = model.Time.ToString("yyyy-M-d-HH-mm-ss");
                Console.WriteLine("Here's the format --" + dtformat + "--");

                var resp = await s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
                                {
                                    BucketName = "XXXX",
                                    ContentBody = request.Body,
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
            return new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }
    }
}
