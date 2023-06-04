using BLL.Models.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace WebAPITests.WebAPI;
public class DogsControllerTests : TestBase
{
    [Fact]
    public async Task Create_InvalidObjectPassed_BadRequest()
    {
        // Arrange
        var dog = new DogDTO()
        {
            Name = "Bobik",
            Color = "Black",
            Tail_length = -5,
            Weight = 44
        };

        // Act
        var response = await webApiClient.PostAsJsonAsync(
            Routes.WebApi.Dog,
            dog);

        // Assert
        response.StatusCode
            .Should()
            .Be((HttpStatusCode)StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Create_ExistingName_BadRequest()
    {
        // Arrange
        var dog = new DogDTO()
        {
            Name = "Doggy",
            Color = "Black",
            Tail_length = 10,
            Weight = 44
        };

        // Act
        var response = await webApiClient.PostAsJsonAsync(
            Routes.WebApi.Dog,
            dog);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task List_NoParams_ReturnsOkWithData()
    {
        // Act
        var response = await webApiClient.GetAsync(Routes.WebApi.Dogs);
        var list = await response.Content.ReadAsAsync<List<DogDTO>>();

        // Assert
        list.Count
            .Should()
            .Be(4);
    }

    [Fact]
    public async Task List_WithSortingParams_ReturnsOkWithData()
    {
        // Arrange
        var url = Routes.WebApi.Dogs + "?attribute=weight&order=desc";

        // Act
        var response = await webApiClient.GetAsync(url);
        var list = await response.Content.ReadAsAsync<List<DogDTO>>();

        // Assert
        list[0].Name
            .Should()
            .Be("Luntik");
    }

    [Fact]
    public async Task List_WithPagingParams_OkWithData()
    {
        // Arrange
        var url = Routes.WebApi.Dogs + "?pageNumber=2&limit=3&pageSize=3";

        // Act
        var response = await webApiClient.GetAsync(url); ;
        var list = await response.Content.ReadAsAsync<List<DogDTO>>();

        // Assert
        list.Count
            .Should()
            .Be(1);
    }

    [Fact]
    public async Task Create_ValidObject_Ok()
    {
        // Arrange
        var dog = new DogDTO()
        {
            Name = "Kevin",
            Color = "Black",
            Tail_length = 10,
            Weight = 44
        };

        // Act
        var response = await webApiClient.PostAsJsonAsync(
            Routes.WebApi.Dog,
            dog);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        dbContext.Dogs.Remove(dbContext.Dogs.FirstOrDefault(x => x.Name == dog.Name)!);
        dbContext.Commit();
    }


    [Fact]
    public async Task List_WithPagingParamsNotExistingPage_ReturnsOkWithEmptyData()
    {
        // Arrange
        var url = Routes.WebApi.Dogs + "?pageNumber=3&limit=3&pageSize=3";

        // Act
        var response = await webApiClient.GetAsync(url);
        var list = await response.Content.ReadAsAsync<List<DogDTO>>();

        // Assert
        list.Should()
            .BeEmpty();
    }

    [Fact]
    public async Task List_WithSortingParamsNotExistingAttribute_BadRequest()
    {
        // Arrange
        var url = Routes.WebApi.Dogs + "?attribute=owner";

        // Act
        var response = await webApiClient.GetAsync(url);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task List_WithSortingAndPagingParams_OkWithData()
    {
        // Arrange
        var url = Routes.WebApi.Dogs + "?attribute=weight&pageNumber=2&limit=3&pageSize=3";

        // Act
        var response = await webApiClient.GetAsync(url);
        var list = await response.Content.ReadAsAsync<List<DogDTO>>();

        // Assert
        list[0].Weight
            .Should()
            .Be(80);
    }
}