using System.Net;
using Bolt.Endeavor.Extensions.Mvc.TestHelpers;
using Bookworm.Catalogue.Api.Contracts;
using Bookworm.Catalogue.Api.Features.Shared.Ports;
using Bookworm.Catalogue.Api.Tests.Fakes;
using NSubstitute;
using Shouldly;

namespace Bookworm.Catalogue.Api.Tests;

public class GetBookByIdTests(WebFixture fixture) 
    : IClassFixture<WebFixture>
{
    [Fact]
    public async Task return_book_given_book_exists_in_repository()
    {
        // Arrange
        var fakeRepo = fixture.GetRequiredService<IBooksRepository>();

        var givenBookRecord = new BookRecord
        {
            Id = "bw-100",
            Title = "title-1",
            Price = 12
        };
        
        fakeRepo
            .GetById(
                givenBookRecord.Id, 
                Arg.Any<CancellationToken>())
            .Returns(givenBookRecord);
        
        // Act
        var gotRsp = await fixture
            .HttpGet<GetBookByIdResponse>(
                GetBookByIdEndpoint.Path(givenBookRecord.Id)
            );

        // Assert
        gotRsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        gotRsp.Content.ShouldNotBeNull();
        gotRsp.Content.Id.ShouldBe(givenBookRecord.Id);
    }
    
    [Fact]
    public async Task return_notfound_given_book_doesnt_exist_in_repository()
    {
        // Arrange
        var fakeRepo = fixture.GetRequiredService<IBooksRepository>();

        var givenBookRecord = new BookRecord
        {
            Id = "bw-100",
            Title = "title-1",
            Price = 12
        };
        
        fakeRepo
            .GetById(
                givenBookRecord.Id, 
                Arg.Any<CancellationToken>())
            .Returns((BookRecord?)null);
        
        // Act
        var gotRsp = await fixture.HttpGet<GetBookByIdResponse>(GetBookByIdEndpoint.Path(givenBookRecord.Id));

        // Assert
        gotRsp.ShouldMatchContent();
    }
}