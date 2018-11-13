//
//  UnityWrapper.h
//  GreedyiOSWrapper
//
//  Created by Lakshya on 8/16/18.
//  Copyright © 2018 GreedyGame Media. All rights reserved.
//

#ifndef UnityWrapper_h
#define UnityWrapper_h
#import <Foundation/Foundation.h>

#ifdef __cplusplus
extern "C" {
#endif
    typedef void (*DelegateAvailableCallback)(const char* campaignId);
    typedef void (*DelegateUnavailableCallback)();
    typedef void (*DelegateErrorCallback)(const char* error);
    void framework_setAvailableDelegate(DelegateAvailableCallback delegateAvailable);
    void framework_setUnavailableDelegate(DelegateUnavailableCallback delegateUnavailable);
    void framework_setErrorDelegate(DelegateErrorCallback delegateError);
    void framework_trigger_unavailable();
    void framework_trigger_available(const char* campaignId);
    void framework_trigger_error(const char* error);
#ifdef __cplusplus
}
#endif

@protocol CampaignListener <NSObject>
- (void)onAvailable:(const char*)campaignId;
- (void)onUnavailable;
- (void)onError:(const char*)error;
@end

@interface UnityWrapper : NSObject<CampaignListener>
@end

@interface UnityObject : NSObject
+(void)sendAvailableCallback:(const char*)campaignId;
+(void)sendUnavailableCallback;
+(void)sendErrorCallback:(const char*)error;

@end


#endif /* UnityWrapper_h */
