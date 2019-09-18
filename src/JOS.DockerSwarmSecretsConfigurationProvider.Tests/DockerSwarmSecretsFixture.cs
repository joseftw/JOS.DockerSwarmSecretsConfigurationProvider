using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JOS.DockerSwarmSecretsConfigurationProvider.Tests
{
    public class DockerSwarmSecretsFixture : IDisposable
    {
        public readonly string SecretsLocation = Path.Combine(".", "secrets");
        private readonly IList<string> _files;

        public DockerSwarmSecretsFixture()
        {
            _files = new List<string>();
        }

        public async Task CreateFile(string fileName, string value)
        {
            if (!Directory.Exists(SecretsLocation))
            {
                Directory.CreateDirectory(SecretsLocation);
            }

            var filePath = Path.Combine(SecretsLocation, fileName);
            using (var file = File.Create(filePath))
            {
                await file.WriteAsync(new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(value)));
                _files.Add(filePath);
            }
        }

        public void Dispose()
        {
            foreach (var file in _files)
            {
                File.Delete(file);
            }
        }
    }
}
