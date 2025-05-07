using Atlas.Actions;
using Microsoft.AspNetCore.Builder;
using System;

namespace microservice_1.Actions
{
    /// <summary>
    /// Implements IConfigureWebApplication
    /// </summary>
    /// <remarks>
    /// For IConfigureWebApplication:
    /// * Configure() does setup and initialization of capabilities that your application depends on
    /// * Based on Priority values that you define.
    /// </remarks>
    internal class ApplicationConfigurationAction : IConfigureWebApplication
    {
        //private readonly SomeOptions _someOptions;

        /// <inheritdoc />
        public ApplicationConfigurationAction
        (
            //IOptions<SomeOptions> someOptions
        )
        {
            // configuration settings from ConfigurationKey attributes
            //_someOptions = someOptions.Value;
        }

        public int Priority { get; } = ConfigurationActionPriority.SomePriority;

        /// <inheritdoc />
        /// <summary>
        /// Performs setup and initialization of capabilities that your application depends on.
        /// </summary>
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
        }
    }
}
