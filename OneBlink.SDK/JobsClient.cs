using System.Threading.Tasks;
using OneBlink.SDK.Model;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System;

namespace OneBlink.SDK
{
    public class JobsClient
    {
        OneBlinkHttpClient oneBlinkHttpClient;

        public JobsClient(string accessKey, string secretKey)
        {
            this.oneBlinkHttpClient = new OneBlinkHttpClient(accessKey, secretKey);
        }

        public async Task DeleteJob(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                throw new ArgumentException("'jobId' must be provided with a valid value");
            }

            string url = $"/jobs/{jobId}";

            await this.oneBlinkHttpClient.DeleteRequest(url);
        }

        public async Task<Job> CreateJob(NewJob newJob)
        {
            _ValidateJob(newJob);

            return await _CreateJob(newJob);
        }

        public async Task<Job> CreateJob<T>(NewJob newJob, T preFillData)
        {
            _ValidateJob(newJob);

            string preFillMetaId = await _SetPreFillData<T>(preFillData, newJob.formId);

            newJob.preFillFormDataId = preFillMetaId;

            return await _CreateJob(newJob);
        }

        public async Task<JobsSearchResult> SearchByExternalId(string externalId)
        {
            JobsSearchParameters searchParams = new JobsSearchParameters
            {
                externalId = externalId
            };

            return await Search(searchParams);
        }

        public async Task<JobsSearchResult> SearchByFormId(int formId)
        {
            JobsSearchParameters searchParams = new JobsSearchParameters
            {
                formId = formId
            };

            return await Search(searchParams);
        }

        public async Task<JobsSearchResult> SearchByUsername(string username)
        {
            JobsSearchParameters searchParams = new JobsSearchParameters
            {
                username = username
            };

            return await Search(searchParams);
        }

        public async Task<JobsSearchResult> Search(JobsSearchParameters searchParams)
        {
            string queryString = string.Empty;

            if (searchParams.formId.HasValue)
            {
                queryString += "formId=" + searchParams.formId.Value.ToString();
            }

            if (searchParams.isSubmitted.HasValue)
            {
                if (queryString != string.Empty)
                {
                    queryString += "&";
                }
                queryString += "isSubmitted=" + searchParams.isSubmitted.Value.ToString();
            }

            if (searchParams.limit.HasValue)
            {
                if (queryString != string.Empty)
                {
                    queryString += "&";
                }
                queryString += "limit=" + searchParams.limit.Value.ToString();
            }

            if (searchParams.offset.HasValue)
            {
                if (queryString != string.Empty)
                {
                    queryString += "&";
                }
                queryString += "offset=" + searchParams.offset.Value.ToString();
            }

            if (!String.IsNullOrWhiteSpace(searchParams.username))
            {
                if (queryString != string.Empty)
                {
                    queryString += "&";
                }
                queryString += "username=" + searchParams.username.ToString();
            }

            if (!String.IsNullOrWhiteSpace(searchParams.externalId))
            {
                if (queryString != string.Empty)
                {
                    queryString += "&";
                }
                queryString += "externalId=" + searchParams.externalId.ToString();
            }

            string url = "/jobs?" + queryString;

            return await this.oneBlinkHttpClient.GetRequest<JobsSearchResult>(url);
        }



        private async Task<Job> _CreateJob(NewJob newJob)
        {
            string url = "/jobs";

            return await this.oneBlinkHttpClient.PostRequest<NewJob, Job>(url, newJob);
        }

        private void _ValidateJob(NewJob newJob)
        {
            if (string.IsNullOrWhiteSpace(newJob.username))
            {
                throw new ArgumentException("'username' must be provided with a valid email address");
            }

            if (string.IsNullOrEmpty(newJob.details.title))
            {
                throw new ArgumentException("The 'title' property of JobDetail must be provided with a value");
            }
        }

        private async Task<string> _SetPreFillData<T>(T preFillData, int formId)
        {
            string url = "/forms/" + formId.ToString() + "/pre-fill-credentials";

            PreFillMeta preFillMeta = await this.oneBlinkHttpClient.PostRequest<PreFillMeta>(url);

            RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName(preFillMeta.s3.region);

            SessionAWSCredentials sessionAWSCredentials = new SessionAWSCredentials(
                preFillMeta.credentials.AccessKeyId,
                preFillMeta.credentials.SecretAccessKey,
                preFillMeta.credentials.SessionToken
            );

            AmazonS3Client amazonS3Client = new AmazonS3Client(sessionAWSCredentials, regionEndpoint);

            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = preFillMeta.s3.bucket,
                Key = preFillMeta.s3.key,
                ContentBody = JsonConvert.SerializeObject(preFillData),
                ContentType = "application/json"
            };

            await amazonS3Client.PutObjectAsync(request);

            return preFillMeta.preFillFormDataId;
        }
    }
}