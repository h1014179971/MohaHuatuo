//
//  HwAdsInterface.m
//  Unity-iPhone
//
//  Created by game team on 2019/11/15.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "HwAdsInterface.h"
#import "BDAutoTrack.h"
#import "BDAutoTrackURLHostItemSG.h"
#import <GameAnalytics/GameAnalytics.h>
#import <AdjustSdk/Adjust.h>
//@interface HwAdsInterface()
//
//@property(nonatomic,strong)LGRewardedVideoAd *rewardedVideoAd;
//
//@end

@implementation HwAdsInterface
typedef void (*CallbackDelegate)(const char *object);
CallbackDelegate callback;


static HwAdsInterface *hwAdsInterfaceInstance;
+ (id) sharedInstance{
    if(hwAdsInterfaceInstance == nil){
        NSLog(@"shareInstance");
        hwAdsInterfaceInstance = [[self alloc] init];
    }
    return hwAdsInterfaceInstance;
}

#pragma mark HwAdsDelegate
- (void) hwAdsRewardedVideoLoadSuccess{
    NSLog(@"call hwAdsRewardedVideoLoadSuccess");
    self.loadAderrorCount = 0;
    self.isLoadRewardSuccess = true;
    UnitySendMessage("IOSPlatformWrapper","OnLoadRewardResult","true");
}

- (void) hwAdsRewardedVideoGiveReward{
    NSLog(@"call hwAdsRewardedVideoGiveReward");
    self.isReward = true;
}
- (void) hwAdsRewardedVideoPlayFail{
    NSLog(@"call hwAdsRewardedVideoPlayFail");
    if(callback != nil)
        callback("false");
    self.isReward = false;
    self.isLoadRewardSuccess = false;
}

- (void) hwAdsRewardedVideoClose{
    NSLog(@"call hwAdsRewardedVideoClose");
    UnitySendMessage("IOSPlatformWrapper","OnLoadRewardResult","false");
    self.isReward = true;
    if(self.isReward){
        callback("true");
    }else{
        callback("false");
    }
    self.isReward = false;
    self.isLoadRewardSuccess = false;
    
}
-(void) hwAdsRewardedVideoWillAppear{
    NSLog(@"call hwAdsRewardedVideoWillAppear");
}

- (void) hwAdsRewardedVideoClick{
    NSLog(@"call hwAdsRewardedVideoClick");
}

- (void) hwAdsRewardedVideoLoadFail{
    NSLog(@"call hwAdsRewardedVideoLoadFail");
    self.isLoadRewardSuccess = false;
    UnitySendMessage("IOSPlatformWrapper","OnLoadRewardResult","false");
    self.loadAderrorCount++;
    NSTimer *loadTimer = [NSTimer timerWithTimeInterval:self.loadAderrorCount *5 target:self selector:@selector(loadMHRewardAd) userInfo:nil repeats:NO];
                [[NSRunLoop currentRunLoop] addTimer:loadTimer forMode:NSDefaultRunLoopMode];
                [[NSRunLoop currentRunLoop] run];
}

- (void)hwAdsRewardedVideoDidAppear {
    NSLog(@"call hwAdsRewardedVideoDidAppear");
}

//插屏加载
- (void)hwAdsInterstitialLoadSuccess;
{
    
}
//加载失败
- (void)hwAdsInterstitialLoadFail{
    
}
//插屏点击
- (void)hwAdsInterstitialClick{}

//插屏播放
- (void)hwAdsInterstitialShow{
    
}
//插屏关闭
- (void)hwAdsInterstitialClose{
    NSLog(@"call hwAdsInterstitialClose");
    if(callback != NULL)
        callback("true");
}

//插屏失败
- (void)hwAdsInterstitialFailToShowWithErrorCode:(NSInteger)errorCode{
    NSLog(@"call hwAdsInterstitialFailToShowWithErrorCode");
    if(callback != NULL)
        callback("false");
        
}







-(void)initMHSDK{
    NSLog(@"abtest== initHwSDK");
    callback("");
    HwAdsInterface* hwAdsInterface = [HwAdsInterface sharedInstance];
    [[HwAds instance] initSDK:339 isFirebase:YES];
    HwAds* hwads = [HwAds instance];
    hwads.hwAdsDelegate = hwAdsInterface;
    hwads.hwAdsInterDelegate = hwAdsInterface;
    
    
    
}

-(void) loadMHInterAd{
    NSLog(@"call loadInterAd");
}


-(void) showMHInterAd{
    NSLog(@"call ShowInterAd");
    [[HwAds instance] showInter];
}

-(BOOL) isMHInterAdLoaded{
    NSLog(@"call isInterLoaded");
	return [[HwAds instance] isInterLoad];
}

-(void) loadMHRewardAd{
    NSLog(@"call loadRewardedVideo");
}

-(void) showMHRewardAd:(NSString *)tag{
    NSLog(@"call showRewardedVideo");
    self.rewardTag = tag;
    [[HwAds instance] showReward:tag];
}


-(BOOL) isMHRewardAdLoaded{
    NSLog(@"cal isRewardLoaded ====%@",[[HwAds instance] isRewardLoad]?@"YES":@"NO");
    return [[HwAds instance] isRewardLoad];
}
-(void) showMHBannerAd{
    NSLog(@"call showBanner");
    [[HwAds instance] showBanner];
}
-(void) hideMHBannerAd{
    NSLog(@"call hideBanner");
    [[HwAds instance] hideBanner];
}
-(BOOL) ABTest{
//    Boolean newIsa = [BDAutoTrack ABTestConfigValueForKey:@"isA" defaultValue:@(NO)];
//    NSLog(@"abtest===%@",newIsa?@"YES":@"NO");
    return YES;
}

@end

void initHwAds( char *str ,CallbackDelegate callDelegate){
    NSLog(@"HwAdsInterface complete  111111 %s",str);
    callback = callDelegate;
    [[HwAdsInterface sharedInstance] initMHSDK];
}
void showHwRewardAd(char *tag,CallbackDelegate callDelegate){
    NSLog(@"HwAdsInterface complete  showHwRewardAd ");
    callback = callDelegate;
    NSString *str = [NSString stringWithUTF8String:tag];
    [[HwAdsInterface sharedInstance] showMHRewardAd:str];
}
void showHwInterAd(char *tag,CallbackDelegate callDelegate){
    NSLog(@"HwAdsInterface complete  showHwInterAd ");
    callback = callDelegate;
    BOOL interLoad = [[HwAdsInterface sharedInstance] isMHInterAdLoaded];
    if(!interLoad){
        if(callback != NULL)
            callback("false");
    }else{
        [[HwAdsInterface sharedInstance] showMHInterAd];
    }
}
BOOL isHwRewardLoaded(){
    return [[HwAdsInterface sharedInstance] isMHRewardAdLoaded];
}
BOOL isHwInterLoaded(){
    return [[HwAdsInterface sharedInstance] isMHInterAdLoaded];
}
void showHwBanner(){
    [[HwAdsInterface sharedInstance] showMHBannerAd];
}
void hideHwBanner(){
    [[HwAdsInterface sharedInstance] hideMHBannerAd];
}

BOOL HwABTest(){
    return [[HwAdsInterface sharedInstance] ABTest];
}

void HwToAskIDFACustomAlertView(){
    NSLog(@"HwToAskIDFACustomAlertView");
}

void TAEventHwPropertie(char *custom, char *str){
    NSLog(@"u3d_parseJson custom: %c", custom);
    NSString *jsonStr = [NSString stringWithCString:str encoding:NSUTF8StringEncoding];
    NSLog(@"u3d_parseJson jsonString: %@", jsonStr);

    NSData *jsonData = [jsonStr dataUsingEncoding:NSUTF8StringEncoding];
    
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:nil];
    NSLog(@"u3d_parseJson dict: %@",dict);
//    NSLog(@"dict - name: %@",dict[@"lev"]);
//    NSLog(@"dict - age: %@",dict[@"login_day"]);
    NSMutableDictionary *dic = [[NSMutableDictionary alloc] initWithDictionary:dict];
    NSLog(@"NSMutableDictionary dict: %@",dic);
//    NSLog(@"NSMutableDictionary - name: %@",dic[@"lev"]);
//    NSLog(@"NSMutableDictionary - age: %@",dic[@"login_day"]);
    if( custom == '\0'){
        custom = "tankclash";
         NSLog(@"u3d_parseJson custom: %c", custom);
    }
    NSString *customKey = [NSString stringWithCString:custom encoding:NSUTF8StringEncoding];
    if(customKey != nil && jsonStr != nil){
        NSLog(@"u3d_parseJson BDAutoTrack: ");
        [BDAutoTrack eventV3:customKey params:dict];
        
    }
    
    NSArray *keysArray = [dict allKeys];

    for (int i = 0; i < keysArray.count; i++) {
        NSString *key = keysArray[i];
        NSString *value = dict[key];
        if(customKey != nil && key != nil && value != nil){
           NSString *ga = [customKey stringByAppendingFormat:@":%@:%@", key, value ];
            if(ga != nil){
                [GameAnalytics addDesignEventWithEventId:ga];
            }
            NSLog(@"GameAanlytics Event====%@", ga);
        }
    }
}

// adjust 打点
void HwAdjustEvent(char *evt){
    NSString *dentifier =[NSString stringWithUTF8String:evt];
    ADJEvent *event = [ADJEvent eventWithEventToken:dentifier]; //
    [Adjust trackEvent:event];
}

// 内购二次验证
void HwAnalyticsPurchaseSecondVerify(char *number,char *token ,char *productId,char *purchaseType,char *orderId)
{
    NSLog(@"HwAnalyticsPurchaseSecondVerify number  %s",number);
    NSString *myNumber=[NSString stringWithUTF8String:number];
    NSString *myProductId =[NSString stringWithUTF8String:productId];
    NSInteger *myPurchaseType = purchaseType;
    NSString *myOrderid = [NSString stringWithUTF8String:orderId];
    NSString *myPurchaseToken = [NSString stringWithUTF8String:token];
    
    [[HwAds instance] hwAnalyticsPurchaseByNumberOfDollars:myNumber productId:myProductId purchaseType:myPurchaseType orderId:myOrderid purchaseToken:myPurchaseToken];
}
void HwAboutFirebase(char *category,char *action,char *label){
    
}




