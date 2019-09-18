using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

namespace JOS.DockerSwarmSecretsConfigurationProvider.Tests
{
    public class ConfigurationProviderTests : IClassFixture<DockerSwarmSecretsFixture>
    {
        private readonly DockerSwarmSecretsFixture _fixture;
        private readonly DockerSwarmSecretsConfigurationProvider _sut;

        public ConfigurationProviderTests(DockerSwarmSecretsFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _sut = new DockerSwarmSecretsConfigurationProvider(_fixture.SecretsLocation);
        }

        [Fact]
        public async Task GivenOneSecretFile_WhenLoad_ThenAddsFilenameAsKeyAndFileContentAsValueToConfigurationDictionary()
        {
            await _fixture.CreateFile("My_Secret_1", "first secret value");

            _sut.Load();
            _sut.TryGet("my:secret:1", out var secretValue);

            secretValue.ShouldBe("first secret value");
        }

        [Fact]
        public async Task GivenMultipleSecretFiles_WhenLoad_ThenAddsFilenameAsKeyAndFileContentAsValueToConfigurationDictionary()
        {
            await _fixture.CreateFile("My_Secret_3", "third secret value");
            await _fixture.CreateFile("My_Secret_4", "fourth secret value");

            _sut.Load();
            _sut.TryGet("my:secret:3", out var secretValue);
            _sut.TryGet("my:secret:4", out var secretValue2);

            secretValue.ShouldBe("third secret value");
            secretValue2.ShouldBe("fourth secret value");
        }

        [Fact]
        public async Task GivenConfigurationBuilder_WhenAddDockerSwarmSecrets_ThenConfigurationContainsValuesFromSecrets()
        {
            await _fixture.CreateFile("My_Secret_5", "5 secret value");
            await _fixture.CreateFile("my_secret_6", "6 secret value");
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddDockerSwarmSecrets(_fixture.SecretsLocation);

            var configuration = configurationBuilder.Build();

            configuration["My:Secret:5"].ShouldBe("5 secret value");
            configuration["my:secret:6"].ShouldBe("6 secret value");
        }

        [Fact]
        public async Task GivenConfigurationBuilderWithExistingProviders_WhenAddDockerSwarmSecretsAfterThem_ThenConfigurationContainsValuesFromSecrets()
        {
            await _fixture.CreateFile("My_Secret_7", "7 secret value");
            await _fixture.CreateFile("my_secret_8", "8 secret value");
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("my:secret:7", "from memory"),
                    new KeyValuePair<string, string>("my:secret:8", "from memory")
                })
                .AddDockerSwarmSecrets(_fixture.SecretsLocation);

            var configuration = configurationBuilder.Build();

            configuration["My:Secret:7"].ShouldBe("7 secret value");
            configuration["my:secret:8"].ShouldBe("8 secret value");
        }

        [Fact]
        public void GivenNoSecrets_WhenLoad_ThenDoesNotCrash()
        {
            var sut = new DockerSwarmSecretsConfigurationProvider(Path.Combine(".","nosecrets"));

            sut.Load();
        }
    }
}
