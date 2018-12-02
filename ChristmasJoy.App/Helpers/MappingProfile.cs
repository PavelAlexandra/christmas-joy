using AutoMapper;
using ChristmasJoy.App.Models.Dtos;
using ChristmasJoy.App.Models.SqLiteModels;
using System;

namespace ChristmasJoy.App.Helpers
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<User, UserViewModel>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => u.Id.ToString()));
      CreateMap<UserViewModel, User>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => string.IsNullOrEmpty(u.Id) ? 0 : Int32.Parse(u.Id)));

      CreateMap<Comment, CommentViewModel>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => u.Id.ToString()));
      CreateMap<CommentViewModel, Comment>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => string.IsNullOrEmpty(u.Id) ? 0 : Int32.Parse(u.Id)));

      CreateMap<WishListItem, WishListItemViewModel>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => u.Id.ToString()));
      CreateMap<WishListItemViewModel, WishListItem>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => string.IsNullOrEmpty(u.Id) ? 0 : Int32.Parse(u.Id)));

      CreateMap<Like, LikeViewModel>();
      CreateMap<LikeViewModel, Like>();

      CreateMap<SecretSanta, SecretSantaViewModel>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => u.Id.ToString()));
      CreateMap<SecretSantaViewModel, SecretSanta>()
        .ForMember(u => u.Id, option => option.ResolveUsing(u => string.IsNullOrEmpty(u.Id) ? 0 : Int32.Parse(u.Id)));
    }
  }
}
