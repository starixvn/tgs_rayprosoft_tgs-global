using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tgs.Core.Infrastructures.Security;
using Tgs.Global;
using Tgs.Global.ApplicationServices.Brands;
using Tgs.Global.ApplicationServices.Setting;
using TGS.Paging;

namespace GlobalApi.UnitTests
{
    [TestFixture]
    public class BrandTests
    {
        [Test]
        public async Task Can_Get_Super_Licensee_Codes_Setting_Value()
        {
            var fakeRepository = Substitute.For<ITgsRepository>();

            var sut = new SuperLicenseeCodesQueryService(fakeRepository);

            var query = new SuperLicenseeCodesQuery();

            await sut.QueryAsync(query).ToArrayAsync();

            var condition = new SettingQueryConditions("SuperLicenseeCodes");

            _ = fakeRepository.Received(1)
                .QuerySettingAsync(Arg.Is(condition));
        }

        [Test]
        public async Task Can_Get_Brand_Api_Keys_Without_SuperLicenseeCodes()
        {
            var fakeRepository = Substitute.For<ITgsRepository>();

            var sut = new BrandApiKeyQueryService(fakeRepository);

            var query = new BrandApiKeyQuery(Array.Empty<string>());

            await sut.QueryAsync(query).ToArrayAsync();

            _ = fakeRepository.Received(1)
                .QueryBrandApiKeysAsync();
        }

        [Test]
        public async Task Can_Get_Brand_Api_Keys_With_SuperLicenseeCodes()
        {
            var fakeRepository = Substitute.For<ITgsRepository>();

            var sut = new BrandApiKeyQueryService(fakeRepository);

            var licenseeCodes = new[]
            {
                "tgs"
            };

            var query = new BrandApiKeyQuery(licenseeCodes);

            await sut.QueryAsync(query).ToArrayAsync();

            _ = fakeRepository.Received(1)
                .QueryBrandApiKeysAsync();
        }

        [Test]
        public async Task Can_Count_Brand_Players()
        {
            var fakeRepository = Substitute.For<ITgsRepository>();

            var sut = new BrandPlayerQueryService(fakeRepository);

            var query = new BrandPlayerQuery(
                "GPI",
                "CNY",
                PlayerType.TestPlayer,
                new Paging<BrandPlayerInfo>
                {
                    Page = 1,
                    RowCount = 10
                });

            await sut.GetAsync(query).ConfigureAwait(false);

            _ = fakeRepository.Received(1)
                .CountBrandPlayersAsync(Arg.Any<BrandPlayerQueryConditions>());
        }

        [Test]
        public async Task Can_Get_Brand_Players()
        {
            var fakeRepository = Substitute.For<ITgsRepository>();

            var sut = new BrandPlayerQueryService(fakeRepository);

            var query = new BrandPlayerQuery(
                "GPI",
                "CNY",
                PlayerType.TestPlayer,
                new Paging<BrandPlayerInfo>
                {
                    Page = 1,
                    RowCount = 10
                });

            await sut.QueryAsync(query).ToArrayAsync();

            _ = fakeRepository.Received(1)
                .QueryBrandPlayersAsync(Arg.Any<BrandPlayerQueryConditions>(), Arg.Is(query.Paging));
        }

        [Test]
        public async Task Can_Create_Brand_Players()
        {
            var fakeRepository = Substitute.For<ITgsRepository>();
            var fakePasswordHandler = Substitute.For<IPasswordHandler>();

            var sut = new CreateBrandPlayerCommandService(fakeRepository, fakePasswordHandler);

            var query = new CreateBrandPlayerCommand(
                "GPI",
                "CNY",
                PlayerType.TestPlayer);

            await sut.ExecuteAsync(query);

            _ = fakeRepository.Received(1)
                .CreatePlayersAsync(Arg.Any<IEnumerable<PlayerCreateInfo>>());
        }

        [Test]
        public async Task Can_Get_Brand_Id_By_Code()
        {
            var fakeTgsRepository = Substitute.For<ITgsRepository>();

            var sut = new GetBrandIdByCodeQueryService(fakeTgsRepository);

            var query = new GetBrandIdByCodeQuery("BrandCode");

            await sut.GetAsync(query);

            _ = fakeTgsRepository.Received(1)
                .QueryBrandIdByCodeAsync(Arg.Is(query.BrandCode));
        }
    }
}
