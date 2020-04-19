**Database design**

See plexure-2.png for db design.

**Functions**

`List<Coupon> GetActiveCouponList()`
select * from coupon where Now() between startDate and endDate

`bool CanRedeemCoupon(userId, couponId)`

`void SaveRedemption(Coupon)`

`List<Redemption> GetRedemptions(couponId, userId)`
select * from redemptions where couponId = $couponId and userId = $userId

**Optimizations on database**

index on id, startDate desc, endDate desc on coupon table

index on id, couponId, userId on redemption table

**Further optimization ideas that require more investigation**

move cold data to another data store?

use caching, e.g. redis, to store active coupons.
daily cron job to refresh cache for active coupons

use nosql db for redemptions, e.g. cassandra

use redis to store redemption count.
