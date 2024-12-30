using System.Diagnostics;

namespace QuantumKat.Utitlity;

public class TokenLoader
{
    private readonly string _tokenType;
    private static readonly string[] _tokenTypes = ["main", "dev"];

    /// <summary>
    /// Constructs a new TokenLoader object with the given token type to use
    /// </summary>
    /// <param name="tokenType">A string of value "main" or "dev" to indicate which bot version it should retrieve a token for.</param>
    /// <exception cref="ArgumentException">Thrown if the value is not "main" or "dev".</exception>
    public TokenLoader(string tokenType)
    {
        if (!_tokenTypes.Contains(tokenType))
        {
            throw new ArgumentException("tokenType may only be of the value \"main\" or \"dev\"");
        }

        _tokenType = tokenType;
    }

    /// <summary>
    /// Attempts to retrieve the token through 1Password with its CLI and a secret reference resolved from the tokenType field.
    /// </summary>
    /// <returns>The bot token</returns>
    /// <exception cref="Exception">If an error occurred trying to use the CLI</exception>
    public async Task<string> LoadWith1Password()
    {
        // TODO: Implement config file to allow easily customizing the secret reference
        // TODO: Improve process handling, and check if the command "op" even exists
        Process process = CreateProcess(fileName: "op", args: $"read \"op://Programming and IT security/QuantumKat Discord bot/{_tokenType} token\"");

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        await process.WaitForExitAsync();

        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception(error);
        }

        process.Dispose();
        return output.Trim(Environment.NewLine.ToCharArray());
    }

    private static Process CreateProcess(string fileName, string args)
    {
        ProcessStartInfo processStartInfo = new()
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            FileName = fileName,
            Arguments = args
        };

        Process process = new()
        {
            StartInfo = processStartInfo
        };
        return process;
    }
}
