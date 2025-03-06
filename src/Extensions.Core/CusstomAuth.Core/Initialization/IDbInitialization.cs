using CusstomAuth.Core.Data;

namespace CusstomAuth.Core.Initialization;

public interface IDbInitialization
{
    Task<InitializeResponse> InitializeAsync();
}

public class FakeDbInitialization : IDbInitialization
{
    public async Task<InitializeResponse> InitializeAsync()
    {
        return await Task.FromResult(new InitializeResponse { });
    }
}
