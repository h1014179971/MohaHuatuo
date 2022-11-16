//
//  OKStartUpTask.h
//  OKStartUp
//
//  Created by bob on 2020/1/13.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "OKStartUpFunction.h"

NS_ASSUME_NONNULL_BEGIN

typedef NSInteger OKStartUpTaskPriority NS_TYPED_ENUM;

FOUNDATION_EXTERN OKStartUpTaskPriority const OKStartUpTaskPriorityDefault __deprecated_msg("已过期，为了兼容性保留，不会有效果"); /// 默认优先级
FOUNDATION_EXTERN OKStartUpTaskPriority const OKStartUpTaskPriorityLow __deprecated_msg("已过期，为了兼容性保留，不会有效果"); /// 低优先级
FOUNDATION_EXTERN OKStartUpTaskPriority const OKStartUpTaskPriorityHigh __deprecated_msg("已过期，为了兼容性保留，不会有效果"); /// 高优先级


/// 自定义Task 参考如下,在OKAppAddStartUpTaskFunction中
/// 仅做简单配置，请勿进行耗时操作
///
/**
#import <OneKit/OKStartUpTask.h>
 
 OKAppTaskAddFunction() {
    task = xx
    task.xx = xxx
    [task scheduleTask];
 }
 */

@interface OKStartUpTask : NSObject


@property (nonatomic, copy) NSString *taskIdentifier;   /// default =className
@property (nonatomic) NSSet<Protocol *> *dependencies;
@property (nonatomic) NSSet<Protocol *> *optional;
@property (nonatomic) NSSet<Protocol *> *offering;
@property (nonatomic, readonly) BOOL visited;
@property (nonatomic, readonly) BOOL executed;

@property (nonatomic, assign) OKStartUpTaskPriority priority __deprecated_msg("已过期，为了兼容性保留，不会有效果");
@property (nonatomic, assign) BOOL syncTask __deprecated_msg("已过期，为了兼容性保留，所有任务都会是同步的，因此业务方应避免耗时操作");
@property (nonatomic, assign) BOOL enabled __deprecated_msg("已过期，为了兼容性保留，不会有效果");
@property(nonatomic, copy, nullable) dispatch_block_t customTaskBeforeBlock __deprecated_msg("已过期，为了兼容性保留，不会有效果");
@property(nonatomic, copy, nullable) dispatch_block_t customTaskAfterBlock __deprecated_msg("已过期，为了兼容性保留，不会有效果");


- (void)start __deprecated_msg("已过期，为了兼容性保留。继承说明：派生类只实现`startWithLaunchOptions:`即可。");
- (void)startWithLaunchOptions:(nullable NSDictionary<UIApplicationLaunchOptionsKey,id> *)launchOptions;
- (void)startApplication:(nullable id) application withLaunchOptions:(nullable NSDictionary<UIApplicationLaunchOptionsKey, id> *)launchOptions __deprecated_msg("已过期，为了兼容性保留。继承说明：派生类只实现`startWithLaunchOptions:`即可。");

- (void)scheduleTask NS_REQUIRES_SUPER;

@end

NS_ASSUME_NONNULL_END
