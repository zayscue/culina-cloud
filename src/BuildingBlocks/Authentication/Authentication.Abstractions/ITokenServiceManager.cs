namespace CulinaCloud.BuildingBlocks.Authentication.Abstractions
{
    public interface ITokenServiceManager
    {
        ITokenService GetTokenService(string audience);
    }
}
