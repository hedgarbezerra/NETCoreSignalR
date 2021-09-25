﻿using Dawn;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Services.Pagination
{
    public interface IUriService
    {
        Uri GetPageUri(int pageIndex, int pageSize, string route);
        Uri GetUri(string route);
    }
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            Guard.Argument(baseUri, nameof(baseUri))
                .NotNull()
                .NotEmpty($"the {typeof(string)} for base URI can't be empty.")
                .NotWhiteSpace();

            _baseUri = baseUri;
        }
        public Uri GetPageUri(int pageIndex, int pageSize, string route)
        {
            var _endpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "pageIndex", pageIndex.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetUri(string route)
        {
            return new Uri(string.Concat(_baseUri, route));
        }
    }
}
