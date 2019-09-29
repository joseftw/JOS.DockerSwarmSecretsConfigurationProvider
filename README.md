## Usage
```
var host = new HostBuilder()
    .ConfigureHostConfiguration(configHost =>
    {
        ...
        configHost.AddDockerSwarmSecrets();
        ...
    });
```

The provider will look for secrets in the following default location /run/secrets.
It will use the filename as the "configuration path" and the file contents as the value.
All files in the /run/secrets folder will be added to the configuration dictionary.

Example:
```/run/secrets/mysecret``` will add "mysecret" to the configuration dictionary.

## Configuration
It's possible to change the location of the secrets:

### Secrets location
```
var host = new HostBuilder()
    .ConfigureHostConfiguration(configHost =>
    {
        ...
        configHost.AddDockerSwarmSecrets("/my/custom/path");
        ...
    })

```

### Parsing of secrets
It's possible to pass in a custom ```Action<string>``` when adding the provider like this:
```
var host = new HostBuilder()
    .ConfigureHostConfiguration(configHost =>
    {
        ...
        configHost.AddDockerSwarmSecrets("/run/secrets", filePath =>
            {
                // Do something with the file
            });
        ...
    });
```
### More information
[https://josefottosson.se/dotnet-core-configurationprovider-for-docker-swarm-secrets/](https://josefottosson.se/dotnet-core-configurationprovider-for-docker-swarm-secrets/)
