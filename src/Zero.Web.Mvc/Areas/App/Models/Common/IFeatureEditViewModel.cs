using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Zero.Editions.Dto;

namespace Zero.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}