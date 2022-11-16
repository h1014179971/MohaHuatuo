//
//  HwAdsInterface_h
//  Unity-iPhone
//
//  Created by game team on 2019/11/15.
//

#ifndef HwAdsInterface_h
#define HwAdsInterface_h


#import <Foundation/Foundation.h>
#import <HwISAdsFramework/HwAds.h>

@interface HwAdsInterface:NSObject<HwAdsDelegate>

@property(nonatomic,strong)HwAds *hwAds;
@property BOOL isReward;
@property BOOL isLoadRewardSuccess; // 加载激励视频是否成功
@property NSString *rewardTag;
@property NSString *abTest;
@property int loadAderrorCount;
+(id) sharedInstance;

//- (void)initHwSDK:(char *)serverURL;
//- (void)loadHwInterAd;
//- (void)showHwInterAd;
//- (BOOL)isHwInterAdLoaded;
//- (void)loadHwRewardAd;
//- (void)showHwRewardAd:(char *)tag;
//- (BOOL)isHwRewardAdLoaded;
//- (void)hwFbEvent:(char *)category
//           action:(char *)action
//            label:(char *)label;
-(void) initMHSDK;
-(void) loadMHInterAd;
-(void) showMHInterAd;
-(BOOL) isMHInterAdLoaded;
-(void) loadMHRewardAd;
-(void) showMHRewardAd:(NSString *)tag;
-(BOOL) isMHRewardAdLoaded;
-(void) showMHBannerAd;
-(void) hideMHBannerAd;
-(BOOL) ABTest;
@end









#endif /* HwAdsCall_h */

//@interface HwAdsInterface : NSObject<HwAdsDelegate>
//@property (nonatomic, strong) HwAds *hwAdsDelegate;
//
//- (void)initTest;
//@end


