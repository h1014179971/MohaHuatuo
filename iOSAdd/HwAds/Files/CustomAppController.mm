#import "UnityAppController.h"
#import "BDAutoTrack.h"
#import "BDAutoTrackConfig.h"
#import "BDAutoTrackURLHostItemSG.h"
#import <GameAnalytics/GameAnalytics.h>
#import <Bugly/Bugly.h>
#import <AdjustSdk/Adjust.h>
 
@interface CustomAppController : UnityAppController
@end
 
IMPL_APP_CONTROLLER_SUBCLASS (CustomAppController)
 
@implementation CustomAppController
 
- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    //dataplayer初始化
    /* 初始化开始 */
    BDAutoTrackConfig *config = [BDAutoTrackConfig configWithAppID:@"407563" launchOptions:launchOptions];

      /* 域名默认国内: BDAutoTrackServiceVendorCN */
    config.serviceVendor =BDAutoTrackServiceVendorSG;//新加坡
    config.appName = @"Planet Smash"; // 与您申请APPID时的app_name一致
    config.channel = @"App Store"; // iOS一般默认App Store
    config.abEnable = YES;
    config.autoTrackEnabled = NO;
    config.showDebugLog = NO; // 是否在控制台输出日志，仅调试使用。release版本请设置为 NO
    config.logNeedEncrypt = YES; // 是否加密日志，默认加密。release版本请设置为 YES
    config.gameModeEnable = YES; // 是否开启游戏模式，游戏APP建议设置为 YES
    [BDAutoTrack startTrackWithConfig:config];
    /* 初始化结束 */
    
    /*GA初始化*/
    [GameAnalytics initializeWithGameKey:@"f682ea52e18d8732c7580630b587d98b" gameSecret:@"8dbba34ced300de39d96e0645dffc359cab4221f"];
    
    [Bugly startWithAppId:@"6870cdba50"];
    
   
    return YES;
}
- (void)applicationDidBecomeActive:(UIApplication *)application{
    [super applicationDidBecomeActive:application];
    NSLog(@"hlyLog:app即将进入前台");
    [Adjust trackSubsessionStart];
}
- (void)applicationWillResignActive:(UIApplication *)application{
    [super applicationWillResignActive:application];
    NSLog(@"hlyLog:app即将进入后台");
    [Adjust trackSubsessionEnd];
}
 
@end
