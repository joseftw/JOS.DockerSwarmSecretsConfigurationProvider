using System;
using Microsoft.Extensions.Configuration;

namespace JOS.DockerSwarmSecretsConfigurationProvider
{
    public static class DockerSwarmSecretsConfigurator
    {
        public static void AddDockerSwarmSecrets(this IConfigurationBuilder configurationBuilder)
        {
            AddDockerSwarmSecrets(configurationBuilder, DockerSwarmSecretsConfigurationProvider.DefaultSecretsPath);
        }

        public static void AddDockerSwarmSecrets(this IConfigurationBuilder configurationBuilder, string secretsPath, Action<string> handle = null)
        {
            configurationBuilder.Add(new DockerSwarmSecretsConfigurationSource(secretsPath, handle));
        }
    }
}
