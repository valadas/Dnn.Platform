﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace DotNetNuke.Tests.Core.Services.ClientCapability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Web;

    using DotNetNuke.Services.ClientCapability;
    using NUnit.Framework;

    /// <summary>  Summary description for FacebookRequestController.</summary>
    [TestFixture]
    public class FacebookRequestControllerTests
    {
        private IDictionary<string, string> _requestDics;

        [SetUp]
        public void SetUp()
        {
            this._requestDics = new Dictionary<string, string>();
            this._requestDics.Add("Empty", string.Empty);
            this._requestDics.Add("Valid", "vlXgu64BQGFSQrY0ZcJBZASMvYvTHu9GQ0YM9rjPSso.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsIjAiOiJwYXlsb2FkIiwidXNlcl9pZCI6ICIxIiwiZXhwaXJlcyI6IjEzMjUzNzU5OTkifQ==");

            this._requestDics.Add("ValidForAPage", "ylleuHAFR0DTpZ3bNr0fjMp7X7le_j8_HN3ONpbbgkk.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTMxOTQ4ODEwNywicGFnZSI6eyJpZCI6IjEzMDYzNDU0MDM3MjcyOCIsImxpa2VkIjpmYWxzZSwiYWRtaW4iOnRydWV9LCJ1c2VyIjp7ImNvdW50cnkiOiJjYSIsImxvY2FsZSI6ImVuX1VTIiwiYWdlIjp7Im1pbiI6MjF9fX0");

            // json data "{\"algorithm\":\"HMAC-SHA256\",\"issued_at\":1319488107,\"page\":{\"id\":\"130634540372728\",\"liked\":false,\"admin\":true},\"user\":{\"country\":\"ca\",\"locale\":\"en_US\",\"age\":{\"min\":21}}}"
            this._requestDics.Add("Invalid", "Invalid Content");
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Empty_Request_String()
        {
            var request = FacebookRequestController.GetFacebookDetailsFromRequest(this._requestDics["Empty"]);
            Assert.That(request, Is.Null);
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Invalid_Request_String()
        {
            var request = FacebookRequestController.GetFacebookDetailsFromRequest(this._requestDics["Invalid"]);
            Assert.That(request, Is.Null);
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Valid_Request_String()
        {
            var request = FacebookRequestController.GetFacebookDetailsFromRequest(this._requestDics["Valid"]);
            Assert.That(request.IsValid, Is.EqualTo(true));
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Valid_Request_String_ForAPage()
        {
            var request = FacebookRequestController.GetFacebookDetailsFromRequest(this._requestDics["ValidForAPage"]);
            Assert.Multiple(() =>
            {
                Assert.That(request.IsValid, Is.EqualTo(true));
                Assert.That(request.Algorithm, Is.EqualTo("HMAC-SHA256"));
                Assert.That(request.IssuedAt, Is.EqualTo(ConvertToTimestamp(1319488107)));

                // user stuff
                Assert.That(request.UserCountry, Is.EqualTo("ca"));
                Assert.That(request.UserLocale, Is.EqualTo("en_US"));
                Assert.That(request.UserMinAge, Is.EqualTo(21));
                Assert.That(request.UserMaxAge, Is.EqualTo(0));

                // page
                Assert.That(request.PageId, Is.EqualTo("130634540372728"));
                Assert.That(request.PageLiked, Is.EqualTo(false));
                Assert.That(request.PageUserAdmin, Is.EqualTo(true));
            });
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Empty_Request()
        {
            var request = FacebookRequestController.GetFacebookDetailsFromRequest(null as HttpRequest);
            Assert.That(request, Is.Null);
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Get_Request()
        {
            HttpRequest httpRequest = new HttpRequest("unittest.aspx", "http://localhost/unittest.aspx", string.Empty);
            httpRequest.RequestType = "GET";

            var request = FacebookRequestController.GetFacebookDetailsFromRequest(httpRequest);
            Assert.That(request, Is.Null);
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Post_Invalid_Request()
        {
            HttpRequest httpRequest = new HttpRequest("unittest.aspx", "http://localhost/unittest.aspx", string.Empty);
            httpRequest.RequestType = "POST";
            this.SetReadonly(httpRequest.Form, false);
            httpRequest.Form.Add("signed_request", this._requestDics["Invalid"]);

            var request = FacebookRequestController.GetFacebookDetailsFromRequest(httpRequest);
            Assert.That(request, Is.Null);
        }

        [Test]
        public void FacebookRequestController_GetFacebookDetailsFromRequest_With_Post_Valid_Request()
        {
            HttpRequest httpRequest = new HttpRequest("unittest.aspx", "http://localhost/unittest.aspx", string.Empty);
            httpRequest.RequestType = "POST";
            this.SetReadonly(httpRequest.Form, false);
            httpRequest.Form.Add("signed_request", this._requestDics["Valid"]);

            var request = FacebookRequestController.GetFacebookDetailsFromRequest(httpRequest);
            Assert.That(request.IsValid, Is.EqualTo(true));
        }

        /// <summary>method for converting a System.DateTime value to a UNIX Timestamp.</summary>
        /// <param name="value">date to convert.</param>
        /// <returns></returns>
        private static DateTime ConvertToTimestamp(long value)
        {
            // create Timespan by subtracting the value provided from
            // the Unix Epoch
            DateTime epoc = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return epoc.AddSeconds((double)value);
        }

        private void SetReadonly(NameValueCollection collection, bool readOnly)
        {
            var readOnlyProperty = collection.GetType().GetProperty("IsReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
            if (readOnlyProperty != null)
            {
                readOnlyProperty.SetValue(collection, readOnly, null);
            }
        }
    }
}
