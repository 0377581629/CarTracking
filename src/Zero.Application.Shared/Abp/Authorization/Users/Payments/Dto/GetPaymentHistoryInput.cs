﻿using Abp.Runtime.Validation;
using Zero.Common;
using Zero.Dto;

namespace Zero.Abp.Authorization.Users.Payments.Dto
{
    public class GetUserPaymentHistoryInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }

            Sorting = DtoSortingHelper.ReplaceSorting(Sorting, s =>
            {
                return s.Replace("editionDisplayName", "Edition.DisplayName");
            });
        }
    }
}
