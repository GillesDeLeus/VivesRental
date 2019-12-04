using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace VivesRental.RestApi.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            //#region General

            //CreateMap<ServiceError, ErrorModel>();

            //#endregion

            //CreateDefaultMap<CreatorProfile, CreatorProfileResponse>();
            //CreateDefaultMap<PlayerProfile, PlayerProfileResponse>();
            //CreateDefaultMap<Game, GameResponse>();
            //CreateDefaultMap<Character, CharacterResponse>();
            //CreateDefaultMap<World, WorldResponse>();
            //CreateDefaultMap<TileMap, TileMapResponse>();
            //CreateDefaultMap<Tile, TileResponse>();
            //CreateDefaultMap<TileExit, TileExitResponse>();

            //CreateMap<Post, PostResponse>()
            //    .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags.Select(t => new TagResponse { Name = t.Tag.Name })));
        }

        #region Helpers

        private void CreateDefaultMap<TEntity, TResponse>()
        {
            //CreateMap<ServiceResult<TEntity>, ErrorResponse>();
            //CreateMap<ServiceResult<TEntity>, Response<TResponse>>();
            //CreateMap<PagedServiceResult<TEntity>, PagedResponse<TResponse>>();
            //CreateMap<TEntity, TResponse>();
        }

        #endregion
    }
}
