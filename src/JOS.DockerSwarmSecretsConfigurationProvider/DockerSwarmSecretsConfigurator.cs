using System;
using Microsoft.Extensions.Configuration;

namespace JOS.DockerSwarmSecretsConfigurationProvider
{
    public static class DockerSwarmSecretsConfigurator
    {
        public static IConfigurationBuilder AddDockerSwarmSecrets(this IConfigurationBuilder configurationBuilder)
        {
            return AddDockerSwarmSecrets(configurationBuilder, DockerSwarmSecretsConfigurationProvider.DefaultSecretsPath);
        }

        public static IConfigurationBuilder AddDockerSwarmSecrets(this IConfigurationBuilder configurationBuilder, string secretsPath, Action<string> handle = null)
        {
            configurationBuilder.Add(new DockerSwarmSecretsConfigurationSource(secretsPath, handle));
            return configurationBuilder;
        }
    }
}
