//
//  OKMonitorService.h
//  OneKit
//
//  Created by yifei Feng on 2022/6/6.
//

#import <OneKit/OKService.h>

@interface OKAddressRange : NSObject

/*! @abstract The start address of this range.
 */
@property(nonatomic, assign, readonly)int64_t startAddress;

/*! @abstract The end address of this range.
 */
@property(nonatomic, assign, readonly)int64_t endAddress;

/*!
 Use this method to initialize an addressRange.
 
 @param startAddress The start address of this range.
 @param endAddress The end address of this range.
 */
+ (instancetype)addressRangeWithStartAddress:(int64_t)startAddress endAddress:(int64_t)endAddress;

@end


@interface OKMonitorAddressConfig : NSObject

@property (nonatomic, copy, readonly) NSArray<OKAddressRange *> * addressRanges;

+ (instancetype)configWithAddressRanges:(NSArray<OKAddressRange *> *)addressRanges;

@end




@interface OKMonitorSDKConfig : NSObject

/*! @abstract The ID of your sdk that you registered from APMInsight.
 @discussion Nonnull & required.
 */
@property (atomic, copy) NSString *sdkID;

/*! @abstract Current version of your sdk
 @discussion Nonnull & required.
 */
@property (atomic, copy) NSString *sdkVersion;

/*! @abstract The channel where your SDK will be published.
 @discussion Nonnull & require. Default : "Other"
 */
@property (atomic, strong) NSString *channel;

/*!
 @abstract The address ranges of your sdk in the main mach-o.
 @discussion Only when the fault address is in the address ranges, we will upload the crash log. Nullable & optional.
 */
@property (atomic, strong) OKMonitorAddressConfig *addressConfig;

/**
 Initialize a config with appID.
 
 @param sdkID The ID of your sdk that you registered from APMInsight.
 @result A RangersAPMForSDKConfig instance.
 */
+ (instancetype)configWithSDKID:(NSString *)sdkID SDKVersion:(NSString *)sdkVersion channel:(NSString *)channel addressConfig:(OKMonitorAddressConfig *)addressConfig;

@end

@protocol OKMonitorSDKService <OKService>

- (void)monitorWithConfig:(OKMonitorSDKConfig *)config;

@end
