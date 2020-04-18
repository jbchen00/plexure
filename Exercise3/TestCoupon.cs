using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Exercise3
{
    public class TestCoupon : IDisposable
    {
        private readonly AutoMock _mock;
        private readonly ILogger _logger;
        private readonly ICouponProvider _couponProvider;
        private readonly IEnumerable<Func<Coupon, Guid, bool>> _hasPositiveEvaluators;
        private readonly IEnumerable<Func<Coupon, Guid, bool>> _allNegativeEvaluators;
        private readonly IEnumerable<Func<Coupon, Guid, bool>> _emptyEvaluators;
        private CouponManager CouponManager { get; set; }

        private bool TestTrueEvaluator(Guid id, Guid id2)
        {
            return true;
        }

        private bool TestFalseEvaluator(Guid id, Guid id2)
        {
            return false;
        }

        public TestCoupon()
        {
            _mock = AutoMock.GetLoose();
            _logger = _mock.Mock<ILogger>().Object;
            _couponProvider = _mock.Mock<ICouponProvider>().Object;

            _emptyEvaluators = new List<Func<Coupon, Guid, bool>>() { };
            _hasPositiveEvaluators = new List<Func<Coupon, Guid, bool>>()
            {
                (coupon, guid) => TestTrueEvaluator(new Guid(), new Guid()),
                (coupon, guid) => TestFalseEvaluator(new Guid(), new Guid())
            };
            _allNegativeEvaluators = new List<Func<Coupon, Guid, bool>>()
            {
                (coupon, guid) => TestFalseEvaluator(new Guid(), new Guid()),
                (coupon, guid) => TestFalseEvaluator(new Guid(), new Guid()),
                (coupon, guid) => TestFalseEvaluator(new Guid(), new Guid())
            };
        }

        public void Dispose()
        {
            _mock.Dispose();
        }

        [Fact]
        public void CanRedeemCoupon_NullLoggerConstructor_ArgumentNullException()
        {
            Assert.NotNull(_couponProvider);
            var ex = Assert.Throws<ArgumentNullException>(() => new CouponManager(null, _couponProvider));
            Assert.Equal(new ArgumentNullException(nameof(ILogger)).Message, ex.Message);
        }

        [Fact]
        public void CanRedeemCoupon_NullCouponProviderConstructor_ArgumentNullException()
        {
            Assert.NotNull(_logger);
            var ex = Assert.Throws<ArgumentNullException>(() => new CouponManager(_logger, null));
            Assert.Equal(new ArgumentNullException(nameof(ICouponProvider)).Message, ex.Message);
        }

        [Fact]
        public async Task CanRedeemCoupon_NullEvaluator_ArgumentNullException()
        {
            var couponManager = new CouponManager(_logger, _couponProvider);
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                couponManager.CanRedeemCoupon(new Guid(), new Guid(), null));
            Assert.Equal(new ArgumentNullException("Evaluators can not be null").Message, ex.Message);
        }

        [Fact]
        public async Task CanRedeemCoupon_NullCoupon_ArgumentNullException()
        {
            _mock.Mock<ICouponProvider>().Setup(m => m.Retrieve(new Guid())).ReturnsAsync(null as Coupon);
            var couponManager = new CouponManager(_logger, _couponProvider);
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                couponManager.CanRedeemCoupon(new Guid(), new Guid(), new List<Func<Coupon, Guid, bool>>()));
            Assert.NotStrictEqual(new KeyNotFoundException(), ex);
        }

        [Fact]
        public async Task CanRedeemCoupon_EmptyEvaluator_ReturnTrue()
        {
            _mock.Mock<ICouponProvider>().Setup(m => m.Retrieve(new Guid())).ReturnsAsync(new Coupon());
            var couponManager = new CouponManager(_logger, _couponProvider);
            var result =
                await couponManager.CanRedeemCoupon(new Guid(), new Guid(), _emptyEvaluators);
            Assert.True(result);
        }

        [Fact]
        public async Task CanRedeemCoupon_PositiveEvaluator_ReturnTrue()
        {
            _mock.Mock<ICouponProvider>().Setup(m => m.Retrieve(new Guid())).ReturnsAsync(new Coupon());
            var couponManager = new CouponManager(_logger, _couponProvider);
            var result =
                await couponManager.CanRedeemCoupon(new Guid(), new Guid(), _hasPositiveEvaluators);
            Assert.True(result);
        }

        [Fact]
        public async Task CanRedeemCoupon_AllNegativeEvaluator_ReturnFalse()
        {
            _mock.Mock<ICouponProvider>().Setup(m => m.Retrieve(new Guid())).ReturnsAsync(new Coupon());
            var couponManager = new CouponManager(_logger, _couponProvider);
            var result =
                await couponManager.CanRedeemCoupon(new Guid(), new Guid(), _allNegativeEvaluators);
            Assert.False(result);
        }
    }
}