using FluentAssertions;
using Moq;
using DomainEntities = Lm.Streamthis.Catalog.Domain.Entities;
using Lm.Streamthis.Catalog.Domain.SeedWork.SearchableRepository;
using Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;
using UseCase = Lm.Streamthis.Catalog.Application.UseCases.Category.ListCategories;

namespace Lm.Streamthis.Catalog.UnitTests.Application.Category.ListCategories;

[Collection(nameof(ListCategoriesFixture))]
public class ListCategoriesTest(ListCategoriesFixture fixture)
{
    [Fact(DisplayName = nameof(Should_List_Categories))]
    [Trait("Application", "List Categories")]
    public async void Should_List_Categories()
    {
        var repositoryMock = fixture.GetMockRepository();
        var categoryList = fixture.GetValidCategoryList();

        var request = fixture.GetValidRequest();

        var searchResponse = new SearchResponse<DomainEntities.Category>(
            request.Page,
            request.PerPage,
            categoryList,
            new Random().Next(50, 200));

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
        response.Items.Should().HaveCount(searchResponse.Items.Count);
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

    [Theory(DisplayName = nameof(Should_List_Categories_Without_All_Request_Parameters))]
    [Trait("Application", "List Categories")]
    [MemberData(
        nameof(ListCategoriesDataGenerator.GetValidRequestsWithoutAnyParameters),
        parameters: 12,
        MemberType = typeof(ListCategoriesDataGenerator))]
    public async void Should_List_Categories_Without_All_Request_Parameters(ListCategoriesRequest request)
    {
        var repositoryMock = fixture.GetMockRepository();
        var categoryList = fixture.GetValidCategoryList();

        var searchResponse = new SearchResponse<DomainEntities.Category>(
            request.Page,
            request.PerPage,
            categoryList,
            new Random().Next(50, 200));

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
        response.Items.Should().HaveCount(searchResponse.Items.Count);
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

    [Fact(DisplayName = nameof(Should_List_Categories_When_Empty))]
    [Trait("Application", "List Categories")]
    public async void Should_List_Categories_When_Empty()
    {
        var repositoryMock = fixture.GetMockRepository();
        var request = fixture.GetValidRequest();

        var searchResponse = new SearchResponse<DomainEntities.Category>(
            request.Page,
            request.PerPage,
            [],
            0);

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
        response.Total.Should().Be(0);
        response.Items.Should().HaveCount(0);
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