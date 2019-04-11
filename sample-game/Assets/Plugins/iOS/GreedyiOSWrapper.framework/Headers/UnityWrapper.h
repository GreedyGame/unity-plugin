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
    typedef void (*DelegateAssetAvailableCallback)(const char* assetDetails);
    typedef void (*DelegateImpressionCallback)();
    typedef void (*DelegateDestroyCallback)();

    void framework_setAvailableDelegate(DelegateAvailableCallback delegateAvailable);
    void framework_setUnavailableDelegate(DelegateUnavailableCallback delegateUnavailable);
    void framework_setErrorDelegate(DelegateErrorCallback delegateError);
    void framework_setAssetAvailableDelegate(DelegateAssetAvailableCallback delegateassetAvailable);
    void framework_setImpressionDelegate(DelegateImpressionCallback delegateImpression);
    void framework_setDestroyDelegate(DelegateDestroyCallback delegateDestroy);
    
    void framework_trigger_available(const char* campaignId);
    void framework_trigger_unavailable();
    void framework_trigger_error(const char* error);
    void framework_trigger_assetAvailable(const char* assetDetails);
    void framework_trigger_impression();
    void framework_trigger_destroy();

#ifdef __cplusplus
}
#endif

@protocol WrapperAdListener <NSObject>
- (void)onAvailable:(const char*)campaignId;
- (void)onUnavailable;
- (void)onError:(const char*)error;
- (void)onAssetAvailable:(const char*)assetDetails;
- (void)onImpression;
- (void)onDestroy;
@end

@interface UnityWrapper : NSObject<WrapperAdListener>
@end

@interface UnityObject : NSObject
+(void)sendAvailableCallback:(const char*)campaignId;
+(void)sendUnavailableCallback;
+(void)sendErrorCallback:(const char*)error;
+(void)sendAssetAvailableCallback:(const char*)assetDetails;
+(void)sendImpressionCallback;
+(void)sendDestroyCallback;
@end


#endif /* UnityWrapper_h */
