// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.NodeServices.Npm;
using Microsoft.AspNetCore.NodeServices.Util;
using Microsoft.AspNetCore.SpaServices.Util;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SpaServices.Extensions.Util;

namespace Microsoft.AspNetCore.SpaServices.VueDevelopmentServer
{
    internal static class VueDevelopmentServerMiddleware
    {
        private const string LogCategoryName = "Microsoft.AspNetCore.SpaServices";
        private static TimeSpan RegexMatchTimeout = TimeSpan.FromSeconds(5); // This is a development-time only feature, so a very long timeout is fine

        public static void Attach(
            ISpaBuilder spaBuilder,
            string scriptName)
        {
            var sourcePath = spaBuilder.Options.SourcePath;
            var devServerPort = 8080;
            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(sourcePath));
            }

            if (string.IsNullOrEmpty(scriptName))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(scriptName));
            }

            // Start create-react-app and attach to middleware pipeline
            var appBuilder = spaBuilder.ApplicationBuilder;
            var logger = LoggerFinder.GetOrCreateLogger(appBuilder, LogCategoryName);
            var portTask = StartCreateVueDevServerAsync(sourcePath, scriptName, devServerPort, logger);

            // Everything we proxy is hardcoded to target http://localhost because:
            // - the requests are always from the local machine (we're not accepting remote
            //   requests that go directly to the create-react-app server)
            // - given that, there's no reason to use https, and we couldn't even if we
            //   wanted to, because in general the create-react-app server has no certificate
            var targetUriTask = portTask.ContinueWith(
                task => new UriBuilder("http", "localhost", task.Result).Uri);

            SpaProxyingExtensions.UseProxyToSpaDevelopmentServer(spaBuilder, () =>
            {
                // On each request, we create a separate startup task with its own timeout. That way, even if
                // the first request times out, subsequent requests could still work.
                var timeout = spaBuilder.Options.StartupTimeout;
                return targetUriTask.WithTimeout(timeout,
                    $"The create-react-app server did not start listening for requests " +
                    $"within the timeout period of {timeout.Seconds} seconds. " +
                    $"Check the log output for error information.");
            });
        }

        private static async Task<int> StartCreateVueDevServerAsync(
            string sourcePath, string scriptName, int portNumber, ILogger logger)
        {
            if (portNumber == default(int))
            {
                portNumber = TcpPortFinder.FindAvailablePort();
            }
            logger.LogInformation($"Starting create-react-app server on port {portNumber}...");

            var scriptRunner = new NpmScriptRunner(sourcePath, scriptName, $"--port {portNumber} --host localhost", null);
            scriptRunner.AttachToLogger(logger);

            using (var stdErrReader = new EventedStreamStringReader(scriptRunner.StdErr))
            {
                try
                {
                    await scriptRunner.StdOut.WaitForMatch(
                        new Regex("DONE", RegexOptions.None, RegexMatchTimeout));
                }
                catch (EndOfStreamException ex)
                {
                    throw new InvalidOperationException(
                        $"The script '{scriptName}' exited without indicating that the " +
                        $"create-react-app server was listening for requests. The error output was: " +
                        $"{stdErrReader.ReadAsString()}", ex);
                }
            }

            return portNumber;
        }
    }
}
