using System;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tgs.Global;
using Tgs.Global.ApplicationServices.Brands;
using Tgs.Global.ApplicationServices.Setting;

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
    }
}