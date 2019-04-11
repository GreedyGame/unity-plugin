//
//  UnityWrapper.m
//  GreedyiOSWrapper
//
//  Created by Lakshya on 8/16/18.
//  Copyright © 2018 GreedyGame Media. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "UnityWrapper.h"

DelegateAvailableCallback delegateAvailable = NULL;
DelegateUnavailableCallback delegateUnavailable = NULL;
DelegateErrorCallback delegateError = NULL;
DelegateAssetAvailableCallback delegateAssetAvailable = NULL;
DelegateImpressionCallback delegateImpressionAvailable = NULL;
DelegateDestroyCallback delegateDestroy = NULL;

static UnityWrapper *__delegate = nil;

void framework_trigger_unavailable() {
    NSLog(@"Delegate onUnavailable called. 1");
    if (__delegate && [__delegate respondsToSelector:@selector(onUnavailable)]) {
        NSLog(@"Delegate onUnavailable called.");
        [__delegate onUnavailable];
    }
}

void framework_trigger_available(const char* campaignId){
    if (__delegate && [__delegate respondsToSelector:@selector(onAvailable:)]) {
        NSLog(@"Delegate onAvailable called with message = %s", campaignId);
        [__delegate onAvailable:campaignId];
    }
}

void framework_trigger_error(const char* error){
    if (__delegate && [__delegate respondsToSelector:@selector(onError:)]) {
        NSLog(@"Delegate onError called with message = %s", error);
        [__delegate onError:error];
    }
}

void framework_trigger_assetAvailable(const char* assetDetails){
    if (__delegate && [__delegate respondsToSelector:@selector(onAssetAvailable:)]){
        NSLog(@"Delegate onAssetAvailable called");
        [__delegate onAssetAvailable:assetDetails];
    }
}

void framework_trigger_impression(){
    if (__delegate && [__delegate respondsToSelector:@selector(onImpression)]){
        NSLog(@"Delegate onImpression called");
        [__delegate onImpression];
    }
}

void framework_trigger_destroy() {
    if (__delegate && [__delegate respondsToSelector:@selector(onDestroy)]){
        NSLog(@"Delegate onDestroy called");
        [__delegate onDestroy];
    }
}

void framework_setAvailableDelegate(DelegateAvailableCallback callback) {
    if (!__delegate) {
        __delegate = [[UnityWrapper alloc] init];
        NSLog(@"Delegate created");
    }
    NSLog(@"Available Delegate assigned");
    delegateAvailable = callback;
}

void framework_setUnavailableDelegate(DelegateUnavailableCallback callback){
    if (!__delegate) {
        __delegate = [[UnityWrapper alloc] init];
        NSLog(@"Delegate created");
    }
    NSLog(@"Unavailable Delegate assigned");
    delegateUnavailable = callback;
}

void framework_setErrorDelegate(DelegateErrorCallback callback){
    if (!__delegate) {
        __delegate = [[UnityWrapper alloc] init];
        NSLog(@"Delegate created");
    }
    NSLog(@"Error Delegate assigned");
    delegateError = callback;
}

void framework_setAssetAvailableDelegate(DelegateAssetAvailableCallback callback) {
    if (!__delegate) {
        __delegate = [[UnityWrapper alloc] init];
        NSLog(@"Delegate created");
    }
    NSLog(@"Asset Available Delegate assigned");
    delegateAssetAvailable = callback;
}

void framework_setImpressionDelegate(DelegateImpressionCallback callback){
    if(!__delegate) {
        __delegate = [[UnityWrapper alloc] init];
        NSLog(@"Delegate created");
    }
    NSLog(@"Impression Delegate assigned");
    delegateImpressionAvailable = callback;
}

void framework_setDestroyDelegate(DelegateDestroyCallback callback){
    if(!__delegate) {
        __delegate = [[UnityWrapper alloc]init];
        NSLog(@"Delegate created");
    }
    NSLog(@"Destroy Delegate assigned");
    delegateDestroy = callback;
}


@implementation UnityWrapper
-(void)onAvailable:(const char*)campaignId {
    if (delegateAvailable != NULL) {
        delegateAvailable(campaignId);
        NSLog(@"C onAvailable");
    }
}

- (void)onUnavailable{
    if(delegateUnavailable != NULL){
        delegateUnavailable();
        NSLog(@"C onUnavailable");
    }
}

- (void)onError:(const char*)error{
    if(delegateError != NULL){
        delegateError(error);
        NSLog(@"C onError = %s", error);
    }
}

- (void)onAssetAvailable:(const char *)assetDetails {
    if (delegateAssetAvailable != NULL){
        delegateAssetAvailable(assetDetails);
        NSLog(@"C onAssetAvailable");
    }
}

-(void)onImpression {
    if (delegateImpressionAvailable != NULL){
        delegateImpressionAvailable();
        NSLog(@"C onImpression");
    }
}
- (void)onDestroy{
    if (delegateDestroy != NULL){
        delegateDestroy();
        NSLog(@"C onDestroy");
    }
}
@end

@implementation UnityObject
+(void)sendAvailableCallback:(const char*)campaignId{
    NSLog(@"Campaign Available callback triggered");
    framework_trigger_available(campaignId);
}

+(void)sendUnavailableCallback{
    NSLog(@"Campaign Unavailable callback triggered");
    framework_trigger_unavailable();
}

+(void)sendErrorCallback:(const char*)error{
    NSLog(@"Campaign Error callback triggered");
    framework_trigger_error(error);
}

+ (void)sendAssetAvailableCallback:(const char *)assetDetails{
    NSLog(@"Campaign assetDetails Available callback triggered");
    framework_trigger_assetAvailable(assetDetails);
}

+ (void)sendImpressionCallback{
    NSLog(@"Campaign impreesion callback triggered");
    framework_trigger_impression();
}

+ (void)sendDestroyCallback{
    NSLog(@"Campaign destroy callback triggered");
    framework_trigger_destroy();
}

@end
