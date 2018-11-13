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
@end

@implementation UnityObject
+(void)sendAvailableCallback:(const char*)campaignId{
    NSLog(@"Campaign Available callback triggered");
    framework_trigger_available(campaignId);
}

+(void)sendUnavailableCallback{
    NSLog(@"Campaign Unavailable callback triggereddd");
    const char* reason = "not available";
    framework_trigger_unavailable();
}

+(void)sendErrorCallback:(const char*)error{
    NSLog(@"Campaign Error callback triggered");
    framework_trigger_error(error);
}
@end
