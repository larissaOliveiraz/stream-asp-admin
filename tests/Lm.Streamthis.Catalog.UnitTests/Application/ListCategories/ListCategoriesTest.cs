using FluentAssertions;
using Moq;
using Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

namespace Lm.Streamthis.Catalog.UnitTests.Application.ListCategories;

[Collection(nameof(ListCategoriesFixture))]
public class ListCategoriesTest(ListCategoriesFixture fixture)
{
    [Fact(DisplayName = nameof(Should_List_Categories))]
    [Trait("Application", "List Categories")]
    public async void Should_List_Categories()
    {
        var repositoryMock = fixture.GetMockRepository();
        var categoryList = fixture.GetValidCategoryList();

        var request = new UseCase.ListCategoriesRequest(
            page: 2,
            perPage: 10,
            search: "user-search",
            sort: "name",
            order: SearchOrder.Asc);

        var searchResponse = new SearchResponse<Category>(
            request.Page,
            request.PerPage,
            categoryList,
            10);

        repositoryMock
            .Setup(x =>
                x.Search(
                    It.Is<SearchRequest>(searchRequest => 
                        searchRequest.Page == request.Page &&
                        searchRequest.PerPage == request.PerPage &&
                        searchRequest.Search == request.Search &&
                        searchRequest.OrderBy == request.Sort &&
                        searchRequest.Order == request.Order), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(searchResponse);

        var useCase = new UseCase.ListCategories(repositoryMock.Object);
        var response = await useCase.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Page.Should().Be(searchResponse.CurrentPage);
        response.PerPage.Should().Be(searchResponse.PerPage);
        response.Total.Should().Be(searchResponse.Total);
        response.Items.Should().HaveCount(searchResponse.Total);
        response.Items.ForEach(item =>
        {
            var categorySearchResponse = searchResponse.Items.Find(category => category.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(categorySearchResponse!.Name);
            item.Description.Should().Be(categorySearchResponse.Description);
            item.IsActive.Should().Be(categorySearchResponse.IsActive);
            item.Id.Should().Be(categorySearchResponse.Id);
            item.CreatedAt.Should().Be(categorySearchResponse.CreatedAt);
        });
        repositoryMock.Verify(repository =>
            repository.Search(
                It.Is<SearchRequest>(searchRequest =>
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Order), 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}