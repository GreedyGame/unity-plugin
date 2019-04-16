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
    void _gg_initialize(){
        printf("[GreedyGameMM]GG SDK Initialized ");
        [[GreedyGameWrapper wrapper] initialize];
    }

    void _gg_setGameId(const char *gameId){
        printf("[GreedyGameMM]Setting game id: %s\n", gameId);
        [[GreedyGameWrapper wrapper] setGameId:[NSString stringWithUTF8String:gameId]];
    }
    
    void _gg_addUnit(const char *unitId){
        printf("[GreedyGameMM]Unit ID Added: %s\n", unitId);
        [[GreedyGameWrapper wrapper] addUnit:[NSString stringWithUTF8String:unitId]];
    }
    
    void _gg_enableAdmob(bool enableAdmob){
        printf("[GreedyGameMM]Admob Enabled: %d\n", enableAdmob);
        [[GreedyGameWrapper wrapper] enableAdmob:enableAdmob];
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
    
    void _gg_refresh(){
        printf("[GreedyGameMM]GG SDK Refresh\n");
        [[GreedyGameWrapper wrapper] refresh];
    }

    void _gg_setCoppaFilter(bool enable) {
        printf("[GreedyGameMM]Coppa Enabled: %d\n", enable);
        [[GreedyGameWrapper wrapper] setCoppaFilter:enable];
    }

    void _gg_setGGNpa(bool enable) {
        printf("[GreedyGameMM]Coppa Enabled: %d\n", enable);
        [[GreedyGameWrapper wrapper] setGGNpa:enable];
    }

    void _gg_gameEngine(const char *engineName){
        printf("[GreedyGameMM]Show UII for ID: %s\n", engineName);
        [[GreedyGameWrapper wrapper] setEngineName:[NSString stringWithUTF8String:engineName]];
    }

    void _gg_engineVersion(const char *engineVersion){
        printf("[GreedyGameMM]Show UII for ID: %s\n", engineVersion);
        [[GreedyGameWrapper wrapper] setEngineVersion:[NSString stringWithUTF8String:engineVersion]];
    }

}
