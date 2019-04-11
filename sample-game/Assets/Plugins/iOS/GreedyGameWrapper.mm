//
//  GreedyGameWrapper.mm
//  GreedyiOSWrapper
//
//  Created by Lakshya on 8/2/18.
//  Copyright © 2018 GreedyGame Media. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GreedyiOSWrapper/GreedyiOSWrapper-Swift.h>

#pragma mark -C interface

extern "C"{
    void _gg_load(){
        printf("[GreedyGameMM]GG SDK Loaded");
        [[GreedyGameWrapper wrapper] load];
    }

    void _gg_setAppId(const char *appId){
        printf("[GreedyGameMM]Setting game id: %s\n", appId);
        [[GreedyGameWrapper wrapper] appId:[NSString stringWithUTF8String:appId]];
    }
    
    void _gg_addUnit(const char *unitId){
        printf("[GreedyGameMM]Unit ID Added: %s\n", unitId);
        [[GreedyGameWrapper wrapper] addUnitId:[NSString stringWithUTF8String:unitId]];
    }
    
    void _gg_enableAdmob(bool enable){
        printf("[GreedyGameMM]Admob Enabled: %d\n", enable);
        [[GreedyGameWrapper wrapper] enableAdmob:enable];
    }
    
    char * _gg_getPath(const char *unitId){
        printf("[GreedyGameMM]Get Path for ID: %s\n", unitId);
        NSString* str_path = [[GreedyGameWrapper wrapper] getPath:[NSString stringWithUTF8String:unitId]];
        if(str_path != nil){
            char* path = strdup([str_path UTF8String]);
            printf("[GreedyGameMM] Returning Path: %s\n",path);
            return path;
        }
        else{
            NSString* empty_path = @"";
            return strdup([empty_path UTF8String]);
        }
    }

    
    void _gg_showUII(const char *unitId){
        printf("[GreedyGameMM]Show UII for ID: %s\n", unitId);
        [[GreedyGameWrapper wrapper] showUII:[NSString stringWithUTF8String:unitId]];
    }
    
    void _gg_setCoppaFilter(bool enable) {
        printf("[GreedyGameMM]Coppa Enabled: %d\n", enable);
        [[GreedyGameWrapper wrapper] enableCoppa:enable];
    }

    void _gg_setGGNpa(bool enable) {
        printf("[GreedyGameMM] GG NPA : %d\n", enable);
        [[GreedyGameWrapper wrapper] setGGNpa:enable];
    }

    void _gg_gameEngine(const char *engineName){
        printf("[GreedyGameMM] Engine Name : %s\n", engineName);
        [[GreedyGameWrapper wrapper] gameEngine:[NSString stringWithUTF8String:engineName]];
    }

    void _gg_engineVersion(const char *engineVersion){
        printf("[GreedyGameMM] Engine Version: %s\n", engineVersion);
        [[GreedyGameWrapper wrapper] engineVersion:[NSString stringWithUTF8String:engineVersion]];
    }

    bool _gg_recordImpression(const char *unitId){
        printf("[GreedyGameMM] gg record impression");
        [[GreedyGameWrapper wrapper] recordImpression:[NSString stringWithUTF8String:unitId]];
        return false;
    }

    bool _gg_isAdReady(){
        printf("[GreedyGameMM] isAdReady");
        return [[GreedyGameWrapper wrapper] isAdReady];
    }

    bool _gg_isAdRendered(){
        printf("[GreedyGameMM] isAdRendered");
        return [[GreedyGameWrapper wrapper] isAdRendered];
    }

    bool _gg_apply(){
        printf("[GreedyGameMM] Apply");
        [[GreedyGameWrapper wrapper] apply];
    }

    bool _gg_destroy(){
        printf("[GreedyGameMM] Destroy");
        [[GreedyGameWrapper wrapper] destroy];
    }

}
