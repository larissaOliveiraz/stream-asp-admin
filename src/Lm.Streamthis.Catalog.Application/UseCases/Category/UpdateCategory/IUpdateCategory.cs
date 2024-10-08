﻿using Lm.Streamthis.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Lm.Streamthis.Catalog.Application.UseCases.Category.UpdateCategory;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryRequest, CategoryResponse>
{
}