using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FroalaEditor
{
    /// <summary>
    /// Amazon S3 functionality.
    /// </summary>
    public static class S3
    {
        /// <summary>
        /// Get signature hash used by the editor to sign to Amazon S3 when uploading a file.
        /// </summary>
        /// <param name="config">Amazon S3 config.</param>
        /// <returns></returns>
        public static object GetHash(S3Config config)
        {
            // Check default region.
            config.Region = config.Region ?? "us-east-1";
            config.Region = config.Region == "s3" ? "us-east-1" : config.Region;

            // Expiration s3 image signature #11
            double expirationInMinutes = 5;
            if(double.TryParse(config.Expiration, out var parsedValue))
            {
                expirationInMinutes = parsedValue;
            }

            // Important variables that will be used throughout this example.
            var bucket = config.Bucket;
            var region = config.Region;
            var keyStart = config.KeyStart;
            var acl = config.Acl;

            // These can be found on your Account page, under Security Credentials > Access Keys.
            var accessKey = config.AccessKey;
            var secretKey = config.SecretKey;

            var dateString = DateTime.Now.ToString("yyyyMMdd");

            var credential = string.Join("/", new string[] { accessKey, dateString, region, "s3/aws4_request" });
            var xAmzDate = dateString + "T000000Z";

            // Build policy.
            object policy = new
            {
                // Expiration s3 image signature #11
                expiration = DateTime.Now.AddMinutes(expirationInMinutes).ToString(@"yyyy-MM-dd\THH:mm:ss.000\Z"),
                conditions = new object[]
                {
                    new {bucket = bucket},
                    new {acl = acl},
                    new Dictionary <string, string> { { "success_action_status", "201" } },
                    new Dictionary <string, string> { { "x-requested-with", "xhr" } },
                    new Dictionary <string, string> { { "x-amz-algorithm", "AWS4-HMAC-SHA256" } },
                    new Dictionary <string, string> { { "x-amz-credential", credential } },
                    new Dictionary <string, string> { { "x-amz-date", xAmzDate } },
                    new string[] { "starts-with", "$key", keyStart },
                    new string[] { "starts-with", "$Content-Type", "" }
                }
            };

            var policyBase64 = Utils.Base64Encode(JsonConvert.SerializeObject(policy));
            
            // Get signature.
            var dateKey = Utils.HMAC256(Utils.ToBytes("AWS4" + secretKey), dateString);
            var dateRegionKey = Utils.HMAC256(dateKey, region);
            var dateRegionServiceKey = Utils.HMAC256(dateRegionKey, "s3");
            var signingKey = Utils.HMAC256(dateRegionServiceKey, "aws4_request");
            var signature = Utils.HexEncode(Utils.HMAC256(signingKey, policyBase64));

            // Return Amazon S3 signing hash.
            return new Dictionary < string, object>
            {
                { "bucket", bucket },
                { "region", region != "us-east-1" ? "s3-" + region : "s3" },
                { "keyStart", keyStart },
                { "params", new Dictionary < string, object>
                    {
                        { "acl", acl },
                        { "policy", policyBase64 },
                        { "x-amz-algorithm", "AWS4-HMAC-SHA256" },
                        { "x-amz-credential", credential },
                        { "x-amz-date", xAmzDate },
                        { "x-amz-signature", signature },
                    }
                }
            };
        }
    }
}
