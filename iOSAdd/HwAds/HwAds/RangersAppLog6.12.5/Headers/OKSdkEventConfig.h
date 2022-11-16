//
//  OKSdkEventConfig.h
//  OneKit
//
//  Created by 谭正豪 on 2021/10/18.
//

#ifndef OKSdkEventConfig_h
#define OKSdkEventConfig_h

@interface OKSdkEventConfig : NSObject

/*! @abstract The ID of your sdk that you registered
 @discussion Nonnull & required.
 */
@property (atomic, copy) NSString *sdkId;

/*! @abstract Current version of your sdk
 @discussion Nonnull & required.
 */
@property (atomic, copy) NSString *sdkVersion;

/*! @abstract The channel where your SDK will be published.
 @discussion Nonnull & require. Default : "Other"
 */
@property (atomic, strong) NSString *channel;

/*!
 @abstract The name of your SDK.
 @discussion Set this property with your SDK name. Nullable & optional.
 */
@property (atomic, copy) NSArray<NSString *> *libNames;

@end
#endif /* OKSdkEventConfig_h */
