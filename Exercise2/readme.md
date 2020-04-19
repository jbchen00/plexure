**Database design**

See plexure-2.png for db design.

**Functions**

`List<Coupon> GetActiveCouponList()`
select * from coupon where Now() between startDate and endDate

`bool CanRedeemCoupon(userId, couponId)`
select count(id) from redemption where couponId = $couponId and userId = $userId
select maxPerUser, maxAllUsers from Coupon where id = $couponId
return count < maxPerUser and count < maxAllUsers

`void SaveRedemption(Coupon)`

`List<Redemption> GetRedemptions(couponId, userId)`
select * from redemption where couponId = $couponId and userId = $userId

**Optimizations on database**

clustered index on id, startDate, endDate on coupon table

clustered index on id, couponId, userId (redemptionDateTime desc) on redemption table

**Further optimization ideas that require more investigation**

move cold data to another data store. use Stretch Database.

use caching, e.g. redis, to store active coupons.
daily cron job to refresh cache for active coupons

use nosql db for redemptions, e.g. cassandra

use redis to store redemption count.
