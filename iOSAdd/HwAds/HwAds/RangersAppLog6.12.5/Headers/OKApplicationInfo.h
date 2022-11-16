//
//  OKApplicationInfo.h
//  OKStartUp
//
//  Created by bob on 2020/1/14.
//

#import <Foundation/Foundation.h>
#import "OKServices.h"
NS_ASSUME_NONNULL_BEGIN

/// 存储应用信息
/// 默认值从bundle或onekit-config.plist中读取
@interface OKApplicationInfo : NSObject

/* 默认值从onekit-config.plist中读取 */
@property (nonatomic, copy) NSString *appID;
@property (nonatomic, copy) NSString *accessKey;  // mars_access_key
@property (nonatomic, copy) NSString *secretKey;  // mars_secret_key
@property (nonatomic, copy, nullable) NSDictionary *serviceInfo;

/* 默认值从bundle中读取 */
@property (nonatomic, copy) NSString *appName;
@property (nonatomic, copy) NSString *appDisplayName;
@property (nonatomic, copy) NSString *channel;
@property (nonatomic, copy) NSString *appVersion;
@property (nonatomic, copy) NSString *buildVersion;
@property (nonatomic, copy) NSString *buildVersionCode;
@property (nonatomic, copy) NSString *bundleIdentifier;
@property (nonatomic, assign) BOOL isInhouseApp;
@property (nonatomic, assign) BOOL isI18NApp;

@property (nonatomic, copy) NSString *deviceModel;/// return string like "iPhone9,1"
@property (nonatomic, copy) NSString *devicePlatform;/// return "iPod", "iPad" or "iPhone"
@property (nonatomic, copy) NSString *systemVersion;
@property (nonatomic, copy, nullable) NSString *sharingKeyChainGroup;

+ (instancetype)sharedInstance;

@end

NS_ASSUME_NONNULL_END
