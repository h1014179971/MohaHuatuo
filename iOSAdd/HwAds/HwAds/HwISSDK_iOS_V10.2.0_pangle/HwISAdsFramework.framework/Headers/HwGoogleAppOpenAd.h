//
//  HwGoogleAppOpenAd.h
//  HwISAdsFramework
//
//  Created by 乾坤 on 2022/6/16.
//

#import <Foundation/Foundation.h>
#import <GoogleMobileAds/GoogleMobileAds.h>

NS_ASSUME_NONNULL_BEGIN

@protocol HwGoogleOpenAdDelegate <NSObject>

-(void)hwGoogleOpenAdClose;

@end

@interface HwGoogleAppOpenAd : NSObject<GADFullScreenContentDelegate>


@property (nonatomic, weak) id<HwGoogleOpenAdDelegate> hwGoogleOpenAdsDelegate;

+ (id)instance;

- (void)initGoogleOpenADByID:(NSString *)adId;

- (void)tryToPresentAd;

@end

NS_ASSUME_NONNULL_END
