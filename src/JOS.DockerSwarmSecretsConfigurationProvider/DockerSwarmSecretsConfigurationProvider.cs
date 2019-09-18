using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace JOS.DockerSwarmSecretsConfigurationProvider
{
    public class DockerSwarmSecretsConfigurationProvider : ConfigurationProvider
    {
        public const string DefaultSecretsPath = "/run/secrets";
        private readonly string _secretsPath;
        private readonly Action<string> _handle;

        public DockerSwarmSecretsConfigurationProvider(string secretsPath) : this(secretsPath, null)
        {
            
        }

        public DockerSwarmSecretsConfigurationProvider(string secretsPath, Action<string> handle)
        {
            _handle = handle ?? (filePath =>
            {
                var fileName = Path.GetFileName(filePath);
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    var key = fileName.Replace("_", ":");
                    var value = File.ReadAllText(filePath);

                    Data.Add(key, value);
                }
            });

            _secretsPath = secretsPath ?? throw new ArgumentNullException(nameof(secretsPath));
        }

        public override void Load()
        {
            if (Directory.Exists(_secretsPath))
            {
                foreach (var file in Directory.EnumerateFiles(_secretsPath))
                {
                    _handle(file);
                }
            }
        }
    }
}
