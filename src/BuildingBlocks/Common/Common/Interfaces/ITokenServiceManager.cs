namespace CulinaCloud.BuildingBlocks.Common.Interfaces
{
    public interface ITokenServiceManager
    {
        ITokenService GetTokenService(string audience);
    }
}
