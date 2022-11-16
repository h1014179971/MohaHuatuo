//
//  OKResult.h
//  OneKit
//
//  Created by bytedance on 2022/2/7.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger, OKResultMeta) {
    OKResultSuccess,
    OKResultFailure,
};

@interface OKResult : NSObject

+ (instancetype)new NS_UNAVAILABLE;
- (instancetype)init NS_UNAVAILABLE;

+ (OKResult *)success: (nonnull id)value;
+ (OKResult *)failure: (nonnull NSError *)error;

@property (readonly) OKResultMeta metaType;
@property (readonly) id value;
@property (readonly) NSError * error;

@end

NS_ASSUME_NONNULL_END
