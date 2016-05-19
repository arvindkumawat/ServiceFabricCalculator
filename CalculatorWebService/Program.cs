// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CalculatorWebService
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using Microsoft.ServiceFabric.Services.Runtime;

    /// <summary>The program.</summary>
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("CalculatorWebServiceType", context => new CalculatorWebService(context)).GetAwaiter().GetResult();
                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(CalculatorWebService).Name);

                Thread.Sleep(Timeout.Infinite); // Prevents this host process from terminating so services keeps running.
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}