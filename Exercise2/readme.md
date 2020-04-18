**coupon**

id
title
startDate
endDate
maxPerUser
maxAllUsers

**redemption**

couponId(FK to Coupon.Id)
userId(FK to User.Id)
redeemedDateTime
code


**Optimizations**

use caching, e.g. redis, to store active coupons and redemption
daily cron job to refresh cache for active coupons

move cold data to another data store?

redeem
get active coupon from redis
redeem
update redis
update db

getRedemptionCount
redis key count?

index on id, startDate, endDate on coupon table
index on couponId and userId on redemption table.
