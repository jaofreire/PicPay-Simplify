using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace PicPaySimplify.Services.AWSServices
{
    public class SESWrapper 
    {
        private readonly IConfiguration _configuration;

        public SESWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IAmazonSimpleEmailService CreateSES()
        {
            var awsCredentials = new BasicAWSCredentials(_configuration.GetValue<string>("AWS:AccessKey"), _configuration.GetValue<string>("AWS:AccessSecretKey"));

            var awsConfig =  new AmazonSimpleEmailServiceConfig()
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1")
            };

            return new AmazonSimpleEmailServiceClient(awsCredentials, awsConfig);
        }

        public async Task<SendEmailResponse> SendEmail(SendEmailRequest emailRequest)
        {
            var sesClient = CreateSES();

            return await sesClient.SendEmailAsync(emailRequest);
        }

        public SendEmailRequest EmailRequest(string emailFrom, List<string>emailTo, string subject, string body)
        {
            var requestEmail = new SendEmailRequest()
            {
                Source = emailFrom,
                Destination = new Destination()
                {
                    ToAddresses = emailTo
                },
                Message = new Message()
                {
                    Subject = new Content()
                    {
                        Charset = "UTF-8",
                        Data = subject
                    },
                    
                    Body = new Body()
                    {
                        Text = new Content()
                        {
                            Charset = "UTF-8",
                            Data = body
                        }
                    }
                }
            };

            return requestEmail;
        }
    }
}
