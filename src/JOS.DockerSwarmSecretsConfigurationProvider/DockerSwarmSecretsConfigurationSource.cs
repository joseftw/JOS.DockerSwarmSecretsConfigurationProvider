using System;
using Microsoft.Extensions.Configuration;

namespace JOS.DockerSwarmSecretsConfigurationProvider
{
    public class DockerSwarmSecretsConfigurationSource : IConfigurationSource
    {
        private readonly string _secretsPath;
        private readonly Action<string> _handle;

        public DockerSwarmSecretsConfigurationSource(string secretsPath) : this(secretsPath, null)
        {

        }

        public DockerSwarmSecretsConfigurationSource(string secretsPath, Action<string> handle)
        {
            _secretsPath = secretsPath ?? throw new ArgumentNullException(nameof(secretsPath));
            _handle = handle;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DockerSwarmSecretsConfigurationProvider(_secretsPath, _handle);
        }
    }
}
