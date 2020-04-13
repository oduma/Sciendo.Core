using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Web
{
    public class WebStringGet : IWebGet<string>
    {
        private readonly ILogger<WebStringGet> logger;

        public WebStringGet(ILogger<WebStringGet> logger)
        {
            this.logger = logger;
        }

        public string Get(Uri url)
        {
            logger.LogInformation("Getting content from url: {url}", url);
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            var httpClient = new HttpClient();
            try
            {
                using (var getTask = httpClient.GetStringAsync(url)
                    .ContinueWith(p => p).Result)
                {
                    if (getTask.Status == TaskStatus.RanToCompletion || !string.IsNullOrEmpty(getTask.Result))
                    {
                        logger.LogInformation("Content retrieved Ok from Url: {0}", url);
                        return getTask.Result;
                    }
                    logger.LogWarning("Content not retrieved from {url}. Task returned {getTask.Status}", url, getTask.Status);
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot retrieve content from {url}.", url);
                return string.Empty;
            }

        }
    }
}
