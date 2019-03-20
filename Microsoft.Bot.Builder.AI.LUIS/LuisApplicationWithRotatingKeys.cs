// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.Bot.Builder.AI.Luis
{
    /// <summary>
    /// Data describing a LUIS application.
    /// </summary>
    public class LuisApplicationWithRotatingKeys : LuisApplication
    {
        public LuisApplicationWithRotatingKeys()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuisApplicationWithRotatingKeys"/> class.
        /// </summary>
        /// <param name="applicationId">LUIS application ID.</param>
        /// <param name="endpointKey">LUIS subscription or endpoint key.</param>
        /// <param name="endpoint">LUIS endpoint to use like https://westus.api.cognitive.microsoft.com.</param>
        public LuisApplicationWithRotatingKeys(string applicationId, List<string> endpointKeysToRotate, string endpoint)
            : this((applicationId, endpointKeysToRotate, endpoint))
        {
        }

        private LuisApplicationWithRotatingKeys(ValueTuple<string, List<string>, string> props)
        {
            var (applicationId, endpointKeysToRotate, endpoint) = props;

            if (!Guid.TryParse(applicationId, out var appGuid))
            {
                throw new ArgumentException($"\"{applicationId}\" is not a valid LUIS application id.");
            }

            foreach (var endpointKey in endpointKeysToRotate)
            {
                if (!Guid.TryParse(endpointKey, out var subscriptionGuid))
                {
                    throw new ArgumentException($"\"{subscriptionGuid}\" is not a valid LUIS subscription key.");
                }
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException($"\"{endpoint}\" is not a valid LUIS endpoint.");
            }

            if (!Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
            {
                throw new ArgumentException($"\"{endpoint}\" is not a valid LUIS endpoint.");
            }

            ApplicationId = applicationId;
            EndpointKeysToRotate = endpointKeysToRotate;
            Endpoint = endpoint;
        }

        /// <summary>
        /// Gets or sets lUIS subscription or endpoint key.
        /// </summary>
        /// <value>
        /// LUIS subscription or endpoint key.
        /// </value>
        public List<string> EndpointKeysToRotate { get; set; }
    }
}