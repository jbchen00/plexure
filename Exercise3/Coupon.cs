using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Exercise3
{
    public interface ICouponProvider
    {
        Task<Coupon> Retrieve(Guid couponId);
    }

    public class Coupon
    {
        
    }

    public class CouponManager
    {
        private readonly ILogger _logger;
        private readonly ICouponProvider _couponProvider;

        public CouponManager(ILogger logger, ICouponProvider couponProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger));
            _couponProvider = couponProvider ?? throw new ArgumentNullException(nameof(ICouponProvider));
        }

        public async Task<bool> CanRedeemCoupon(Guid couponId, Guid userId,
            IEnumerable<Func<Coupon, Guid, bool>> evaluators)
        {
            if (evaluators == null)
                throw new ArgumentNullException("Evaluators can not be null");

            var coupon = await _couponProvider.Retrieve(couponId);

            if (coupon == null)
                throw new KeyNotFoundException();

            if (!evaluators.Any())
                return true;

            bool result = false;
            foreach (var evaluator in evaluators)
                result |= evaluator(coupon, userId);

            return result;
        }
    }
}