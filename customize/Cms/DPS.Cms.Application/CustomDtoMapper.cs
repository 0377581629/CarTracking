using AutoMapper;
using DPS.Cms.Application.Shared.Dto.Category;
using DPS.Cms.Application.Shared.Dto.ImageBlock;
using DPS.Cms.Application.Shared.Dto.ImageBlockGroup;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Dto.MenuGroup;
using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Dto.PageLayout;
using DPS.Cms.Application.Shared.Dto.PageTheme;
using DPS.Cms.Application.Shared.Dto.Post;
using DPS.Cms.Application.Shared.Dto.Post.PostCategory;
using DPS.Cms.Application.Shared.Dto.Post.PostTags;
using DPS.Cms.Application.Shared.Dto.Tags;
using DPS.Cms.Application.Shared.Dto.Widget;
using DPS.Cms.Core.Advertisement;
using DPS.Cms.Core.Menu;
using DPS.Cms.Core.Page;
using DPS.Cms.Core.Post;
using DPS.Cms.Core.Widget;

namespace DPS.Cms.Application
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditImageBlockGroupDto, ImageBlockGroup>().ReverseMap();
            configuration.CreateMap<ImageBlockGroupDto, ImageBlockGroup>().ReverseMap();
            configuration.CreateMap<ImageBlockGroupDto, CreateOrEditImageBlockGroupDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditImageBlockDto, ImageBlock>().ReverseMap();
            configuration.CreateMap<ImageBlockDto, ImageBlock>().ReverseMap();
            configuration.CreateMap<ImageBlockDto, CreateOrEditImageBlockDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditPageDto, Page>().ReverseMap();
            configuration.CreateMap<PageDto, Page>().ReverseMap();
            configuration.CreateMap<PageDto, CreateOrEditPageDto>().ReverseMap();
            configuration.CreateMap<PageWidgetDto, PageWidget>().ReverseMap();
            configuration.CreateMap<PageWidgetDetailDto, PageWidgetDetail>().ReverseMap();
            configuration.CreateMap<PageDto, PageConfigDto>().ReverseMap();

            configuration.CreateMap<CreateOrEditPageThemeDto, PageTheme>().ReverseMap();
            configuration.CreateMap<PageThemeDto, PageTheme>().ReverseMap();
            configuration.CreateMap<PageThemeDto, CreateOrEditPageThemeDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditPageLayoutDto, PageLayout>().ReverseMap();
            configuration.CreateMap<PageLayoutDto, PageLayout>().ReverseMap();
            configuration.CreateMap<PageLayoutDto, CreateOrEditPageLayoutDto>().ReverseMap();
            configuration.CreateMap<PageLayoutBlockDto, PageLayoutBlock>().ReverseMap();
            configuration.CreateMap<PageLayoutDto, PageLayoutConfigDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditWidgetDto, Widget>().ReverseMap();
            configuration.CreateMap<WidgetDto, Widget>().ReverseMap();
            configuration.CreateMap<WidgetDto, CreateOrEditWidgetDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditTagsDto, Tags>().ReverseMap();
            configuration.CreateMap<TagsDto, Tags>().ReverseMap();
            configuration.CreateMap<TagsDto, CreateOrEditTagsDto>().ReverseMap();

            configuration.CreateMap<CreateOrEditMenuGroupDto, MenuGroup>().ReverseMap();
            configuration.CreateMap<MenuGroupDto, MenuGroup>().ReverseMap();
            configuration.CreateMap<MenuGroupDto, CreateOrEditMenuGroupDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditMenuDto, Menu>().ReverseMap();
            configuration.CreateMap<MenuDto, Menu>().ReverseMap();
            configuration.CreateMap<MenuDto, CreateOrEditMenuDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditCategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CategoryDto, CreateOrEditCategoryDto>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditPostDto, Post>().ReverseMap();
            configuration.CreateMap<Post, CreateOrEditPostDto>().ReverseMap();
            configuration.CreateMap<PostDto, Post>().ReverseMap();
            configuration.CreateMap<PostDto, CreateOrEditPostDto>().ReverseMap();
            
            configuration.CreateMap<PostTagDetailDto, PostDto>().ReverseMap();
            configuration.CreateMap<PostTagDetail, PostTagDetailDto>().ReverseMap();
            configuration.CreateMap<PostCategoryDetail, PostCategoryDetailDto>().ReverseMap();
        }
    }
}