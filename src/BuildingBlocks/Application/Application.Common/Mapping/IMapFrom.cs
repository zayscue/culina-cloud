using AutoMapper;

namespace CulinaCloud.BuildingBlocks.Application.Common.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }

    public abstract class MapFrom<T> : IMapFrom<T>
    {
        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}