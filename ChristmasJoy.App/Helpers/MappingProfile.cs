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
      CreateMap<User, UserViewModel>();
      CreateMap<UserViewModel, User>();

      CreateMap<Comment, CommentViewModel>()
        .ForMember(u => u.Likes, options => options.Ignore());
      CreateMap<CommentViewModel, Comment>()
        .ForMember(u => u.Likes, options => options.Ignore());

      CreateMap<WishListItem, WishListItemViewModel>();
      CreateMap<WishListItemViewModel, WishListItem>();

      CreateMap<Like, LikeViewModel>();
      CreateMap<LikeViewModel, Like>();

      CreateMap<SecretSanta, SecretSantaViewModel>();
      CreateMap<SecretSantaViewModel, SecretSanta>();
    }
  }
}
