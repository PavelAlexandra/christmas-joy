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

      CreateMap<Comment, CommentViewModel>();
      CreateMap<CommentViewModel, Comment>();

      CreateMap<WishListItem, WishListItemViewModel>();
      CreateMap<WishListItemViewModel, WishListItem>();

      CreateMap<Like, LikeViewModel>();
      CreateMap<LikeViewModel, Like>();

      CreateMap<SecretSanta, SecretSantaViewModel>();
      CreateMap<SecretSantaViewModel, SecretSanta>();
    }
  }
}
