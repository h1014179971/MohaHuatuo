//
//  HwAds.h
//  HwISAdsFramework
//
//  Created by 乾坤 on 2022/6/16.
//

#import <UIKit/UIKit.h>


#define ADLOADINTERVAL 15
#define MAX_COUNTGAMEBRAIN 10

#define HW_INSTALLTIME @"HW_InstalllTime"
#define HW_LASTSHOWADTIME @"HW_LastShowAdTime"


#ifndef HwAds_h
#define HwAds_h

#endif /* HwAds_h */

typedef NS_ENUM(NSInteger, AJHwSDKUACType) {
    
    /**
     * UAC新手引导完成打点
     */
    AJHWGuideFinish = 0,
    /**
     * UAC其他具体事件点
     */
    AJHWLevel ,
    /**
     * UAC看视频用户打点high
     */
    AJHWVideohigh ,
    /**
     * UAC看视频用户打点low
     */
    AJHWVideolow ,
    /**
     * UAC内购用户打点
     */
    AJHWPurchase ,

};

@protocol HwAdsDelegate <NSObject>
@optional
//加载成功  添加delegate
- (void)hwAdsRewardedVideoLoadSuccess;
//播放失败，不给奖励
- (void)hwAdsRewardedVideoPlayFailWithErrorCode:(NSInteger)errorCode;
//广告展示 建议在收到这个回调时，暂停游戏；
- (void)hwAdsRewardedVideoDidAppear;
//广告关闭
- (void)hwAdsRewardedVideoClose;
//广告被点击
- (void)hwAdsRewardedVideoClick;
//广告播放完成，给奖励，最好在这里做标记，在close中给
- (void)hwAdsRewardedVideoGiveReward;
@end

@protocol HWAdsInterDelegate <NSObject>
@optional
//插屏加载
- (void)hwAdsInterstitialLoadSuccess;
//加载失败
- (void)hwAdsInterstitialLoadFail;
//插屏点击 add 3.0
- (void)hwAdsInterstitialClick;
//插屏播放 add 3.0
- (void)hwAdsInterstitialShow;
//插屏关闭 add 3.0
- (void)hwAdsInterstitialClose;
//插屏展示失败 add 10.2.0
- (void)hwAdsInterstitialFailToShowWithErrorCode:(NSInteger)errorCode;
@end

@protocol HWAdsBannerDelegate <NSObject>
@optional
//banner加载 add 3.1
- (void)hwAdsBannerLoadSuccess;
@end

@interface HwAds : NSObject
{
}

@property (nonatomic, weak) id<HwAdsDelegate> hwAdsDelegate;
@property (nonatomic, weak) id<HWAdsInterDelegate> hwAdsInterDelegate;
@property (nonatomic, weak) id<HWAdsBannerDelegate> hwAdsBannerDelegate;

//实例化
+ (id)instance;
//初始化 serverProjectId为项目在game brain对应的ID
- (void)initSDK:(int)serverProjectId isFirebase:(BOOL)isFirebase;

- (void)showBanner;

- (void)hideBanner;

- (BOOL)isBannerLoad;

- (void)showInter;

- (BOOL)isFacebookInter;

- (BOOL)isInterLoad;

- (void)showReward:(NSString *)tag;

- (BOOL)isRewardLoad;


/// 内购完成打点方法，二次验证也在这个方法内包含不需要再进行二次验证
/// @param number 内购的金额 美金
/// @param myProductId 商品ID 需传入苹果后台创建的商品编号（是一串数字）
/// @param myPurchaseType 商品类型，1是订阅，0是普通商品
/// @param myOrderId 订单的transaction.transactionIdentifier 苹果生成的订单号一段数字 不是本地自己创建的订单号
/// @param myPurchaseToken 订单的transaction receiptString
- (void)hwAnalyticsPurchaseByNumberOfDollars:(NSString *)number
                                   productId:(NSString *)myProductId
                                   purchaseType:(NSInteger)myPurchaseType
                                   orderId:(NSString *)myOrderId
                                   purchaseToken:(NSString *)myPurchaseToken;

/// 自定义Adjust打点
/// @param adjustToken adjust事件token
- (void)hwAdjustEventToken:(NSString *)adjustToken;


/// 弹出IDFA弹窗
- (void)hwToAskIDFAAlertView;


/**
 * 获取 SDK 版本
 */

+ (NSString *)sdkVersion;




@end
